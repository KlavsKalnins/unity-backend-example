using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ApplyTexture : MonoBehaviour
{
    public static string baseURL = "http://localhost:3000/";
    public enum ObjectTypes {
        cube,
        sphere,
    }
    [Serializable]
    public class JsonObj
    {
        public string objectType;
        public string material;
    }
    [SerializeField] MeshFilter mf;
    [SerializeField] RawImage rawImg;
    [SerializeField] GameObject buttonPanel;
    [SerializeField] GameObject buttonPrefabs;

    [SerializeField] Material mat;
    void Start()
    {
        // setup buttons
        foreach (ObjectTypes objType in (ObjectTypes[]) Enum.GetValues(typeof(ObjectTypes)))
        {
            var typeName = objType.ToString();
            var btn = Instantiate(buttonPrefabs);
            btn.name = typeName + "Button";
            btn.transform.SetParent(buttonPanel.transform);
            btn.GetComponentInChildren<TMP_Text>().text = typeName;
            btn.GetComponent<Button>().onClick.AddListener(delegate {
                GetObjectType(typeName);
            });
        }
    }
    public void GetObjectType(string name) => StartCoroutine(GetRequest(baseURL + name));

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success) {
            var myObject = JsonUtility.FromJson<JsonObj>(request.downloadHandler.text);
            byte[] imageBytes = Convert.FromBase64String(myObject.material);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage( imageBytes );
            rawImg.texture = tex;
            mat.SetTexture("_MainTex", tex);
            SetShape(myObject.objectType);
        } else {
            Debug.Log("Make sure that backend is running and is on port 3000");
        }
    }
    private void SetShape(String shapeType) {
        var shape = PrimitiveType.Cube;
        switch (shapeType) {
            case "Cube":
                shape = PrimitiveType.Cube;
                break;
            case "Sphere":
                shape = PrimitiveType.Sphere;
                break;
        }
        GameObject objSpecificShape = GameObject.CreatePrimitive(shape);
        mf.mesh = objSpecificShape.GetComponent<MeshFilter>().mesh;
        Destroy(objSpecificShape);
    }
}

using UnityEngine;

public class CubeScript : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(speed,speed,speed) * Time.deltaTime, Space.World);
    }
    
}

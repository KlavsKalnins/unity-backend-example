import express from "express";
import { objectType } from "./types";
const app = express();
const path = require("path");
const fs = require("fs");

const base64_encode = function (path: String) {
  var bitmap = fs.readFileSync(path);
  return Buffer.from(bitmap).toString("base64");
};

const dir = path.join(__dirname, "public");
app.use(express.static(dir));

app.get("/", (req, res) => {
  res.send(`
  <h1>Backend running</h1>
  <p>goto <strong><a href="/cube">Cube</a></strong> or <strong><a href="/sphere">Sphere</a></strong> to see json</p>
  <p>run unity example to see images delivered onto objects</p>
  `);
});

Object.keys(objectType).map((key) => {
  app.get("/" + key, (req, res) => {
    var base64 = base64_encode("public/img/" + key + ".jpg");
    res.json({ objectType: key, material: base64 });
  });
});

const port = process.env.PORT || 3000;

app.listen(port, () => console.log(`listening to PORT ${port}`));

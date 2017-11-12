using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageImport : MonoBehaviour {

    public string url = "https://mitten25.github.io/img/DumpedIcon.PNG";
    IEnumerator Start()
    {
        WWW www = new WWW(url);
        yield return www;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
    }
}

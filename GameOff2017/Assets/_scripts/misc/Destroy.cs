using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyObj());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}

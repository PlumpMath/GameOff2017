using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    //public List<GameObject> lives;

    public GameObject life;

    private void Awake()
    {
        //singleton setup
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        //Instantiate(lives, GameObject.Find("lives").transform.position, Quaternion.identity, GameObject.Find("lives").transform);
	}
	
	// Update is called once per frame
	void Update () {

        //update camera
        if (GetComponent<Canvas>().worldCamera == null)
            GetComponent<Canvas>().worldCamera = Camera.current;

	}

    public void UpdateLives()
    {
        foreach (Transform child in GameObject.Find("lives").transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < LevelManager.instance.lives; i++)
            Instantiate(life, GameObject.Find("lives").transform.position, Quaternion.identity, GameObject.Find("lives").transform);
    }
}

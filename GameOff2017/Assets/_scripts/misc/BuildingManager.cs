using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    //singleton
    public static BuildingManager instance;

    private void Awake()
    {
        //singleton setup
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSprite(string style)
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load(style + "_building", typeof(Sprite)) as Sprite;
    }
}

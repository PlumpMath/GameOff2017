using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    //singleton
    public static EnvironmentManager instance;

    //array of all levels
    [HideInInspector]
    public Object[] levels;

    private void Awake()
    {
        //singleton setup
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        //load levels from resources folder
        levels = Resources.LoadAll("levels");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //input manager
    private Player input_manager;

    // Use this for initialization
    void Start () {

        //initialize input manage
        input_manager = ReInput.players.GetPlayer(0);

    }
	
	// Update is called once per frame
	void Update () {

        //Insert coin to continue
		if (input_manager.GetButtonDown("jump")){
            SceneManager.LoadScene("level");
        }
	}
}

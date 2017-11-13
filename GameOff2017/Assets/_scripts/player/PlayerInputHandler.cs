using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInputHandler : MonoBehaviour
{
    //singleton
    public static PlayerInputHandler instance;

    //input manager
    private Player input;

    //inputs
    [HideInInspector]
    public float h_dir;
    [HideInInspector]
    public float v_dir;
    [HideInInspector]
    public bool jump;
    [HideInInspector]
    public bool attack;

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
        //initialize input manage
        input = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        //process input
        h_dir = input.GetAxis("h_dir");
        v_dir = input.GetAxis("v_dir");
        jump = input.GetButtonDown("jump");
        attack = input.GetButtonDown("attack");
    }
}

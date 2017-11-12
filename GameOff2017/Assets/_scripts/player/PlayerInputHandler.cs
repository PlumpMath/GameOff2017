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
    private float move_horizontal;
    private float move_vertical;
    private bool jump;
    private bool attack;

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
        move_horizontal = input.GetAxis("move_horizontal");
        move_vertical = input.GetAxis("move_vertical");
        jump = input.GetButtonDown("jump");
        attack = input.GetButtonDown("attack");
    }
}

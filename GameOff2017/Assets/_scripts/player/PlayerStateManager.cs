using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    //singleton
    public static PlayerStateManager instance;

    //state manager
    public enum player_states { IDLE, RUNNING, JUMPING, THROWING };
    [HideInInspector]
    public player_states current_state;

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
        //set default state
        current_state = player_states.IDLE;
    }
}

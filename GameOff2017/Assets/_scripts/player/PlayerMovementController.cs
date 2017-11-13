using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    //rigidbody
    private Rigidbody2D rb;

    //player values
    public float move_speed;
    public float jump_force;

    //player attributes
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool jumped;

    private void Start()
    {
        //initialize components
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HorizontalMovement();
    }

    private void HorizontalMovement()
    {
        Vector2 velocity = new Vector2(PlayerInputHandler.instance.h_dir * move_speed * Time.deltaTime, rb.velocity.y);
        rb.velocity = velocity;
    }

    private void Jump()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //rigidbody
    private Rigidbody2D rb;

    //singleton
    public static PlayerController instance;

    //direction state
    public enum direction { LEFT, RIGHT};
    [HideInInspector]
    public direction current_direction;

    //pizza
    public GameObject pizza;
    public Transform pizza_spawn;
    public float pizza_spawn_distance;

    //player values
    public float move_speed;
    public float jump_force;

    //player attributes
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool can_attack;

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
        //initialize rigidbody
        rb = gameObject.GetComponent<Rigidbody2D>();

        //set initial direction
        current_direction = direction.RIGHT;

        //set attributes
        can_attack = true;
    }

    private void Update()
    {
        //update movement
        HorizontalMovement();
        if (PlayerInputHandler.instance.jump && grounded)
            Jump();

        //throw pizza
        if (PlayerInputHandler.instance.attack && can_attack)
            Attack();
    }

    private void HorizontalMovement()
    {
        Vector2 velocity = new Vector2(PlayerInputHandler.instance.h_dir * move_speed * Time.deltaTime, rb.velocity.y);

        if (velocity.x > 0 && current_direction != direction.RIGHT)
        {
            current_direction = direction.RIGHT;
            pizza_spawn.transform.localPosition = new Vector2(pizza_spawn_distance, pizza_spawn.transform.localPosition.y);
        }
        else if (velocity.x < 0 && current_direction != direction.LEFT)
        {
            current_direction = direction.LEFT;
            pizza_spawn.transform.localPosition = new Vector2(-pizza_spawn_distance, pizza_spawn.transform.localPosition.y);
        }

        rb.velocity = velocity;

    }

    private void Jump()
    {
        grounded = false;
        rb.AddForce(transform.up * jump_force, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        can_attack = false;
        Instantiate(pizza, pizza_spawn.position, pizza_spawn.rotation);
    }
}

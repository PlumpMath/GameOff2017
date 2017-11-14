using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //components
    private Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator anim;

    //singleton
    public static PlayerController instance;

    //player state
    private bool dead;

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
    public float death_force;

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
        if (!dead)
        {
            //update movement
            HorizontalMovement();
            if (PlayerInputHandler.instance.jump && grounded)
                Jump();

            //throw pizza
            if (PlayerInputHandler.instance.attack && can_attack)
                Attack();
        }
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

        if(grounded)
        {
            if(anim.GetBool("jumping") == true)
                anim.SetBool("jumping", false);
            if(velocity.x == 0 && anim.GetBool("idle") == false)
            {
                anim.SetBool("idle", true);
                anim.SetBool("walking", false);
            }
            else if(velocity.x != 0 && anim.GetBool("walking") == false)
            {
                anim.SetBool("idle", false);
                anim.SetBool("walking", true);
            }
        }

        if (current_direction == direction.LEFT && !sprite.flipX)
            sprite.flipX = true;
        else if (current_direction == direction.RIGHT && sprite.flipX)
            sprite.flipX = false;
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

    private void Death()
    {
        dead = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * death_force, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
            Death();
    }
}

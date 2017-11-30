using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //components
    [HideInInspector]
    public Rigidbody2D rb;
    [Header("COMPONENTS")]
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
    [Header("PIZZA")]
    public GameObject pizza;
    public Transform pizza_spawn;
    public float pizza_spawn_distance;

    //player values
    [Header("MOVEMENT")]
    public float move_speed;
    public float jump_force;
    public float death_force;

    //player attributes
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool can_attack;
    private bool can_move;
    [HideInInspector]
    public bool beat_level;

    //sfx
    private AudioSource audio;
    [Header("SFX")]
    public AudioClip jump_sound;
    public AudioClip attack_sound;
    public AudioClip death_sound;

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
        can_move = true;

        //get audio source
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!can_move)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            anim.SetBool("jumping", false);
            anim.SetBool("idle", true);
            anim.SetBool("walking", false);
            return;
        }

        if (!dead)
        {
            //update movement
            HorizontalMovement();
            VerticalMovement();

            //throw pizza
            if (PlayerInputHandler.instance.attack)
            {
                if (can_attack)
                    Attack();
                else
                    Recall();
            }

            if(!grounded && !anim.GetBool("jumping"))
            {
                anim.SetBool("jumping", true);
                anim.SetBool("idle", false);
                anim.SetBool("walking", false);
            }
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
            if(anim.GetBool("jumping"))
                anim.SetBool("jumping", false);
            if(velocity.x == 0 && !anim.GetBool("idle"))
            {
                anim.SetBool("idle", true);
                anim.SetBool("walking", false);
            }
            else if(velocity.x != 0 && !anim.GetBool("walking"))
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

    private void VerticalMovement()
    {
        //jump
        if (PlayerInputHandler.instance.jump && grounded)
        {
            audio.PlayOneShot(jump_sound);
            grounded = false;
            rb.AddForce(transform.up * jump_force, ForceMode2D.Impulse);
        }

        //check for falling
        if (grounded && rb.velocity.y != 0)
            grounded = false;
    }

    private void Attack()
    {
        can_attack = false;
        Instantiate(pizza, pizza_spawn.position, pizza_spawn.rotation);
    }

    private void Recall()
    {
        PizzaController.instance.Recall();
    }

    private IEnumerator Death()
    {
        audio.PlayOneShot(death_sound);
        dead = true;
        //can_move = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0.5f;
        rb.AddForce(transform.up * death_force, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1);

        //remove life
        LevelManager.instance.lives--;
        LevelManager.instance.BreakdownLevel();
    }

    public IEnumerator LevelComplete()
    {
        beat_level = true;
        can_move = false;
        yield return new WaitForSeconds(1);
        LevelManager.instance.current_level = null;
        LevelManager.instance.BreakdownLevel();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("boundary"))
            StartCoroutine(Death());
        /*
        else if (collision.gameObject.CompareTag("end_level"))
        {
            StartCoroutine(LevelComplete());
        }
        */
    }
}

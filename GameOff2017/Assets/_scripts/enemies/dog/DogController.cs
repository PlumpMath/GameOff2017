using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : Enemy {

    // Enemy States
    public enum direction { LEFT, RIGHT };
    public direction current_direction;

    // Enemy Attributes
    public float speed;

    //death sound
    public AudioClip death_sound;

    // Use this for initialization
    void Start () {
        StartCoroutine(Descend());

        //set sprite
        sprite = transform.Find("enemy_sprite").GetComponent<SpriteRenderer>();

        if ((this.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).x > 0)
        {
            current_direction = direction.LEFT;
        }
        else
            current_direction = direction.RIGHT;
    }
	
	// Update is called once per frame
	void Update () {
        if (current_direction == direction.LEFT && !sprite.flipX)
            sprite.flipX = true;
        else if (current_direction == direction.RIGHT && sprite.flipX)
            sprite.flipX = false;
    }

    private IEnumerator Descend()
    {
        while (!dead)
        {
            if (current_direction == direction.RIGHT)
            {
                this.transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
            }
            yield return null;
        }
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (current_direction == direction.RIGHT)
                current_direction = direction.LEFT;
            else
                current_direction = direction.RIGHT;
        }
    }

    public override void Death()
    {
        GetComponent<AudioSource>().PlayOneShot(death_sound);

        base.Death();
        
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        StartCoroutine(Remove());
    }

    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}

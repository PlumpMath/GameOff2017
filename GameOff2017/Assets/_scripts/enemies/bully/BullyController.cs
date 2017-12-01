using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullyController : Enemy {

    // Enemy States
    public enum direction { LEFT, RIGHT};
    public direction current_direction;

    // Enemy Attributes
    public float speed;

    // Raycast
    RaycastHit2D rayCastLeft;
    RaycastHit2D rayCastRight;

    //death sound
    public AudioClip death_sound;

	// Use this for initialization
	void Start () {

        // Randomize Direction
        if (Random.Range(0, 1) == 0)
            current_direction = direction.LEFT;
        else
            current_direction = direction.RIGHT;

        StartCoroutine(Patrol());

        //set sprite
        sprite = transform.Find("enemy_sprite").GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (current_direction == direction.LEFT && sprite.flipX)
            sprite.flipX = false;
        else if (current_direction == direction.RIGHT && !sprite.flipX)
            sprite.flipX = true;
	}

    public IEnumerator Patrol()
    {
        Vector2 dir = this.transform.TransformDirection(Vector2.down) * 1f;

        while (!dead)
        {
            Vector3 leftBase = new Vector3(transform.position.x - .3f, transform.position.y, transform.position.z);
            Vector3 rightBase = new Vector3(transform.position.x + .3f, transform.position.y, transform.position.z);
            rayCastLeft = Physics2D.Raycast(leftBase, Vector2.down, 1f);
            rayCastRight = Physics2D.Raycast(rightBase, Vector2.down, 1f);
            Debug.DrawRay(leftBase, dir, Color.red);
            Debug.DrawRay(rightBase, dir, Color.red);

            if (current_direction == direction.RIGHT)
            {
                this.transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                if (rayCastRight.collider == null || rayCastRight.collider.transform.tag != "ground")// || rayCastRight.collider.transform.tag == "enemy")
                {
                    current_direction = direction.LEFT;
                }
            }
            else
            {
                this.transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
                if (rayCastLeft.collider == null || rayCastLeft.collider.transform.tag != "ground")// || rayCastRight.collider.transform.tag == "enemy")
                {
                    current_direction = direction.RIGHT;
                }
            }

            yield return null;
        }
        yield return null;
    }

    public override void Death()
    {
        GetComponent<AudioSource>().PlayOneShot(death_sound);

        base.Death();

        anim.SetBool("dead", true);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        StartCoroutine(Remove());
    }

    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
            if (current_direction == direction.RIGHT)
                current_direction = direction.LEFT;
            else
                current_direction = direction.RIGHT;
    }
}

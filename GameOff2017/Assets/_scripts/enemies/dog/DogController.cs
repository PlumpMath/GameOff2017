﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : Enemy {

    // Enemy States
    public enum direction { LEFT, RIGHT };
    public direction current_direction;

    // Enemy Attributes
    public float speed;

    // Use this for initialization
    void Start () {
        StartCoroutine(Descend());

        //set sprite
        sprite = transform.Find("enemy_sprite").GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
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
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("enemy"))
        {
            if (current_direction == direction.RIGHT)
                current_direction = direction.LEFT;
            else
                current_direction = direction.RIGHT;
        }
    }

    public override void Death()
    {
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
}

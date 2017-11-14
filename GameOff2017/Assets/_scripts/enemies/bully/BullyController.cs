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

	// Use this for initialization
	void Start () {

        // Randomize Direction
        if (Random.Range(0, 1) == 0)
            current_direction = direction.LEFT;
        else
            current_direction = direction.RIGHT;

        StartCoroutine(Patrol());
    }
	
	// Update is called once per frame
	void Update () {

	}

    public IEnumerator Patrol()
    {
        Vector2 dir = this.transform.TransformDirection(Vector2.down) * 1f;

        while (!dead)
        {
            Vector3 leftBase = new Vector3(transform.position.x - .2f, transform.position.y, transform.position.z);
            Vector3 rightBase = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z);
            rayCastLeft = Physics2D.Raycast(leftBase, Vector2.down, 1f);
            rayCastRight = Physics2D.Raycast(rightBase, Vector2.down, 1f);
            Debug.DrawRay(leftBase, dir, Color.red);
            Debug.DrawRay(rightBase, dir, Color.red);

            if (current_direction == direction.RIGHT)
            {
                this.transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                if (rayCastRight.collider == null || rayCastRight.collider.transform.tag != "ground")
                {
                    current_direction = direction.LEFT;
                }
            }
            else
            {
                this.transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
                if (rayCastLeft.collider == null || rayCastLeft.collider.transform.tag != "ground")
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
        base.Death();

        anim.SetBool("dead", true);
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Remove());
    }

    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}

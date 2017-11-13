using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    //player reference
    private PlayerMovementController player;

    //player size fields
    [HideInInspector]
    public float player_height;

    //object currently standing on
    [HideInInspector]
    public string standing_on;

    private void Start()
    {
        //get player component
        player = transform.parent.GetComponent<PlayerMovementController>();

        //get player height
        player_height = player.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        standing_on = collision.gameObject.tag;

        //check for ground collision
        if (collision.gameObject.tag == "Ground")
        {
            if (player.transform.position.y - (player_height / 2) > collision.gameObject.transform.position.y +
                (collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2))
            {
                player.grounded = true;
                player.jumped = false;
            }
            else
                player.grounded = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //check for player leaving the ground
        player.grounded = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private float player_height;

    private void Start()
    {
        //get player height
        player_height = PlayerController.instance.sprite.bounds.size.y;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            if (PlayerController.instance.transform.position.y - (player_height / 2) > collision.gameObject.transform.position.y +
                    (collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2))
                PlayerController.instance.grounded = true;
            else
                PlayerController.instance.grounded = false;
        }
        else if(collision.gameObject.CompareTag("pizza"))
        {
            if (PlayerController.instance.transform.position.y - (player_height / 2) > collision.gameObject.transform.position.y +
                    (PizzaController.instance.sprite.bounds.size.y / 2))
                PlayerController.instance.grounded = true;
            else
                PlayerController.instance.grounded = false;
        }
    }
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController.instance.grounded = false;
    }
    */
}

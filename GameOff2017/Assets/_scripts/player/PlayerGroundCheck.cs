using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private float player_height;

    private void Start()
    {
        //get player height
        player_height = PlayerController.instance.GetComponent<BoxCollider2D>().bounds.size.y;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(player_height);
        Debug.Log(PlayerController.instance.transform.position.y + 0.35f);
        Debug.Log(collision.gameObject.transform.position.y +
                    (collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2));
        if (collision.gameObject.CompareTag("ground"))
        {
            if (PlayerController.instance.transform.position.y + 0.35f > collision.gameObject.transform.position.y +
                    (collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2))
                PlayerController.instance.grounded = true;
            else
                PlayerController.instance.grounded = false;

        }
        else if(collision.gameObject.CompareTag("pizza"))
        {
            if (PlayerController.instance.transform.position.y + 0.35f > collision.gameObject.transform.position.y +
                    (PizzaController.instance.GetComponent<BoxCollider2D>().bounds.size.y / 2))
                PlayerController.instance.grounded = true;
            else
                PlayerController.instance.grounded = false;
        }
        else if(collision.gameObject.CompareTag("end_level") && !PlayerController.instance.beat_level)
        {
            if (PlayerController.instance.transform.position.y + 0.35f > collision.gameObject.transform.position.y +
                    (collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2))
                PlayerController.instance.StartCoroutine(PlayerController.instance.LevelComplete());
        }
    }
}

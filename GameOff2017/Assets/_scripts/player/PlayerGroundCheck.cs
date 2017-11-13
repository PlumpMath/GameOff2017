using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("pizza"))
        {
            PlayerController.instance.grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController.instance.grounded = false;
    }
}

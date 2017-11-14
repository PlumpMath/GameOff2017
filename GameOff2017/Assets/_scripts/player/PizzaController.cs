using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaController : MonoBehaviour
{
    //player reference
    private GameObject player;

    //pizza attributes
    public float throw_speed;
    public float return_speed;
    public float throw_distance;
    private int throw_direction;
    private Vector2 target_position;
    private bool move_to_target = true;
    private bool wait = false;
    public float wait_time;
    private bool return_to_player = false;
    public BoxCollider2D platform_collider;

    private void Start()
    {
        //get player
        player = GameObject.FindGameObjectWithTag("Player");

        //disable collider
        platform_collider.enabled = false;

        //get target
        if (PlayerController.instance.current_direction == PlayerController.direction.LEFT)
            throw_direction = -1;
        else
            throw_direction = 1;
        target_position = new Vector2(gameObject.transform.position.x + throw_distance * throw_direction, gameObject.transform.position.y);
    }

    private void Update()
    {
        if (move_to_target && !wait && !return_to_player)
            MoveToTarget();
        else if(!move_to_target && !wait && return_to_player)
            MoveToPlayer();
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.Lerp(transform.position, target_position, throw_speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target_position) < 0.1f)
        {
            move_to_target = false;
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        wait = true;
        float wait_timer = wait_time;

        platform_collider.enabled = true;

        while (wait_timer > 0f)
        {
            wait_timer -= Time.deltaTime;
            yield return null;
        }

        wait = false;
        return_to_player = true;
        platform_collider.enabled = false;
    }

    private void MoveToPlayer()
    {
        //get player position
        Vector2 player_position = player.transform.position;

        //move to player
        transform.position = Vector2.MoveTowards(transform.position, player_position, return_speed * Time.deltaTime);
    }

    private void KillEnemy(GameObject enemy)
    {
        Debug.Log("kill");
        move_to_target = false;
        wait = false;
        return_to_player = true;
        platform_collider.enabled = false;
        enemy.GetComponent<Enemy>().Death();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
            KillEnemy(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !move_to_target && !wait)
        {
            PlayerController.instance.can_attack = true;
            Destroy(gameObject);
        }
    }
}

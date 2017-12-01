using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHouseController : MonoBehaviour {

    public bool dog;
    public GameObject dog_prefab;
    public DogController.direction direction;
    public Sprite closed;

	// Use this for initialization
	void Start () {
        dog = true;
        if (Random.Range(0, 1) == 0)
            direction = DogController.direction.LEFT;
        else
            direction = DogController.direction.RIGHT;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && dog)
        {
            GameObject temp = Instantiate(dog_prefab, transform.position, Quaternion.identity, transform);
            temp.GetComponent<DogController>().current_direction = direction;
            GetComponent<SpriteRenderer>().sprite = closed;
            dog = false;
        }
    }
}

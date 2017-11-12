using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreCoroutine : MonoBehaviour {

    public List<GameObject> scores;

	void Start () {
        StartCoroutine(Highscores());
	}
	void Update () {
		
	}

    IEnumerator Highscores()
    {
        for (int i = 0; i < 5; i++){
            yield return new WaitForSeconds(1);
            scores[i].gameObject.SetActive(true);
        }
    }
}

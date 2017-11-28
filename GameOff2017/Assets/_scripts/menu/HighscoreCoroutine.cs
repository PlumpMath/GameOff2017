using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreCoroutine : MonoBehaviour {

    public List<GameObject> scores;

    private List<string> places;

    private void Awake()
    {
        ScoreManager.instance.DownloadHighscores();
    }

    void Start ()
    {
        StartCoroutine(Highscores());
        places = new List<string>() { "1st", "2nd", "3rd", "4th", "5th" };
	}
	void Update () {
		
	}

    IEnumerator Highscores()
    {
        for (int i = 0; i < 5; i++){
            yield return new WaitForSeconds(1);
            ScoreManager.instance.highscore_text.text = ScoreManager.instance.highscoresList[0].score.ToString("000000");
            Regex reg = new Regex("([A-Z])+");
            scores[i].gameObject.GetComponent<Text>().text = places[i] + "     " + ScoreManager.instance.highscoresList[i].score.ToString("000000          ") + reg.Match(ScoreManager.instance.highscoresList[i].username);
            scores[i].gameObject.SetActive(true);
        }
    }
}

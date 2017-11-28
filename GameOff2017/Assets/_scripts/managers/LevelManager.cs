using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    //singleton
    public static LevelManager instance;

    //prefabs
    [Header("LEVEL PREFABS")]
    public GameObject player;
    public GameObject platform;

    //level properties
    [Header("LEVEL PROPERTIES")]
    public GameObject level_root;
    public Transform[] first_floor;
    public Transform[] second_floor;
    public Transform[] third_floor;
    public Transform[] fourth_floor;
    public Transform[] fifth_floor;
    [HideInInspector]
    //current level
    public Object current_level;
    //level object
    public GameObject level;
    //end of level
    private bool end_set = false;

    //player properties
    public GameObject current_player;
    public int lives;

    private void Awake()
    {
        //singleton setup
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        //set lives
        lives = 3;

        //update ui
        UIManager.instance.UpdateLives();

        //setup first level
        SetupLevel();

    }

    private void Update()
    {
        if (level == null && lives > 0)
            SetupLevel();
    }

    private void SpawnLevel()
    {
        //spawn a random level
        if (current_level == null)
        {
            level = new GameObject("platforms");
            level.transform.SetParent(level_root.transform);

            foreach(Transform t in first_floor)
            {
                float rand = Random.Range(0f, 1f);
                if(rand < 0.5f)
                    Instantiate(platform, t.position, Quaternion.identity, level.transform);
            }
            foreach (Transform t in second_floor)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.5f)
                    Instantiate(platform, t.position, Quaternion.identity, level.transform);
            }
            foreach (Transform t in third_floor)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.5f)
                    Instantiate(platform, t.position, Quaternion.identity, level.transform);
            }
            foreach (Transform t in fourth_floor)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.5f)
                    Instantiate(platform, t.position, Quaternion.identity, level.transform);
            }
            foreach (Transform t in fifth_floor)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.5f)
                {
                    GameObject plat = Instantiate(platform, t.position, Quaternion.identity, level.transform);
                    if(!end_set)
                    {
                        plat.tag = "end_level";
                        end_set = true;
                    }
                }
            }
        }
        //level = Instantiate(current_level, level_root.transform) as GameObject;
    }

    public void BreakdownLevel()
    {
 
        //update ui
        UIManager.instance.UpdateLives();

        //destroy level and player
        if (level != null && lives != 0)
        {
            Destroy(level);
            Destroy(current_player);
            end_set = false;

            //add score
            ScoreManager.instance.current_score += 100;
        }
        else
            StartCoroutine(GameOver());
    }

    public IEnumerator GameOver()
    {
        yield return StartCoroutine(TypeText());
        Destroy(level);
        Destroy(current_player);

        ScoreManager.instance.submit.SetActive(true);
        string[] letters = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        int current_letter = 0;
        int current_letter_letter = 0;
        bool score_submitted = false;
        Player input_manager = ReInput.players.GetPlayer(0);

        while (score_submitted != true)
        {
            if (input_manager.GetAxis ("UIHorizontal") > 0)
            {
                current_letter_letter--;
                if (current_letter_letter < 0)
                    current_letter_letter = 25;
                ScoreManager.instance.submit.transform.Find("Letters").transform.GetChild(current_letter).transform.GetComponent<Text>().text = letters[current_letter_letter];
            }
            else if (input_manager.GetAxis("UIHorizontal") < 0)
            {
                current_letter_letter++;
                if (current_letter_letter > 25)
                    current_letter_letter = 0;
                ScoreManager.instance.submit.transform.Find("Letters").transform.GetChild(current_letter).transform.GetComponent<Text>().text = letters[current_letter_letter];
            }

            yield return null;
        }
        ScoreManager.instance.submit.SetActive(false);
        string name = ScoreManager.instance.submit.transform.Find("Letters").transform.GetChild(0).transform.GetComponent<Text>().text + ScoreManager.instance.submit.transform.Find("Letters").transform.GetChild(1).transform.GetComponent<Text>().text + ScoreManager.instance.submit.transform.Find("Letters").transform.GetChild(2).transform.GetComponent<Text>().text;
        name = name + Random.Range(0000, 9999).ToString("0000");
        ScoreManager.instance.AddNewHighscore(name, ScoreManager.instance.current_score);
        SceneManager.LoadScene(0);
    }

    IEnumerator TypeText()
    {
        string message = "GAME OVER !";
        Text text = GameObject.Find("gameover").GetComponent<Text>();
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            yield return 0;
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(2);
        text.text = "";
    }

    private void SpawnPlayer()
    {
        //get spawn point from current level prefab
        Transform player_spawn = GameObject.FindGameObjectWithTag("player_spawn").transform;

        //spawn player
        current_player = Instantiate(player, player_spawn.position, Quaternion.identity, player_spawn);
    }

    private void SpawnEnemies()
    {
        //get spawn points from current level prefab
        GameObject[] enemy_spawn_points = GameObject.FindGameObjectsWithTag("enemy_spawn");

        //spawn enemies
        for (int i = 0; i < enemy_spawn_points.Length; i++)
        {
            Instantiate(enemy_spawn_points[i].GetComponent<SpawnPrefab>().prefab, enemy_spawn_points[i].transform.position, Quaternion.identity, enemy_spawn_points[i].transform);
        }
    }

    public void SetupLevel()
    {
        //spawn level
        SpawnLevel();

        //spawn player
        SpawnPlayer();

        //spawn enemies
        SpawnEnemies();

    }
}

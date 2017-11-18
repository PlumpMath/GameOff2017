using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    //singleton
    public static LevelManager instance;

    //prefabs
    [Header("PLAYER PREFAB")]
    public GameObject player;

    //level properties
    [Header("LEVEL PROPERTIES")]
    public GameObject level_root;
    [HideInInspector]
    //current level
    public Object current_level;
    //level object
    public GameObject level;

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
            current_level = EnvironmentManager.instance.levels[Random.Range(0, EnvironmentManager.instance.levels.Length)];
        level = Instantiate(current_level, level_root.transform) as GameObject;
    }

    public void BreakdownLevel()
    {
 
        //update ui
        UIManager.instance.UpdateLives();

        //destroy level and player
        if (level != null && lives != 0)
            Destroy(level);
        else
            StartCoroutine(GameOver());
    }

    public IEnumerator GameOver()
    {
        yield return StartCoroutine(TypeText());
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

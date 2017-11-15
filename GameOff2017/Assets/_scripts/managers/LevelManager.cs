using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    //singleton
    public static LevelManager instance;

    //prefabs
    [Header("PLAYER PREFAB")]
    public GameObject player;
    [Header("ENEMY PREFAB")]
    public GameObject bully;

    //level properties
    [Header("LEVEL PROPERTIES")]
    public GameObject level_root;
    [HideInInspector]
    //current level
    public Object current_level;
    //level object
    public GameObject level;

    public GameObject current_player;

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
        //setup first level
        SetupLevel();
    }

    private void Update()
    {
        if (level == null)
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
        //destroy level and player
        if (level != null)
            Destroy(level);
    }

    private void SpawnPlayer()
    {
        //get spawn point from current level prefab
        Transform player_spawn = GameObject.FindGameObjectWithTag("player_spawn").transform;

        //spawn player
        Debug.Log(current_player);
        current_player = Instantiate(player, player_spawn.position, Quaternion.identity, player_spawn);
    }

    private void SpawnEnemies()
    {
        //get spawn points from current level prefab
        GameObject[] enemy_spawn_points = GameObject.FindGameObjectsWithTag("enemy_spawn");


        //spawn enemies
        for (int i = 0; i < enemy_spawn_points.Length; i++)
        {
            Instantiate(bully, enemy_spawn_points[i].transform.position, Quaternion.identity, enemy_spawn_points[i].transform);
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

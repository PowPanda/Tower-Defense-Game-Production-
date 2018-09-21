using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour {

    public GameObject[] objects;                // The prefab to be spawned.
    public float spawnTime;                // How long between each spawn.
    private Vector3 spawnPosition;
    public GameObject enemy1, enemy2;           // Enemy Prefabs
    private int enemyCount;
    public int maxEnemyCount;
    public int remainingEnemy;
    public Text text_remainingEnemy;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    // Use this for initialization
    void Start()
    {
        objects = new GameObject[2] { enemy1, enemy2 };
        enemyCount = 0;
        maxEnemyCount = 10;
        spawnTime = 5f;

        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        remainingEnemy = maxEnemyCount;
    }

    void Update()
    {
        DisplayAmountRemainingEnemy();
    }

    void Spawn()
    {
        spawnPosition.x = Random.Range(spawnPoint1.position.x, spawnPoint2.position.x);
        spawnPosition.y = spawnPoint1.position.y + 3f;
        spawnPosition.z = spawnPoint1.position.z;

        Instantiate(objects[Random.Range(0, objects.Length)], spawnPosition, Quaternion.identity);

        enemyCount++;

        if (enemyCount >= maxEnemyCount)
            CancelInvoke("Spawn");
    }

    void ResetEnemyCount()
    {
        enemyCount = 0;
    }

    void DisplayAmountRemainingEnemy()
    {
        text_remainingEnemy.text = "x " + remainingEnemy.ToString();
    }
}

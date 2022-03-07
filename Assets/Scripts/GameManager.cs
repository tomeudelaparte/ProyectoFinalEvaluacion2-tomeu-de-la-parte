using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameObject player;

    public GameObject enemyPrefab;
    public GameObject healthPrefab;
    public GameObject shieldPrefab;

    private GameObject[] enemySpawnPositionsNorth;
    private GameObject[] enemySpawnPositionsSouth;
    private GameObject[] healthSpawnPositions;
    private GameObject[] shieldSpawnPositions;

    private GameObject[] healthItemsAvailable;
    private GameObject[] shieldItemsAvailable;


    private int waveIndex = 0;
    private int[] enemiesPerWave = { 5, 10, 14 };
    private int enemiesLeft;

    public GameObject[] levelPanel;

    private bool isGameOver;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isGameOver = false;

        player = GameObject.FindGameObjectWithTag("Player");

        enemySpawnPositionsNorth = GameObject.FindGameObjectsWithTag("SpawnPointN");
        enemySpawnPositionsSouth = GameObject.FindGameObjectsWithTag("SpawnPointS");

        healthSpawnPositions = GameObject.FindGameObjectsWithTag("HealthSpawnPoint");
        shieldSpawnPositions = GameObject.FindGameObjectsWithTag("ShieldSpawnPoint");

        SpawnEnemyWave(enemiesPerWave[waveIndex]);

        SpawnHealthShield();
    }

    private void Update()
    {
        if(!isGameOver)
        {
            enemiesLeft = FindObjectsOfType<EnemyControllerAI>().Length;

            if (enemiesLeft <= 0)
            {
                waveIndex++;

                if (waveIndex <= enemiesPerWave.Length)
                {
                    foreach (GameObject text in levelPanel)
                    {
                        text.GetComponent<TextMeshPro>().text = "0" + (waveIndex + 1);
                    }

                    SpawnHealthShield();

                    SpawnEnemyWave(enemiesPerWave[waveIndex]);
                }
                else
                {
                    GameOver();
                }
            }
        }
    }

    private void SpawnEnemyWave(int totalEnemies)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            if (player.transform.position.z < 0)
            {
                Instantiate(enemyPrefab, enemySpawnPositionsNorth[i].transform.position, transform.rotation);
            }
            else
            {
                Instantiate(enemyPrefab, enemySpawnPositionsSouth[i].transform.position, transform.rotation);
            }
        }
    }

    private void SpawnHealthShield()
    {
        healthItemsAvailable = GameObject.FindGameObjectsWithTag("HealthItem");
        shieldItemsAvailable = GameObject.FindGameObjectsWithTag("ShieldItem");

        if(healthItemsAvailable.Length > 0)
        {
            foreach (GameObject item in healthItemsAvailable)
            {
                Destroy(item);
            }
        }

        if (shieldItemsAvailable.Length > 0)
        {
            foreach (GameObject item in shieldItemsAvailable)
            {
                Destroy(item);
            }
        }

        foreach (GameObject spawnPoint in healthSpawnPositions)
        {
            Instantiate(healthPrefab, spawnPoint.transform.position, healthPrefab.transform.rotation);
        }

        foreach (GameObject spawnPoint in shieldSpawnPositions)
        {
            Instantiate(shieldPrefab, spawnPoint.transform.position, shieldPrefab.transform.rotation);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        isGameOver = true;
    }
}
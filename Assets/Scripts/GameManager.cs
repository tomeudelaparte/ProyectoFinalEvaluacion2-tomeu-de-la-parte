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

    public GameObject playerInterface;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    private int waveIndex = 0;
    private int[] enemiesPerWave = {5};
    private int enemiesLeft;

    public GameObject[] levelPanel;

    private bool isGameOver;
    private float gameTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemySpawnPositionsNorth = GameObject.FindGameObjectsWithTag("SpawnPointN");
        enemySpawnPositionsSouth = GameObject.FindGameObjectsWithTag("SpawnPointS");

        healthSpawnPositions = GameObject.FindGameObjectsWithTag("HealthSpawnPoint");
        shieldSpawnPositions = GameObject.FindGameObjectsWithTag("ShieldSpawnPoint");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerInterface.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

        isGameOver = false;

        SpawnEnemyWave(enemiesPerWave[waveIndex]);

        SpawnHealthShield();
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if(!isGameOver)
        {
            enemiesLeft = FindObjectsOfType<EnemyControllerAI>().Length;

            if (enemiesLeft <= 0)
            {
                waveIndex++;

                if (waveIndex < enemiesPerWave.Length)
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
                   MissionComplete();
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

    private void MissionComplete()
    {
        Time.timeScale = 0;

        Debug.Log("MISSION COMPLETE: " + gameTime.ToString("F1"));
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
    }
}
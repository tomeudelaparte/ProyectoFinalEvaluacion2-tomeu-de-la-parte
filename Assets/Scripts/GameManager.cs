using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public AudioSource gameManagerAudioSource;

    public GameObject mainCamera;
    public GameObject playerInterface;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject objectiveCompletePanel;
    public GameObject gameOverPanel;

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
    private int[] enemiesPerWave = {5, 9, 14};
    private int enemiesLeft;

    public TextMeshPro[] levelNumberPanel;
    public TextMeshProUGUI gameTimeText;

    public bool isGameOver, isObjectiveComplete;
    private float gameTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManagerAudioSource = FindObjectOfType<AudioSource>();

        enemySpawnPositionsNorth = GameObject.FindGameObjectsWithTag("SpawnPointN");
        enemySpawnPositionsSouth = GameObject.FindGameObjectsWithTag("SpawnPointS");

        healthSpawnPositions = GameObject.FindGameObjectsWithTag("HealthSpawnPoint");
        shieldSpawnPositions = GameObject.FindGameObjectsWithTag("ShieldSpawnPoint");

        playerInterface.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        objectiveCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        isGameOver = false;
        isObjectiveComplete = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Time.timeScale = 1;

        SpawnEnemyWave(enemiesPerWave[waveIndex]);

        SpawnHealthShield();
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (!isGameOver)
        {
            enemiesLeft = FindObjectsOfType<EnemyControllerAI>().Length;

            if (enemiesLeft <= 0)
            {
                waveIndex++;

                if (waveIndex < enemiesPerWave.Length)
                {
                    foreach (TextMeshPro level in levelNumberPanel)
                    {
                        level.text = "0" + (waveIndex + 1);
                    }

                    SpawnHealthShield();

                    SpawnEnemyWave(enemiesPerWave[waveIndex]);
                }
                else
                {
                    if(!isObjectiveComplete)
                    {
                        isObjectiveComplete = true;

                        ObjectiveComplete();
                    }
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

        if (healthItemsAvailable.Length > 0)
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

    private void ObjectiveComplete()
    {
        isGameOver = true;
        Time.timeScale = 0;

        objectiveCompletePanel.SetActive(true);
        playerInterface.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameManagerAudioSource.Stop();

        gameTimeText.text = gameTime.ToString("F1") + "s";
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        gameOverPanel.SetActive(true);
        playerInterface.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameManagerAudioSource.Stop();
    }

    public void ExitToMain()
    {
        SceneManager.LoadScene(0);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

}
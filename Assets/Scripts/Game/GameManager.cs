using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Variables para referenciar
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

    public TextMeshPro[] levelNumberPanel;
    public TextMeshProUGUI currentLevelUI;
    public TextMeshProUGUI enemiesLeftUI;
    public TextMeshProUGUI gameTimeText;

    // Index de la oleada
    private int waveIndex = 0;

    // Enemigos por oleada (LEVEL 01 = 5, LEVEL 02 = 9, LEVEL 03 = 14)
    private int[] enemiesPerWave = { 5, 9, 14 };

    // Variables booleanas
    public bool isGameOver, isObjectiveComplete;

    // Posiciones spawn de enemigos
    private GameObject[] enemySpawnPositionsNorth;
    private GameObject[] enemySpawnPositionsSouth;

    // Posiciones spawn de items
    private GameObject[] healthSpawnPositions;
    private GameObject[] shieldSpawnPositions;

    private GameObject[] healthItemsAvailable;
    private GameObject[] shieldItemsAvailable;

    // Enemigos restantes
    private int enemiesLeft;

    // Tiempo de juego
    private float gameTime;

    void Start()
    {
        // Obtiene las referencias que necesita
        player = GameObject.FindGameObjectWithTag("Player");
        gameManagerAudioSource = GetComponent<AudioSource>();

        // Obtiene las posiciones spawn de los enemigos
        enemySpawnPositionsNorth = GameObject.FindGameObjectsWithTag("SpawnPointN");
        enemySpawnPositionsSouth = GameObject.FindGameObjectsWithTag("SpawnPointS");

        // Obtiene las posiciones spawn de los items
        healthSpawnPositions = GameObject.FindGameObjectsWithTag("HealthSpawnPoint");
        shieldSpawnPositions = GameObject.FindGameObjectsWithTag("ShieldSpawnPoint");

        // Activa y desactiva los paneles
        playerInterface.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        objectiveCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Variables booleanas
        isGameOver = false;
        isObjectiveComplete = false;

        // Oculta y bloquea el ratón
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // Reanuda el tiempo
        Time.timeScale = 1;

        // Activa la música
        gameManagerAudioSource.Play();

        // Empieza la oleada
        StartWave();
    }

    private void Update()
    {
        // Suma el tiempo de juego
        gameTime += Time.deltaTime;

        // Si isGameOver es false
        if (!isGameOver)
        {
            // Obtiene los enemigos en escena
            enemiesLeft = FindObjectsOfType<EnemyControllerAI>().Length;

            // Actualiza el número de enemigos restantes en la UI
            enemiesLeftUI.text = enemiesLeft.ToString();

            // Si enemiesLeft es menor o igual a 0
            if (enemiesLeft <= 0)
            {
                // Siguiente oleada +1
                waveIndex++;

                // Si waveIndex es menor al tamaño de enemiesPerWave
                if (waveIndex < enemiesPerWave.Length)
                {
                    // Empieza la oleada
                    StartWave();
                }
                else
                {
                    // Si isObjectiveComplete es False
                    if (!isObjectiveComplete)
                    {
                        // isObjectiveComplete es True
                        isObjectiveComplete = true;

                        // Objetivo Completado
                        ObjectiveComplete();
                    }
                }
            }
        }
    }

    // Empieza la oleada
    private void StartWave()
    {
        // Actualiza el level de la interfaz de usuario
        currentLevelUI.text = (waveIndex + 1).ToString("D2");

        // Actualiza el level del panel giratorio
        foreach (TextMeshPro level in levelNumberPanel)
        {
            level.text = (waveIndex + 1).ToString("D2");
        }

        // Spawnea la oleada
        SpawnHealthShieldItems();

        // Spawnea los items
        SpawnEnemyWave(enemiesPerWave[waveIndex]);
    }

    // Spawnea los enemigos segun la posicion del Player
    private void SpawnEnemyWave(int totalEnemies)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            // Si la posicion de player en Z es menor a 0
            if (player.transform.position.z < 0)
            {
                // Spawnea los enemigos en la posiciones en la zona norte
                Instantiate(enemyPrefab, enemySpawnPositionsNorth[i].transform.position, transform.rotation);
            }
            else
            {
                // Spawnea los enemigos en la posiciones en la zona sur
                Instantiate(enemyPrefab, enemySpawnPositionsSouth[i].transform.position, transform.rotation);
            }
        }
    }

    // Spawnea los items de Health y Shield en la posiciones correspondientes
    private void SpawnHealthShieldItems()
    {
        // Obtiene los items en escena
        healthItemsAvailable = GameObject.FindGameObjectsWithTag("HealthItem");
        shieldItemsAvailable = GameObject.FindGameObjectsWithTag("ShieldItem");

        // Si healthItemsAvailable.Length es mayor a 0
        if (healthItemsAvailable.Length > 0)
        {
            foreach (GameObject item in healthItemsAvailable)
            {
                // Destruye el item
                Destroy(item);
            }
        }

        // Si shieldItemsAvailable.Length es mayor a 0
        if (shieldItemsAvailable.Length > 0)
        {
            foreach (GameObject item in shieldItemsAvailable)
            {
                // Destruye el item
                Destroy(item);
            }
        }

        // Spawnea healthPrefab en las posiciones de spawn
        foreach (GameObject spawnPoint in healthSpawnPositions)
        {
            Instantiate(healthPrefab, spawnPoint.transform.position, healthPrefab.transform.rotation);
        }

        // Spawnea shieldPrefab en las posiciones de spawn
        foreach (GameObject spawnPoint in shieldSpawnPositions)
        {
            Instantiate(shieldPrefab, spawnPoint.transform.position, shieldPrefab.transform.rotation);
        }
    }

    // Objetivo Completado
    private void ObjectiveComplete()
    {
        // isGameOver es True
        isGameOver = true;

        // Pausa el tiempo
        Time.timeScale = 0;

        // Activa el panel de objetivo completado y oculta la interfaz de usuario
        objectiveCompletePanel.SetActive(true);
        playerInterface.SetActive(false);

        // Desbloquea y muestra el ratón
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Para la música
        gameManagerAudioSource.Stop();

        // Actualiza el texto con el tiempo de juego 
        gameTimeText.text = gameTime.ToString("F1") + "s";
    }

    // Game Over
    public void GameOver()
    {
        // isGameOver es True
        isGameOver = true;

        // Pausa el tiempo
        Time.timeScale = 0;

        // Activa el panel de GameOver y oculta la interfaz de usuario
        gameOverPanel.SetActive(true);
        playerInterface.SetActive(false);

        // Desbloquea y muestra el ratón
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Para la música
        gameManagerAudioSource.Stop();
    }

    // Carga la escena del menu principal
    public void ExitToMain()
    {
        SceneManager.LoadScene(0);
    }

    // Carga la misma escena
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
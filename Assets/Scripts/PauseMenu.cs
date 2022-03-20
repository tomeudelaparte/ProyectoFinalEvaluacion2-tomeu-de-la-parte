using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject playerInterface;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    private bool isActivePause = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.isGameOver)
        {
            ShowPauseMenu();
        }
    }

    public void ShowPauseMenu()
    {
        if (!isActivePause)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            playerInterface.SetActive(false);
            pauseMenu.SetActive(true);

            gameManager.gameManagerAudioSource.Pause();

            isActivePause = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            playerInterface.SetActive(true);
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);

            gameManager.gameManagerAudioSource.Play();

            isActivePause = false;
        }
    }
}

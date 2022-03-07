using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    private bool isActivePause = false;
    private bool isActiveOptions = false;

    void Start()
    {
        Screen.lockCursor = true;
    }

    private void Update()
    {
        checkPauseMenu();
    }

    private void backButton()
    {
        optionsMenu.SetActive(false);
    }

    private void exitToMain()
    {
        SceneManager.LoadScene(0);
    }

    private void openOptions()
    {

        if (!isActiveOptions)
        {
            optionsMenu.SetActive(true);
            isActiveOptions = true;
        }
        else
        {
            optionsMenu.SetActive(false);
            isActiveOptions = false;
        }

    }

    private void checkPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActivePause)
            {
                Time.timeScale = 0;
                Screen.lockCursor = false;
                pauseMenu.SetActive(true);

                isActivePause = true;

            }
            else
            {

                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                Screen.lockCursor = true;

                isActivePause = false;
            }
        }
    }
}

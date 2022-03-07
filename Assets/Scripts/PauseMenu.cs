using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    private bool isActivePause = false;
    private bool isActiveOptions = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            checkPauseMenu();
        }
    }

    public void backButton()
    {
        openOptions();
    }

    public void exitToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void openOptions()
    {

        if (!isActiveOptions)
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(true);

            isActivePause = false;
            isActiveOptions = true;
        }
        else
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);

            isActivePause = true;
            isActiveOptions = false;
        }
    }

    public void checkPauseMenu()
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

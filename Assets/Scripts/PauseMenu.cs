using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject playerInterface;

    public GameObject pauseMenu;
    public GameObject optionsMenu;

    private bool isActivePause = false;
    private bool isActiveOptions = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu();
        }
    }

    public void ExitToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowPauseMenu()
    {

        if (!isActivePause)
        {
            Time.timeScale = 0;
            Cursor.visible = true;

            playerInterface.SetActive(false);
            pauseMenu.SetActive(true);

            isActivePause = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;

            playerInterface.SetActive(true);
            pauseMenu.SetActive(false);

            isActivePause = false;
        }
    }

    public void ShowOptions()
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
}

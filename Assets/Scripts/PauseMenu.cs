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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu();
        }
    }

    public void ShowPauseMenu()
    {
        if (!isActivePause)
        {
            Time.timeScale = 0;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;

            playerInterface.SetActive(false);
            pauseMenu.SetActive(true);

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


            isActivePause = false;
        }
    }
    public void ExitToMain()
    {
        SceneManager.LoadScene(0);
    }
}

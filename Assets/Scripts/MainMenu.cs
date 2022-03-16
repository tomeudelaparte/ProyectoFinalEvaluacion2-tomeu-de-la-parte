using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject instructionsPanel;

    private bool optionsIsActive = false;
    private bool instructionsIsActive = false;

    private void Start()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        instructionsPanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenCloseOptions()
    {
        if (!optionsIsActive)
        {
            optionsIsActive = true;
            optionsPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
        else
        {
            optionsIsActive = false;
            optionsPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
    }

    public void OpenCloseInstructions()
    {
        if (!instructionsIsActive)
        {
            instructionsIsActive = true;
            instructionsPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
        else
        {
            instructionsIsActive = false;
            instructionsPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

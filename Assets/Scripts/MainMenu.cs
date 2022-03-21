using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject instructionsPanel;

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

    public void ExitGame()
    {
        #if UNITY_EDITOR

                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}

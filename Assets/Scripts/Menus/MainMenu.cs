using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Variables para referenciar los menus
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject instructionsPanel;

    private void Start()
    {
        // Activa el menu principal y desactiva las opciones e instrucciones
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        instructionsPanel.SetActive(false);

        // Reanuda el tiempo
        Time.timeScale = 1;
    }

    // Carga la escena del juego
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Sale del juego en Build y del editor en Unity
    public void ExitGame()
    {
        #if UNITY_EDITOR

                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}

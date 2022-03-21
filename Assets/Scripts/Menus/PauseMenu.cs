using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   // Referencia a GameManager
    private GameManager gameManager;

    // Variales para referenciar los paneles
    public GameObject playerInterface;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    // Variable booleana
    private bool isActivePause = false;

    void Start()
    {
        // Obtiene el GameManager
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Si pulsas ESC y GameOver es False
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.isGameOver)
        {
            // Muestra el menu de pausa
            ShowPauseMenu();
        }
    }

    // Muestra el menu de pausa
    public void ShowPauseMenu()
    {
        // Si pausa no est� activa
        if (!isActivePause)
        {
            // Pausa el tiempo
            Time.timeScale = 0;

            // Desbloquea y muestra el rat�n
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Oculta la interfaz de usuario
            playerInterface.SetActive(false);

            // Muestra el menu de pausa
            pauseMenu.SetActive(true);

            // Pausa la musica
            gameManager.gameManagerAudioSource.Pause();

            // Pausa es True
            isActivePause = true;
        }
        else
        {
            // Reanuda el tiempo
            Time.timeScale = 1;

            // Bloquea y oculta el rat�n
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            // Muestra la interfaz de usuario
            playerInterface.SetActive(true);

            // Oculta el menu de opciones y pausa
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);

            // Reanuda la m�sica
            gameManager.gameManagerAudioSource.Play();

            // Pausa es False
            isActivePause = false;
        }
    }
}

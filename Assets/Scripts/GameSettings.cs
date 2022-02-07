using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        Screen.lockCursor = true;

        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Screen.lockCursor = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }
    }
}

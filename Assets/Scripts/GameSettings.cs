using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        Screen.lockCursor = true;

        Application.targetFrameRate = 60;
    }
}

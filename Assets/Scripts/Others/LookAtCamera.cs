using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Referencia a Main Camera
    private GameObject cameraPlayer;

    void Start()
    {
        // Obtiene el Main Camera
        cameraPlayer = GameObject.Find("Main Camera");
    }

    void Update()
    {
        // El GameObject rota en dirección a la cámara
        transform.LookAt(cameraPlayer.transform.position);

        // Limita la rotación en X y Z
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}

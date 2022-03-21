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
        // El GameObject rota en direcci�n a la c�mara
        transform.LookAt(cameraPlayer.transform.position);

        // Limita la rotaci�n en X y Z
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}

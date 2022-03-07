using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject cameraPlayer;

    void Start()
    {
        cameraPlayer = GameObject.Find("Main Camera");
    }

    void Update()
    {
        transform.LookAt(cameraPlayer.transform.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}

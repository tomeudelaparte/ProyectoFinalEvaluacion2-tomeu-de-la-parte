using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private GameObject camera; 
    
    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        transform.LookAt(camera.transform.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);


    }
}

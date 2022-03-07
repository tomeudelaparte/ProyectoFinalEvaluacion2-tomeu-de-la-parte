using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    public float velocity = 20;

    void Update()
    {
        transform.Rotate(Vector3.up * velocity * Time.deltaTime);
    }
}

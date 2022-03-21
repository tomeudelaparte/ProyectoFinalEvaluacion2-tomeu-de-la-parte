using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    // Velocidad de movimiento
    public float velocity = 20;

    void Update()
    {
        // Rota el GameObject en el eje Vertical
        transform.Rotate(Vector3.up * velocity * Time.deltaTime);
    }
}

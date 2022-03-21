using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpGameObject : MonoBehaviour
{
    // Velocidad de movimiento
    public float velocity = 20;

    void Update()
    {
        // Mueve el GameObject hacia arriba en relaci�n a las coordenandas de la escena
        transform.Translate(Vector3.up * velocity * Time.deltaTime, Space.World);
    }
}

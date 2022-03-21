using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Tiempo de destrucción
    public float lifeTime = 8f;

    void Start()
    {
        // Destruye el GameObect con un temporizador
        Destroy(gameObject, lifeTime);
    }
}

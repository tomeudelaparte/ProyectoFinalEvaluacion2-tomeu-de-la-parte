using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float range = 800f;

    void Update()
    {
        if (transform.position.x > range
            || transform.position.x < -range
            || transform.position.y > range
            || transform.position.z < -range
            || transform.position.z > range
            || transform.position.z < -range)
        {
            Destroy(gameObject);
        }
    }
}

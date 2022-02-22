using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.one * 20 * Time.deltaTime);
    }
}

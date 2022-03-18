using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpGameObject : MonoBehaviour
{
    public float velocity = 20;

    void Update()
    {
        transform.Translate(Vector3.up * velocity * Time.deltaTime, Space.World);
    }
}

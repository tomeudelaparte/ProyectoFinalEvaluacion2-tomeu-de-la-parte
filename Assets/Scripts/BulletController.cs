using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 300f;
    private float range = 800f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
            Destroy(gameObject);
        }
    }
}

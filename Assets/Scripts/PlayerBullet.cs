using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float speed = 300f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyControllerAI>().health -= 0.5f;

            if (other.gameObject.GetComponent<EnemyControllerAI>().health <= 0)
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }
    }
}

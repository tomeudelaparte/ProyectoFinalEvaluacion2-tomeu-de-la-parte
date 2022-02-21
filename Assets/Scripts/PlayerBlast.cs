using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlast : MonoBehaviour
{
    private float speed = 300f;
    public ParticleSystem blueBlastParticles, redBlastParticles;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(redBlastParticles, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            Instantiate(blueBlastParticles, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}

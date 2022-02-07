using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBullet : MonoBehaviour
{
    private float speed = 300f;
    public ParticleSystem impact;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyControllerAI>().health -= 0.5f;

            if (other.gameObject.GetComponent<EnemyControllerAI>().health <= 0)
            {
                Destroy(other.gameObject);
            }

            if (other.gameObject.GetComponent<EnemyControllerAI>().health <= 0.5f)
            {
                other.gameObject.GetComponent<EnemyControllerAI>().isOnEngine = false;

                other.gameObject.GetComponent<NavMeshAgent>().speed = 0;
            }

            spawnImpactParticles(other);

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            spawnImpactParticlesGround(other);

            Destroy(gameObject);
        }
    }

    private void spawnImpactParticles(Collider other)
    {
        impact = Instantiate(impact, transform.position, transform.rotation);

        impact.GetComponent<Renderer>().material.SetColor("_EmissionColor", other.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color);

        impact.Play();
    }

    private void spawnImpactParticlesGround(Collider other)
    {
        impact = Instantiate(impact, transform.position, transform.rotation);

        impact.GetComponent<Renderer>().material.SetColor("_EmissionColor", other.gameObject.transform.GetComponent<Renderer>().material.color);

        impact.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 200f;

    public ParticleSystem impact;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spawnImpactParticles(other);

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            spawnImpactParticlesGround(other);

            Destroy(gameObject);
        }

    }

    private void spawnImpactParticles(Collision other)
    {
        impact = Instantiate(impact, transform.position, transform.rotation);

        impact.GetComponent<Renderer>().material.SetColor("_EmissionColor", other.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color);

        impact.Play();
    }

    private void spawnImpactParticlesGround(Collision other)
    {
        impact = Instantiate(impact, transform.position, transform.rotation);

        impact.GetComponent<Renderer>().material.SetColor("_EmissionColor", other.gameObject.transform.GetComponent<Renderer>().material.color);

        impact.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlast : MonoBehaviour
{
    private float speed = 300f;

    public ParticleSystem impact;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
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
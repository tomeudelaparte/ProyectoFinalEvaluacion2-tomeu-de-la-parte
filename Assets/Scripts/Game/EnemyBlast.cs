using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlast : MonoBehaviour
{
    // Referencia a las partículas
    public ParticleSystem yellowBlastParticles, pinkBlastParticles;
    
    // Velocidad de movimiento
    private float speed = 300f;

    void Update()
    {
        // Mueve el GameObject hacia delante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si colisiona con Player
        if (other.gameObject.CompareTag("Player"))
        {
            // Instancia partículas rosas
            Instantiate(pinkBlastParticles, transform.position, transform.rotation);

            // Destruye el disparo
            Destroy(gameObject);
        }

        // Si colisiona con Ground
        if (other.gameObject.CompareTag("Ground"))
        {
            // Instancia partículas amarillas
            Instantiate(yellowBlastParticles, transform.position, transform.rotation);

            // Destruye el disparo
            Destroy(gameObject);
        }
    }
}
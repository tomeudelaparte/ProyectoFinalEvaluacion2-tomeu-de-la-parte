using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlast : MonoBehaviour
{
    // Referencia a las part�culas
    public ParticleSystem blueBlastParticles, redBlastParticles;

    // Velocidad de movimiento
    private float speed = 300f;

    void Update()
    {
        // Mueve el GameObject hacia delante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si colisiona con Enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Instancia part�culas rojas
            Instantiate(redBlastParticles, transform.position, transform.rotation);

            // Destruye el disparo
            Destroy(gameObject);
        }

        // Si colisiona con Ground
        if (other.gameObject.CompareTag("Ground"))
        {
            // Instancia part�culas azules
            Instantiate(blueBlastParticles, transform.position, transform.rotation);

            // Destruye el disparo
            Destroy(gameObject);
        }
    }
}

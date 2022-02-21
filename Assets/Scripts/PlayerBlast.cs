using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerBlast : MonoBehaviour
{
    public GameObject damageTextPrefab;

    private float speed = 300f;
    public ParticleSystem blueBlastParticles, redBlastParticles;

    private float damage = 0.25f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyControllerAI>().health -= damage;

            GameObject textDamage = Instantiate(damageTextPrefab, transform.position, damageTextPrefab.transform.rotation);

            textDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage.ToString();

            if (other.gameObject.GetComponent<EnemyControllerAI>().health <= 0.5f)
            {
                other.gameObject.GetComponent<EnemyControllerAI>().isOnEngine = false;

                other.gameObject.GetComponent<NavMeshAgent>().speed = 0;
            }
            
            if (other.gameObject.GetComponent<EnemyControllerAI>().health <= 0)
            {
                Destroy(other.gameObject);
            }

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

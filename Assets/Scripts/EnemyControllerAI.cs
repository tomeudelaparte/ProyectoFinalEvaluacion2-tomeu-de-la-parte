using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerAI : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent enemy;
    public GameObject blastPrefab;

    private Vector3 directionToPlayer, newPosition;

    private float playerDetectionDistance = 300f;

    public float health = 1f;

    private Animator canonAnimator;

    void Start()
    {
        player = GameObject.Find("Player");
        enemy = GetComponent<NavMeshAgent>();

        canonAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        InvokeRepeating("shoot", 2f, 2f);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < playerDetectionDistance && distance >= 100)
        {
            directionToPlayer = transform.position - player.transform.position;

            newPosition = transform.position - directionToPlayer;

            enemy.SetDestination(newPosition);

            if (enemy.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(enemy.velocity.normalized);
            }
        }

        transform.GetChild(0).transform.LookAt(player.transform.position);
    }

    private void shoot()
    {
        Instantiate(blastPrefab, transform.GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).rotation);
        canonAnimator.SetTrigger("Shoot");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
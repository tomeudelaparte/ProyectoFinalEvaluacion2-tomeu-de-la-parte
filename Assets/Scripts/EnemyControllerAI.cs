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

    private float playerDetectionDistance = 5000f;

    public float health = 1f;
    public bool isOnEngine = true;

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

        Vector3 lookPos = (player.transform.position - transform.position).normalized;

        lookPos.y += 5;

        transform.GetChild(0).LookAt(player.transform.position);
        transform.GetChild(0).GetChild(0).LookAt(lookPos);

        transform.GetChild(0).localEulerAngles = new Vector3(0, transform.GetChild(0).localEulerAngles.y, 0);
        transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(-transform.GetChild(0).GetChild(0).localEulerAngles.x, 0, 0);
    }

    private void shoot()
    {
        Instantiate(blastPrefab, transform.GetChild(0).GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).GetChild(0).rotation);
        canonAnimator.SetTrigger("Shoot");
    }
}
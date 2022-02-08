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

    private float playerDetectionDistance = 2000f;

    public float health = 1f;
    public bool isOnEngine = true;

    private float shootSpeed = 2f;
    private bool canShootWeapon = true;

    private Animator canonAnimator;

    void Start()
    {
        player = GameObject.Find("Player");
        enemy = GetComponent<NavMeshAgent>();

        canonAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
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

        if (canShootWeapon && IsPlayerOnSight())
        {
            weaponShoot();
        }

        transform.GetChild(0).LookAt(player.transform.position);
        transform.GetChild(0).localEulerAngles = new Vector3(0, transform.GetChild(0).localEulerAngles.y, 0);
    }

    private void weaponShoot()
    {
        canonAnimator.SetTrigger("Shoot");

        Instantiate(blastPrefab, transform.GetChild(0).GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).GetChild(0).rotation);

        StartCoroutine(weaponCooldown());
    }

    private IEnumerator weaponCooldown()
    {
        canShootWeapon = false;
        yield return new WaitForSeconds(shootSpeed);
        canShootWeapon = true;
    }

    private bool IsPlayerOnSight()
    {
        RaycastHit hitData;

        Ray ray = new Ray(transform.GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).forward);

        Debug.DrawRay(transform.GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).forward * playerDetectionDistance, Color.yellow);

        if (Physics.Raycast(ray, out hitData, playerDetectionDistance))
        {
            return hitData.collider.CompareTag("Player");
        }
        else
        {
            return false;
        }
    }
}
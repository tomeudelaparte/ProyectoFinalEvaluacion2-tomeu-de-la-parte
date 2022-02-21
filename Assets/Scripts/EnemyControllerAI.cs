using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyControllerAI : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent enemyAgent;
    public GameObject blastPrefab;
    private AudioSource audioSourceEnemy;

    public AudioClip shootSFX;

    private Vector3 directionToPlayer, newPosition;

    private float playerDetectionDistance = 2000f;

    public float health = 1f;

    private float shootSpeed = 2f;
    private bool canShootWeapon = true;

    private Animator canonAnimator;

    private bool isColliding = false;

    public GameObject damageTextPrefab;
    private float damage = 0.25f;

    void Start()
    {
        player = GameObject.Find("Player");
        audioSourceEnemy = GetComponent<AudioSource>();
        enemyAgent = GetComponent<NavMeshAgent>();

        canonAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < playerDetectionDistance && distance >= 100)
        {
            directionToPlayer = transform.position - player.transform.position;

            newPosition = transform.position - directionToPlayer;

            enemyAgent.SetDestination(newPosition);

            if (enemyAgent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(enemyAgent.velocity.normalized);
            }
        }

        if (canShootWeapon && IsPlayerOnSight())
        {
            audioSourceEnemy.PlayOneShot(shootSFX, 1f);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBlast"))
        {
            if (isColliding) return;
            isColliding = true;

            GameObject textDamage = Instantiate(damageTextPrefab, other.transform.position, damageTextPrefab.transform.rotation);
            textDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage.ToString();

            health -= 0.25f;

            if (health <= 0)
            {
                Destroy(gameObject);
            }

            StartCoroutine(TriggerEnterOn());
        }
    }

    private IEnumerator TriggerEnterOn()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }
}
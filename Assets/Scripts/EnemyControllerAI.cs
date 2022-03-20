using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyControllerAI : MonoBehaviour
{
    public GameObject canon;
    public GameObject turret;
    public GameObject blastPrefab;
    public GameObject healthBarUI;
    public GameObject damageTextPrefab;

    private GameObject player;
    private NavMeshAgent enemyAgent;
    private AudioSource audioSourceEnemy;
    private Animator canonAnimator;

    private float enemyHealth = 1f;

    private float[] damageHealthRange = { 0.20f, 0.36f };

    private float shootCooldown = 2f;
    private bool shootTrigger = true;

    private float playerDetectionDistance = 2000f;

    private bool isBlastColliding = false;

    private Vector3 directionToPlayer, newPosition;
    private float damage, distance;

    void Start()
    {
        player = GameObject.Find("Player");
        audioSourceEnemy = GetComponent<AudioSource>();
        enemyAgent = GetComponent<NavMeshAgent>();

        canonAnimator = canon.GetComponent<Animator>();

        healthBarUI.GetComponentInChildren<Slider>().value = enemyHealth;
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

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

        if (shootTrigger && IsPlayerOnSight())
        {
            audioSourceEnemy.Play();
            WeaponShoot();
        }

        turret.transform.LookAt(player.transform.position);
        turret.transform.localEulerAngles = new Vector3(0, turret.transform.localEulerAngles.y, 0);

        canon.transform.LookAt(player.transform.position);
        canon.transform.localEulerAngles = new Vector3(canon.transform.localEulerAngles.x, 0, 0);
    }

    private void WeaponShoot()
    {
        canonAnimator.SetTrigger("Shoot");

        Instantiate(blastPrefab, canon.transform.GetChild(0).position, canon.transform.GetChild(0).rotation);

        StartCoroutine(WeaponCooldown());
    }

    private IEnumerator WeaponCooldown()
    {
        shootTrigger = false;
        yield return new WaitForSeconds(shootCooldown);
        shootTrigger = true;
    }

    private bool IsPlayerOnSight()
    {
        RaycastHit hitData;

        Ray ray = new Ray(canon.transform.position, canon.transform.forward);

        Debug.DrawRay(canon.transform.position, canon.transform.forward * playerDetectionDistance, Color.yellow);

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
            if (isBlastColliding) return;
            isBlastColliding = true;

            damage = Mathf.Round(Random.Range(damageHealthRange[0], damageHealthRange[1]) * 100f) / 100f;

            GameObject textDamage = Instantiate(damageTextPrefab, other.transform.position, damageTextPrefab.transform.rotation);
            textDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage.ToString("F2");

            enemyHealth -= damage;
            healthBarUI.GetComponentInChildren<Slider>().value = enemyHealth;

            player.GetComponent<PlayerController>().audioSourcePlayer[1].Play();

            if (enemyHealth < 1)
            {
                healthBarUI.SetActive(true);
            }

            if (enemyHealth <= 0)
            {
                Destroy(gameObject);
            }

            StartCoroutine(TriggerEnterOn());
        }
    }

    private IEnumerator TriggerEnterOn()
    {
        yield return new WaitForEndOfFrame();
        isBlastColliding = false;
    }
}
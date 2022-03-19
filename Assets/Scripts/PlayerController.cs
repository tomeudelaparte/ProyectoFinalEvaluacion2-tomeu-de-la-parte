using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject canon;
    public GameObject turret;
    public GameObject blastPrefab;

    private Rigidbody rigidbodyPlayer;
    public AudioSource[] audioSourcePlayer;
    private Animator canonAnimator;

    private float horizontalInput, verticalInput, mouseInputX, mouseInputY;
    private float mouseSensitivity = 2;

    public GameObject healthBarUI, shieldBarUI;
    private float health = 1f;
    private float shield = 1f;

    private float speedMovement = 50f;
    private float speedRotation = 70f;
    private float maxVelocity = 50f;

    private bool canShootWeapon = true;
    private float shootSpeed = 0.25f;

    private float groundDistance = 7f;
    private bool isColliding = false;

    private float xRotation;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        rigidbodyPlayer = GetComponent<Rigidbody>();
        audioSourcePlayer = GetComponents<AudioSource>();
        canonAnimator = canon.GetComponent<Animator>();

        shieldBarUI.GetComponentInChildren<Slider>().value = shield;
        healthBarUI.GetComponentInChildren<Slider>().value = health;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canShootWeapon)
        {
            canonAnimator.SetTrigger("Shoot");

            audioSourcePlayer[0].Play();

            WeaponShoot();
        }
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * horizontalInput);
        turret.transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * mouseInputX);

        xRotation -= mouseInputY * speedRotation * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -15f, 15f);
        canon.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (IsGrounded())
        {
            rigidbodyPlayer.AddRelativeForce(Vector3.forward * speedMovement * verticalInput, ForceMode.VelocityChange);

            if (rigidbodyPlayer.velocity.magnitude > maxVelocity)
            {
                rigidbodyPlayer.velocity = rigidbodyPlayer.velocity.normalized * maxVelocity;
            }
        }

        rigidbodyPlayer.AddForce(Vector3.up * -100);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthItem"))
        {
            if (health < 1f)
            {
                if (health + 0.5f >= 1f)
                {
                    health = 1f;
                }
                else
                {
                    health += 0.5f;
                }

                healthBarUI.GetComponentInChildren<Slider>().value = health;

                audioSourcePlayer[2].Play();

                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("ShieldItem"))
        {
            if (shield < 1f)
            {
                if (shield + 1f >= 1f)
                {
                    shield = 1f;
                }
                else
                {
                    shield += 1f;
                }

                audioSourcePlayer[2].Play();

                shieldBarUI.GetComponentInChildren<Slider>().value = shield;

                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("EnemyBlast"))
        {
            if (isColliding) return;
            isColliding = true;

            if (shield > 0)
            {
                shield -= Mathf.Round(Random.Range(0.30f, 0.36f) * 100f) / 100f;
                shieldBarUI.GetComponentInChildren<Slider>().value = shield;

            }
            else if (health > 0)
            {
                health -= Mathf.Round(Random.Range(0.20f, 0.36f) * 100f) / 100f;
                healthBarUI.GetComponentInChildren<Slider>().value = health;
            }

            if (health <= 0)
            {
                gameManager.GameOver();
            }

            StartCoroutine(TriggerEnterOn());
        }
    }

    private IEnumerator TriggerEnterOn()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }

    private void WeaponShoot()
    {
        canonAnimator.SetTrigger("Shoot");

        Instantiate(blastPrefab, canon.transform.GetChild(0).position, canon.transform.GetChild(0).rotation);

        StartCoroutine(WeaponCooldown());
    }

    private IEnumerator WeaponCooldown()
    {
        canShootWeapon = false;
        yield return new WaitForSeconds(shootSpeed);
        canShootWeapon = true;
    }

    private bool IsGrounded()
    {
        RaycastHit hitData;

        Ray ray = new Ray(transform.position, -transform.up);

        Debug.DrawRay(transform.position, -transform.up * groundDistance, Color.cyan);

        if (Physics.Raycast(ray, out hitData, groundDistance))
        {
            return hitData.collider.CompareTag("Ground");
        }
        else
        {
            return false;
        }
    }
}

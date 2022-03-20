using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private SettingsManager settingsManager;

    public GameObject canon;
    public GameObject turret;
    public GameObject blastPrefab;
    public GameObject healthBarUI, shieldBarUI;
    public AudioSource[] audioSourcePlayer;

    private Rigidbody rigidbodyPlayer;
    private Animator canonAnimator;

    private float[] damageHealthRange = { 0.20f, 0.36f };
    private float[] damageShieldRange = { 0.30f, 0.36f };

    private float healthItemValue = 0.5f;
    private float shieldItemValue = 0.75f;

    private float horizontalInput, verticalInput, mouseInputX, mouseInputY, xRotation;

    public float mouseSensitivity = 0;

    private float healthPlayer = 1f;
    private float shieldPlayer = 1f;

    private float speedMovement = 50f;
    private float speedRotation = 80f;
    private float maxVelocity = 50f;

    private bool shootTrigger = true;
    private float shootCooldown = 0.25f;

    private float groundDistance = 7f;
    private bool isBlastColliding = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        settingsManager = FindObjectOfType<SettingsManager>();

        rigidbodyPlayer = GetComponent<Rigidbody>();
        audioSourcePlayer = GetComponents<AudioSource>();
        canonAnimator = canon.GetComponent<Animator>();

        shieldBarUI.GetComponentInChildren<Slider>().value = shieldPlayer;
        healthBarUI.GetComponentInChildren<Slider>().value = healthPlayer;

        rigidbodyPlayer.centerOfMass = new Vector3(0, -1, 0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && shootTrigger)
        {
            canonAnimator.SetTrigger("Shoot");

            audioSourcePlayer[0].Play();

            WeaponShoot();
        }
        
        mouseSensitivity = settingsManager.mouseSensitivity;
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime;
        verticalInput = Input.GetAxis("Vertical");

        mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        turret.transform.Rotate(Vector3.up * speedRotation * mouseInputX);

        xRotation -= mouseInputY * speedRotation;
        xRotation = Mathf.Clamp(xRotation, -15f, 15f);
        canon.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (IsGrounded())
        {
            transform.Rotate(Vector3.up * speedRotation * horizontalInput);
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
            if (healthPlayer < 1f)
            {
                if (healthPlayer + healthItemValue >= 1f)
                {
                    healthPlayer = 1f;
                }
                else
                {
                    healthPlayer += healthItemValue;
                }

                healthBarUI.GetComponentInChildren<Slider>().value = healthPlayer;

                audioSourcePlayer[2].Play();

                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("ShieldItem"))
        {
            if (shieldPlayer < 1f)
            {
                if (shieldPlayer + shieldItemValue >= 1f)
                {
                    shieldPlayer = 1f;
                }
                else
                {
                    shieldPlayer += shieldItemValue;
                }

                audioSourcePlayer[2].Play();

                shieldBarUI.GetComponentInChildren<Slider>().value = shieldPlayer;

                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("EnemyBlast"))
        {
            if (isBlastColliding) return;
            isBlastColliding = true;

            audioSourcePlayer[3].Play();

            if (shieldPlayer > 0)
            {
                shieldPlayer -= Mathf.Round(Random.Range(damageShieldRange[0], damageShieldRange[1]) * 100f) / 100f;
                shieldBarUI.GetComponentInChildren<Slider>().value = shieldPlayer;

            }
            else if (healthPlayer > 0)
            {
                healthPlayer -= Mathf.Round(Random.Range(damageHealthRange[0], damageHealthRange[1]) * 100f) / 100f;
                healthBarUI.GetComponentInChildren<Slider>().value = healthPlayer;
            }
           
            if (healthPlayer <= 0)
            {
                gameManager.GameOver();
            }

            StartCoroutine(TriggerEnterOn());
        }
    }

    private IEnumerator TriggerEnterOn()
    {
        yield return new WaitForEndOfFrame();
        isBlastColliding = false;
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

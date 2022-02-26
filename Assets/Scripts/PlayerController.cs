using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject blastPrefab;
    private Rigidbody rigidbodyPlayer;
    private AudioSource audioSourcePlayer;

    public AudioClip shootSFX;
    private Animator canonAnimator;

    private float mouseSensitivity = 2;

    private float horizontalInput, verticalInput, mouseInputX, mouseInputY;

    private bool canShootWeapon = true;

    public GameObject healthBar, shieldBar;
    private float health = 1f;
    private float shield = 1f;

    private float speedMovement = 20f;
    private float speedRotation = 70f;
    private float maxVelocity = 50f;

    private float shootSpeed = 0.25f;

    private float groundDistance = 6f;
    private bool isColliding = false;

    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        audioSourcePlayer = GetComponent<AudioSource>();
        canonAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        shieldBar.GetComponentInChildren<Slider>().value = shield;
        healthBar.GetComponentInChildren<Slider>().value = health;
    }

    private void FixedUpdate()
    {
        rigidbodyPlayer.AddForce(Vector3.up * -100);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * horizontalInput);

        if (IsGrounded())
        {
            rigidbodyPlayer.AddRelativeForce(Vector3.forward * speedMovement * verticalInput, ForceMode.VelocityChange);

            if (rigidbodyPlayer.velocity.magnitude > maxVelocity)
            {
                rigidbodyPlayer.velocity = rigidbodyPlayer.velocity.normalized * maxVelocity;
            }
        }

        mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.GetChild(0).transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * mouseInputX);
        
        transform.GetChild(0).GetChild(0).transform.Rotate(Vector3.left * speedRotation * Time.deltaTime * mouseInputY);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canShootWeapon)
        {
            canonAnimator.SetTrigger("Shoot");

            audioSourcePlayer.PlayOneShot(shootSFX, 0.3f);

            WeaponShoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthPowerUp"))
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

                healthBar.GetComponentInChildren<Slider>().value = health;

                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("ShieldPowerUp"))
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

                shieldBar.GetComponentInChildren<Slider>().value = shield;

                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("EnemyBlast"))
        {
            if (isColliding) return;
            isColliding = true;

            if (shield > 0)
            {
                shield -= 0.35f;
                shieldBar.GetComponentInChildren<Slider>().value = shield;

            }
            else if (health > 0)
            {
                health -= 0.25f;
                healthBar.GetComponentInChildren<Slider>().value = health;
            }

            if (health <= 0)
            {
                //Time.timeScale = 0;
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

        Instantiate(blastPrefab, transform.GetChild(0).GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).GetChild(0).rotation);

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

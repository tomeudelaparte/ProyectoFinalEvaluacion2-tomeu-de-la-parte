using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject blastPrefab;
    private Rigidbody rigidbodyPlayer;
    private AudioSource audioSourcePlayer;

    public AudioClip shootSFX;
    private Animator canonAnimator;

    private float horizontalInput, verticalInput, mouseInputX, mouseInputY;

    private bool canShootWeapon = true;

    private float speedMovement = 20f;
    private float speedRotation = 70f;
    private float maxVelocity = 50f;

    private float shootSpeed = 0.25f;

    private float groundDistance = 6f; 

    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        audioSourcePlayer = GetComponent<AudioSource>();
        canonAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rigidbodyPlayer.AddForce(Vector3.up * -100);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * horizontalInput);

        if(IsGrounded())
        {
            rigidbodyPlayer.AddRelativeForce(Vector3.forward * speedMovement * verticalInput, ForceMode.VelocityChange);

            if (rigidbodyPlayer.velocity.magnitude > maxVelocity)
            {
                rigidbodyPlayer.velocity = rigidbodyPlayer.velocity.normalized * maxVelocity;
            }
        }

        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        transform.GetChild(0).transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * mouseInputX);
        transform.GetChild(0).GetChild(0).transform.Rotate(Vector3.left * speedRotation * Time.deltaTime * mouseInputY);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canShootWeapon)
        {
            canonAnimator.SetTrigger("Shoot");

            audioSourcePlayer.PlayOneShot(shootSFX, 0.3f);

            weaponShoot();
        }
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

 
    private bool IsGrounded() {

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

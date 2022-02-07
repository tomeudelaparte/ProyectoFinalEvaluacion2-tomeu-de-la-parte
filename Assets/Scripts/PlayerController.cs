using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject blastPrefab;
    private Rigidbody rigidbodyPlayer;

    private float horizontalInput, verticalInput, mouseInputX, mouseInputY;

    private float speedMovement = 2f;
    private float speedRotation = 60f;

    private float interval;
    private float intervalTime = 0.25f;

    private bool canShoot = true;

    private Animator canonAnimator;

    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        canonAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        Physics.gravity *= 2f;
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * horizontalInput);
        rigidbodyPlayer.AddRelativeForce(Vector3.forward * speedMovement * verticalInput, ForceMode.VelocityChange);

        /*
        if(verticalInput == 0 && rigidbodyPlayer.velocity != Vector3.zero)
        {
            rigidbodyPlayer.velocity *= 0.8f;
        }
        */

        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        transform.GetChild(0).transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * mouseInputX);
        transform.GetChild(0).GetChild(0).transform.Rotate(Vector3.left * speedRotation * Time.deltaTime * mouseInputY);
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            Instantiate(blastPrefab, transform.GetChild(0).GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).GetChild(0).rotation);

            canonAnimator.SetTrigger("Shoot");

            interval += Time.time;
        }

        checkingWeapon();
    }

    private void checkingWeapon()
    {
        if (interval <= Time.time)
        {
            canShoot = true;
            interval = intervalTime;
        }
        else
        {
            canShoot = false;
        }
    }
}

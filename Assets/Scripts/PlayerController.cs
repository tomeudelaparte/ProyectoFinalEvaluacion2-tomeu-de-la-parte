using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject blastPrefab;
    private Rigidbody rigidbodyPlayer;

    private float horizontalInput, verticalInput, mouseInputX;

    private float speedMovement = 40f;
    private float speedRotation = 60f;

    private float interval;
    private float intervalTime = 0.25f;

    private bool canShoot = true;

    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbodyPlayer.AddForce(Vector3.up * -1000, ForceMode.Force);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseInputX = Input.GetAxis("Mouse X");

        transform.Translate(Vector3.forward * speedMovement * Time.deltaTime * verticalInput);
        transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * horizontalInput);

        transform.GetChild(0).transform.Rotate(Vector3.up * speedRotation * Time.deltaTime * mouseInputX);

        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            Instantiate(blastPrefab, transform.GetChild(0).GetChild(2).position, transform.GetChild(0).GetChild(2).rotation);

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

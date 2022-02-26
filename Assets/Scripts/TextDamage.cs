using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDamage: MonoBehaviour
{
    private float lifeTime = 2.5f;
    private GameObject cameraPlayer;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        cameraPlayer = GameObject.Find("Main Camera");
    }

    void Update()
    {
        transform.Translate(Vector3.up * 20 * Time.deltaTime, Space.World);

        transform.LookAt(cameraPlayer.transform.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}

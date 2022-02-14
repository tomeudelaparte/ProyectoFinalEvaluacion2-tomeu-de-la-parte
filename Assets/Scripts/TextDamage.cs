using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDamage: MonoBehaviour
{
    private float lifeTime = 5f;
    private GameObject player;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.Translate(Vector3.up * 20 * Time.deltaTime, Space.World);

        transform.LookAt(player.transform.position);
        //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

    }
}

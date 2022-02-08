using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonLookAt : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.LookAt(player.transform.position);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FramesPerSecond : MonoBehaviour
{
    private float deltaTime, fps;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        gameObject.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(fps).ToString();
    }
}

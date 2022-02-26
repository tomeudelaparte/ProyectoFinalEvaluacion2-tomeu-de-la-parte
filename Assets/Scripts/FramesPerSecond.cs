using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FramesPerSecond : MonoBehaviour
{
    public float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        gameObject.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(fps).ToString();
    }
}

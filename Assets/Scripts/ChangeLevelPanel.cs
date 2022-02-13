using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeLevelPanel : MonoBehaviour
{
    public GameObject[] text;
    private string[] levels = { "01", "02", "03", "04", "05" };

    private int textSelection = 0;

    void Start()
    {
        InvokeRepeating("changeLevel", 2, 2);
    }

    private void changeLevel()
    {
        foreach (GameObject number in text)
        {
            number.GetComponent<TextMeshPro>().text = levels[textSelection];
        }

        textSelection++;

        if (textSelection >= levels.Length)
        {
            textSelection = 0;
        }
    }
}

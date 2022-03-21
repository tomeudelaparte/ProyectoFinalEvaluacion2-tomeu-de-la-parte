using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    // Setea un valor con una key y guarda
    public void SavePrefs(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    // Devuelve un valor a partir de una key
    public string LoadPrefs(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    // Comprueba si la key existe
    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    // Elimina todos los PlayerPrefs guardados
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}

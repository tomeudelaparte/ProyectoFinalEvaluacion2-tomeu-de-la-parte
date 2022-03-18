using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPrefsManager : MonoBehaviour
{
   //public GameObject framesPerSecond;

    void Start()
    {
        //SetShowFps(bool.Parse(LoadPrefs("Show FPS")));
    }

    public void SavePrefs(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public string LoadPrefs(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SetScreenOptions(int width, int height, int display)
    {
        FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;

        switch (display)
        {
            case 0:

                mode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:

                mode = FullScreenMode.FullScreenWindow;
                break;

            case 2:

                mode = FullScreenMode.Windowed;
                break;
        }

        Screen.SetResolution(width, height, mode);
    }

    public void SetVerticalSync(bool isActivated)
    {
        if (isActivated)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void SetMaxFrames(int fps)
    {
        Application.targetFrameRate = fps;
    }

    public void SetShowFps(bool isActivated)
    {
        if (isActivated)
        {
            //framesPerSecond.SetActive(true);
        }
        else
        {
            //framesPerSecond.SetActive(false);
        }
    }

    public void SetAudioOptions(float master, float music, float effects)
    {

    }
}

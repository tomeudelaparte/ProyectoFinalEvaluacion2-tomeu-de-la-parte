using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System.Linq;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionList;
    public TextMeshProUGUI frameLimitText;
    public GameObject framerate;

    private PlayerController playerScript;
    private Resolution[] resolutions;

    public float mouseSensitivity = 2;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
    }

    public void LoadResolutionsAvailable()
    {
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
        System.Array.Reverse(resolutions);

        resolutionList.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length - 3; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionList.AddOptions(options);
        resolutionList.value = currentResolutionIndex;
        resolutionList.RefreshShownValue();
    }

    public void updateFrameSliderText(float frames)
    {
        frameLimitText.text = frames.ToString();
    }

    public void SetScreen(int displayIndex, int resolutionIndex)
    {
        FullScreenMode displayMode = FullScreenMode.ExclusiveFullScreen;

        switch (displayIndex)
        {
            case 0:

                displayMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:

                displayMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:

                displayMode = FullScreenMode.Windowed;
                break;
        }

        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, displayMode);
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

    public void SetFramerateLimit(int frames)
    {
        Application.targetFrameRate = frames;
    }

    public void SetShowFps(bool isActivated)
    {
        if (isActivated)
        {
            framerate.SetActive(true);
        }
        else
        {
            framerate.SetActive(false);
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}

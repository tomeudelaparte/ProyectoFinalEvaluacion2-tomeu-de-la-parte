using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    private PlayerPrefsManager playerPrefsManagerScript;
    private SettingsManager settingsManagerScript;

    public TMP_Dropdown displayModeDropdown, resolutionDropdown;
    public Toggle verticalSyncToggle;
    public Slider framerateLimitSlider;
    public Toggle showFPSToggle;

    public Slider masterVolumeSlider, musicVolumeSlider, effectsVolumeSlider;

    public Slider mouseSensivitySlider;

    private string[] videoSettingsKeys = { "DISPLAY MODE", "RESOLUTION", "VERTICAL SYNC", "FPS LIMIT", "SHOW FPS" };
    private string[] audioSettingsKeys = { "MASTER VOLUME", "MUSIC VOLUME", "EFFECTS VOLUME" };
    private string[] controlSettingsKeys = { "MOUSE SENSIVITY" };

    void Start()
    {
        playerPrefsManagerScript = FindObjectOfType<PlayerPrefsManager>();
        settingsManagerScript = FindObjectOfType<SettingsManager>();

        settingsManagerScript.LoadResolutionsAvailable();

        LoadSettings();
        SaveSettings();
    }

    public void SaveSettings()
    {
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[0], displayModeDropdown.value.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[1], resolutionDropdown.value.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[2], verticalSyncToggle.isOn.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[3], framerateLimitSlider.value.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[4], showFPSToggle.isOn.ToString());

        playerPrefsManagerScript.SavePrefs(audioSettingsKeys[0], masterVolumeSlider.value.ToString());
        playerPrefsManagerScript.SavePrefs(audioSettingsKeys[1], musicVolumeSlider.value.ToString());
        playerPrefsManagerScript.SavePrefs(audioSettingsKeys[2], effectsVolumeSlider.value.ToString());

        playerPrefsManagerScript.SavePrefs(controlSettingsKeys[0], mouseSensivitySlider.value.ToString());

        ApplySettings();
    }

    public void LoadSettings()
    {
        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[0]))
        {
            displayModeDropdown.value = int.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[0]));
        }

        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[1]))
        {
            resolutionDropdown.value = int.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[1]));
        }

        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[2]))
        {
            verticalSyncToggle.isOn = bool.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[2]));
        }

        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[3]))
        {
            framerateLimitSlider.value = int.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[3]));
        }

        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[4]))
        {
            showFPSToggle.isOn = bool.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[4]));
        }

        if (playerPrefsManagerScript.HasKey(audioSettingsKeys[0]))
        {
            masterVolumeSlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(audioSettingsKeys[0]));
        }

        if (playerPrefsManagerScript.HasKey(audioSettingsKeys[1]))
        {
            musicVolumeSlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(audioSettingsKeys[1]));
        }

        if (playerPrefsManagerScript.HasKey(audioSettingsKeys[2]))
        {
            effectsVolumeSlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(audioSettingsKeys[2]));
        }

        if (playerPrefsManagerScript.HasKey(controlSettingsKeys[0]))
        {
            mouseSensivitySlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(controlSettingsKeys[0]));
        }
    }

    public void ApplySettings()
    {
        settingsManagerScript.SetScreen(displayModeDropdown.value, resolutionDropdown.value);
        settingsManagerScript.SetVerticalSync(verticalSyncToggle.isOn);
        settingsManagerScript.SetFramerateLimit(int.Parse(framerateLimitSlider.value.ToString()));
        settingsManagerScript.SetShowFps(showFPSToggle.isOn);

        settingsManagerScript.SetMasterVolume(masterVolumeSlider.value);
        settingsManagerScript.SetMusicVolume(musicVolumeSlider.value);
        settingsManagerScript.SetEffectsVolume(effectsVolumeSlider.value);

        settingsManagerScript.SetMouseSensitivity(mouseSensivitySlider.value);
    }
}

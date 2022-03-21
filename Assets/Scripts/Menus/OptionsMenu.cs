using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    // Referencias para PlayerPrefsManager y SettingsManager
    private PlayerPrefsManager playerPrefsManagerScript;
    private SettingsManager settingsManagerScript;

    // Variables para referencias las opciones del menu de ajustes
    public TMP_Dropdown displayModeDropdown, resolutionDropdown;
    public Toggle verticalSyncToggle;
    public Slider framerateLimitSlider;
    public Toggle showFPSToggle;

    public Slider masterVolumeSlider, musicVolumeSlider, effectsVolumeSlider;

    public Slider mouseSensivitySlider;

    // Arrays de keys
    private string[] videoSettingsKeys = { "DISPLAY MODE", "RESOLUTION", "VERTICAL SYNC", "FPS LIMIT", "SHOW FPS" };
    private string[] audioSettingsKeys = { "MASTER VOLUME", "MUSIC VOLUME", "EFFECTS VOLUME" };
    private string[] controlSettingsKeys = { "MOUSE SENSIVITY" };

    void Start()
    {
        // Obtiene las referencias
        playerPrefsManagerScript = FindObjectOfType<PlayerPrefsManager>();
        settingsManagerScript = FindObjectOfType<SettingsManager>();

        // Añade la lista de resoluciones disponibles
        settingsManagerScript.LoadResolutionsAvailable();

        // Carga y guarda los ajustes.
        LoadSettings();
        SaveSettings();
    }

    // Guarda los ajustes del menu de opciones
    public void SaveSettings()
    {
        // Guarda los valores de los ajustes de video parseados a String con su key correspondiente
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[0], displayModeDropdown.value.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[1], resolutionDropdown.value.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[2], verticalSyncToggle.isOn.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[3], framerateLimitSlider.value.ToString());
        playerPrefsManagerScript.SavePrefs(videoSettingsKeys[4], showFPSToggle.isOn.ToString());

        // Guarda los valores de los ajustes de sonido parseados a String con su key correspondiente
        playerPrefsManagerScript.SavePrefs(audioSettingsKeys[0], masterVolumeSlider.value.ToString());
        playerPrefsManagerScript.SavePrefs(audioSettingsKeys[1], musicVolumeSlider.value.ToString());
        playerPrefsManagerScript.SavePrefs(audioSettingsKeys[2], effectsVolumeSlider.value.ToString());

        // Guarda los valores de los ajustes de controles parseados a String con su key correspondiente
        playerPrefsManagerScript.SavePrefs(controlSettingsKeys[0], mouseSensivitySlider.value.ToString());

        // Aplica los ajustes del menu de opciones
        ApplySettings();
    }

    // Carga los ajustes guardados del menu de opciones
    public void LoadSettings()
    {
        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[0]))
        {
            // Parsea el valor String obtenido con la Key
            displayModeDropdown.value = int.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[0]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[1]))
        {
            // Parsea el valor String obtenido con la Key
            resolutionDropdown.value = int.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[1]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[2]))
        {
            // Parsea el valor String obtenido con la Key
            verticalSyncToggle.isOn = bool.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[2]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[3]))
        {
            // Parsea el valor String obtenido con la Key
            framerateLimitSlider.value = int.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[3]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(videoSettingsKeys[4]))
        {
            // Parsea el valor String obtenido con la Key
            showFPSToggle.isOn = bool.Parse(playerPrefsManagerScript.LoadPrefs(videoSettingsKeys[4]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(audioSettingsKeys[0]))
        {
            // Parsea el valor String obtenido con la Key
            masterVolumeSlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(audioSettingsKeys[0]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(audioSettingsKeys[1]))
        {
            // Parsea el valor String obtenido con la Key
            musicVolumeSlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(audioSettingsKeys[1]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(audioSettingsKeys[2]))
        {
            // Parsea el valor String obtenido con la Key
            effectsVolumeSlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(audioSettingsKeys[2]));
        }

        // Comprueba si existe la key
        if (playerPrefsManagerScript.HasKey(controlSettingsKeys[0]))
        {
            // Parsea el valor String obtenido con la Key
            mouseSensivitySlider.value = float.Parse(playerPrefsManagerScript.LoadPrefs(controlSettingsKeys[0]));
        }
    }

    // Aplica los ajustes del menu de opciones
    public void ApplySettings()
    {
        // Aplica la resolución y el modo de pantalla
        settingsManagerScript.SetScreen(displayModeDropdown.value, resolutionDropdown.value);

        // Aplica la sincronización vertical
        settingsManagerScript.SetVerticalSync(verticalSyncToggle.isOn);

        // Aplica el limite de frames
        settingsManagerScript.SetFramerateLimit(int.Parse(framerateLimitSlider.value.ToString()));

        // Aplica el mostrar los frames
        settingsManagerScript.SetShowFps(showFPSToggle.isOn);

        // Aplica el volumen de Master, Música y Efectos
        settingsManagerScript.SetMasterVolume(masterVolumeSlider.value);
        settingsManagerScript.SetMusicVolume(musicVolumeSlider.value);
        settingsManagerScript.SetEffectsVolume(effectsVolumeSlider.value);

        // Aplica la sensibilidad de movimiento del canon
        settingsManagerScript.SetMouseSensitivity(mouseSensivitySlider.value);
    }
}

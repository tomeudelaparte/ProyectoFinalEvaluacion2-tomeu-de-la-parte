using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System.Linq;

public class SettingsManager : MonoBehaviour
{
    // Variables para referencias
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionList;
    public TextMeshProUGUI frameLimitText;
    public GameObject framerate;

    // Array de resoluciones de pantalla
    private Resolution[] resolutions;

    // Sensibilidad del movimiento del canon
    public float mouseSensitivity = 2;

    // Carga las resoluciones disponibles
    public void LoadResolutionsAvailable()
    {
        // Obtiene las resoluciones de 60Hz
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();

        // Invierte el orden del array
        System.Array.Reverse(resolutions);

        // Limpia todas las opciones
        resolutionList.ClearOptions();

        List<string> options = new List<string>();

        // Index de la resolución actual
        int currentResolutionIndex = 0;

        // Recorre todas las resoluciones menos las 3 últimas
        for (int i = 0; i < resolutions.Length - 3; i++)
        {
            // Crea y añade una opción con este formato en String
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Si la resolución actual es igual a la resolución del array
            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                // Guarda el Index de la resolución
                currentResolutionIndex = i;
            }
        }

        // Añade las opciones a la lista
        resolutionList.AddOptions(options);

        // Selecciona la resolución actual en la lista
        resolutionList.value = currentResolutionIndex;

        // Refresca el valor seleccionado
        resolutionList.RefreshShownValue();
    }

    // Actualiza el número de frames límite del slider
    public void UpdateFrameSliderText(float frames)
    {
        frameLimitText.text = frames.ToString();
    }

    // Aplica los ajustes de resolución y modo de pantalla
    public void SetScreen(int displayIndex, int resolutionIndex)
    {
        // Valor default de displayMode
        FullScreenMode displayMode = FullScreenMode.ExclusiveFullScreen;

        // Selección según el index obtenido
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

        // Setea la resolución y el modo de pantalla
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, displayMode);
    }

    // Activa la sincronización vertical según el valor obtenido
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

    // Aplica el límite de frames
    public void SetFramerateLimit(int frames)
    {
        Application.targetFrameRate = frames;
    }

    // Activa que se muestren los frames según el valor obtenido
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

    // Aplica el volumen de Master
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    // Aplica el volumen de Music
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    // Aplica el volumen de Effects
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    // Aplica la sensibilidad del movimiento del canon
    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}

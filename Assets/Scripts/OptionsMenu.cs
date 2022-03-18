using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [System.Serializable]
    public class OptionClass
    {
        public GameObject option;
        public string key;
        public string value;
    }

    [System.Serializable]
    public class ResolutionClass
    {
        public int width;
        public int height;

        public string toString()
        {
            return width + " X " + height;
        }
    }

    private SettingsManager settingsManager;
    public GameObject sliderFrameText;

    public List<OptionClass> videoSettings = new List<OptionClass>();
    public List<OptionClass> audioSettings = new List<OptionClass>();

    public List<ResolutionClass> resolutionList = new List<ResolutionClass>();
    public List<string> displayModeList = new List<string>();

    void Start()
    {
        settingsManager = FindObjectOfType<SettingsManager>();

        settingsManager.DeleteAll();

        if (settingsManager.HasKey(videoSettings[0].key) == false)
        {
            settingsManager.DeleteAll();

            DefaultSettings();

            ApplySettings();
        }

        LoadOptions();
    }

    private void Update()
    {
        sliderFrameText.GetComponent<TextMeshProUGUI>().text = videoSettings[3].option.GetComponentInChildren<Slider>().value.ToString();
    }

    public void SaveOptions()
    {
        SaveVideoSettings();
        SaveAudioSettings();

        ApplySettings();
    }

    public void LoadOptions()
    {
        LoadVideoSettings();
        LoadAudioSettings();
    }

    private void DefaultSettings()
    {
        DefaultVideoSettings();
        DefaultAudioSettings();
    }

    private void LoadVideoSettings()
    {
        LoadListOptions();

        videoSettings[0].option.GetComponentInChildren<TMP_Dropdown>().value = int.Parse(settingsManager.LoadPrefs(videoSettings[0].key));
        videoSettings[1].option.GetComponentInChildren<TMP_Dropdown>().value = int.Parse(settingsManager.LoadPrefs(videoSettings[1].key));
        videoSettings[2].option.GetComponentInChildren<Toggle>().isOn = bool.Parse(settingsManager.LoadPrefs(videoSettings[2].key));
        videoSettings[3].option.GetComponentInChildren<Slider>().value = float.Parse(settingsManager.LoadPrefs(videoSettings[3].key));
        videoSettings[4].option.GetComponentInChildren<Toggle>().isOn = bool.Parse(settingsManager.LoadPrefs(videoSettings[4].key));
    }

    private void LoadAudioSettings()
    {
        audioSettings[0].option.GetComponentInChildren<Slider>().value = float.Parse(settingsManager.LoadPrefs(audioSettings[0].key));
        audioSettings[1].option.GetComponentInChildren<Slider>().value = float.Parse(settingsManager.LoadPrefs(audioSettings[1].key));
        audioSettings[2].option.GetComponentInChildren<Slider>().value = float.Parse(settingsManager.LoadPrefs(audioSettings[2].key));
    }

    private void SaveVideoSettings()
    {
        settingsManager.SavePrefs(videoSettings[0].key, videoSettings[0].option.GetComponentInChildren<TMP_Dropdown>().value.ToString());
        settingsManager.SavePrefs(videoSettings[1].key, videoSettings[1].option.GetComponentInChildren<TMP_Dropdown>().value.ToString());
        settingsManager.SavePrefs(videoSettings[2].key, videoSettings[2].option.GetComponentInChildren<Toggle>().isOn.ToString());
        settingsManager.SavePrefs(videoSettings[3].key, videoSettings[3].option.GetComponentInChildren<Slider>().value.ToString()); ;
        settingsManager.SavePrefs(videoSettings[4].key, videoSettings[4].option.GetComponentInChildren<Toggle>().isOn.ToString());
    }

    private void SaveAudioSettings()
    {
        settingsManager.SavePrefs(audioSettings[0].key, audioSettings[0].option.GetComponentInChildren<Slider>().value.ToString());
        settingsManager.SavePrefs(audioSettings[1].key, audioSettings[1].option.GetComponentInChildren<Slider>().value.ToString());
        settingsManager.SavePrefs(audioSettings[2].key, audioSettings[2].option.GetComponentInChildren<Slider>().value.ToString());
    }

    private void DefaultVideoSettings()
    {
        foreach (OptionClass setting in videoSettings)
        {
            settingsManager.SavePrefs(setting.key, setting.value);
        }
    }

    private void DefaultAudioSettings()
    {
        foreach (OptionClass setting in audioSettings)
        {
            settingsManager.SavePrefs(setting.key, setting.value);
        }
    }

    private void LoadListOptions()
    {
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();

        foreach (string display in displayModeList)
        {

            list.Add(new TMP_Dropdown.OptionData(display));
        }

        videoSettings[0].option.GetComponentInChildren<TMP_Dropdown>().AddOptions(list);


        List<TMP_Dropdown.OptionData> list02 = new List<TMP_Dropdown.OptionData>();

        foreach (ResolutionClass resolution in resolutionList)
        {

            list02.Add(new TMP_Dropdown.OptionData(resolution.toString()));
        }

        videoSettings[1].option.GetComponentInChildren<TMP_Dropdown>().AddOptions(list02);
    }

    private void ApplySettings()
    {
        settingsManager.SetScreenOptions(
            resolutionList[int.Parse(settingsManager.LoadPrefs(videoSettings[1].key))].width,
            resolutionList[int.Parse(settingsManager.LoadPrefs(videoSettings[1].key))].height,
            int.Parse(settingsManager.LoadPrefs(videoSettings[0].key)));

        settingsManager.SetMaxFrames(int.Parse(settingsManager.LoadPrefs(videoSettings[3].key)));

        settingsManager.SetVerticalSync(bool.Parse(settingsManager.LoadPrefs(videoSettings[2].key)));

        settingsManager.SetShowFps(bool.Parse(settingsManager.LoadPrefs(videoSettings[4].key)));
    }
}

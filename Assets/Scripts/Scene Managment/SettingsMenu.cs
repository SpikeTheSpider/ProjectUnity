using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle musicToggle;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    void Start()
    {
        LoadResolutions();
        LoadSettings();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void SetMusic()
    {
        AudioListener.pause = !musicToggle.isOn;
        PlayerPrefs.SetInt("Music", musicToggle.isOn ? 1 : 0);
    }

    public void SetResolution()
    {
        Resolution selectedResolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.Save();
    }

    private void LoadResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.RefreshShownValue();
    }

    private void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        int savedMusic = PlayerPrefs.GetInt("Music", 1);
        int savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1);
        int savedResolution = PlayerPrefs.GetInt("Resolution", 0);

        resolutionDropdown.value = savedResolution;
        SetResolution();

        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        musicToggle.isOn = (savedMusic == 1);
        AudioListener.pause = !(savedMusic == 1);

        fullscreenToggle.isOn = (savedFullscreen == 1);
        Screen.fullScreen = (savedFullscreen == 1);
    }
}
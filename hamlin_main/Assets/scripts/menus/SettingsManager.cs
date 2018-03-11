using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

  public Toggle fullscreenToggle;
  public Dropdown resolutionDropdown;
  public Dropdown antialiasingDropdown;
  public Dropdown vSyncDropdown;
  public Slider musicVolumeSlider;

  public AudioSource musicSource;
  public Resolution[] resolutions;
  public GameSettings gameSettings;

  void OnEnable()
  {

    gameSettings = new GameSettings();

    fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });
    resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
    antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
    vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
    musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });

    resolutions = Screen.resolutions;
    foreach (Resolution resolution in resolutions)
    {
      Dropdown.OptionData data = new Dropdown.OptionData(resolution.ToString());
      resolutionDropdown.options.Add(data);
    }
  }

  public void OnFullScreenToggle()
  {

    gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
  }

  public void OnResolutionChange()
  {
    Screen.SetResolution(resolutions[resolutionDropdown.value].width,
      resolutions[resolutionDropdown.value].height, Screen.fullScreen);
  }

  public void OnAntialiasingChange()
  {

    QualitySettings.antiAliasing = gameSettings.antialiasing = (int)Mathf.Pow(2f, antialiasingDropdown.value);
  }

  public void OnVSyncChange()
  {

    QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
  }

  public void OnMusicVolumeChange()
  {

    AudioListener.volume = musicVolumeSlider.value;
    gameSettings.musicVolume = musicVolumeSlider.value;

  }

  public void SaveSettings()
  {

  }

  public void LoadSettings()
  {

  }

}

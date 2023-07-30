using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;
    [SerializeField] private Slider gameVolumeSlider;

    float mainVolume;
    float musicVolume;
    float uiVolume;
    float gameVolume;
    int fullscreen;
    Resolution[] resolutions;

    [SerializeField] bool setVolumesOnSave = false;

    void Start()
    {
        LoadSettings();
        resolutions = Screen.resolutions;
    }

    public void SetMainVolume(float volume)
    {
        if(volume < -40) volume = -80;
        mainVolume = volume;
        audioMixer.SetFloat("main", mainVolume);
    } 

    public void SetMusicVolume(float volume)
    {
        if(volume < -40) volume = -80;
        musicVolume = volume;
    } 

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if(isFullscreen) fullscreen = 1;
        else fullscreen = 0;

        if(isFullscreen)
        {
            resolutions = Screen.resolutions;
            Resolution resolution = resolutions[resolutions.Length-1];
            Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        }
        else
        {
            Screen.SetResolution(960, 540, isFullscreen);
        }
    }
    public void SetUIVolume(float volume)
    {
        if(volume < -40) volume = -80;
        uiVolume = volume;
        audioMixer.SetFloat("ui", uiVolume);
    } 
    public void SetGameVolume(float volume)
    {
        if(volume < -40) volume = -80;
        gameVolume = volume;
    } 

    public void LoadSettings()
    {
        //Set all volumes
        mainVolume = PlayerPrefs.GetFloat("main_volume", 0f);
        mainVolumeSlider.value = mainVolume;
        if(mainVolume < -40) mainVolume = -80;
        audioMixer.SetFloat("main", mainVolume);

        musicVolume = PlayerPrefs.GetFloat("music_volume", -5f);
        musicVolumeSlider.value = musicVolume;
        if(musicVolume < -40) musicVolume = -80;
        audioMixer.SetFloat("music", musicVolume);

        uiVolume = PlayerPrefs.GetFloat("ui_volume", -5f);
        uiVolumeSlider.value = uiVolume;
        if(uiVolume < -40) uiVolume = -80;
        audioMixer.SetFloat("ui", uiVolume);

        gameVolume = uiVolume = PlayerPrefs.GetFloat("game_volume", -5f);
        gameVolumeSlider.value = gameVolume;
        if(gameVolume < -40) gameVolume = -80;
        audioMixer.SetFloat("game", gameVolume);

        fullscreen = PlayerPrefs.GetInt("fullscreen", 0);
        if(fullscreen == 1) 
        {
            SetFullscreen(true);
            fullscreenToggle.isOn = true; 
        }
        else 
        {
            SetFullscreen(false);
            fullscreenToggle.isOn = false; 
        }
    }

    public void SaveSettings()
    {
        if(setVolumesOnSave)
        {
            audioMixer.SetFloat("main_volume", mainVolume);
            audioMixer.SetFloat("music_volume", musicVolume);
            audioMixer.SetFloat("ui_volume", uiVolume);
            audioMixer.SetFloat("game_volume", gameVolume);
        }

        PlayerPrefs.SetFloat("main_volume", mainVolume);
        PlayerPrefs.SetFloat("music_volume", musicVolume);
        PlayerPrefs.SetFloat("ui_volume", uiVolume);
        PlayerPrefs.SetFloat("game_volume", gameVolume);

        PlayerPrefs.SetInt("fullscreen", fullscreen);
    }
    
    public void ResetVolumes()
    {
        if(setVolumesOnSave)
        {
            audioMixer.SetFloat("main_volume", mainVolume);
            audioMixer.SetFloat("music_volume", musicVolume);
            audioMixer.SetFloat("ui_volume", uiVolume);
            audioMixer.SetFloat("game_volume", gameVolume);
        }
        
        mainVolumeSlider.value = PlayerPrefs.GetFloat("main_volume", 0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("music_volume", 0f);
        uiVolumeSlider.value = PlayerPrefs.GetFloat("ui_volume", 0f);
        gameVolumeSlider.value = PlayerPrefs.GetFloat("game_volume", 0f);
    }
}

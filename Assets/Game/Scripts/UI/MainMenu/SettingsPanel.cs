using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject panelObject;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider soundsVolumeSlider;
    [SerializeField]
    private Slider sensivitySlider;

    private AudioVolumeSystem volumeSystem;

    public void Init()
    {
        volumeSystem = FindObjectOfType<AudioVolumeSystem>();
        musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        soundsVolumeSlider.onValueChanged.AddListener(SoundVolumeChanged);
        sensivitySlider.onValueChanged.AddListener(SensivityChanged);
        CheckSliders();
        panelObject.SetActive(false);
    }

    public void ShowPanel()
    {
        panelObject.SetActive(true);
        CheckSliders();
    }

    public void CheckSliders()
    {
        musicVolumeSlider.value = volumeSystem.MusicVolume;
        soundsVolumeSlider.value = volumeSystem.SoundVolume;
        sensivitySlider.value = GameSettings.sensivity;
    }
    private void MusicVolumeChanged(float value)
    {
        volumeSystem.MusicVolume = value;
    }
    private void SoundVolumeChanged(float value)
    {
        volumeSystem.SoundVolume = value;
    }
    private void SensivityChanged(float value)
    {
        GameSettings.sensivity = value;
    }
}

public static class GameSettings
{
    public static float musicVolume = 1f;
    public static float soundVolume = 1f;
    public static float sensivity = 1f;
}

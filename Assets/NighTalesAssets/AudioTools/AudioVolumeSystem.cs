using System;
using UnityEngine;

public class AudioVolumeSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource mainSource;

    [SerializeField, Range(0, 1)]
    private float musicVolume;
    [SerializeField, Range(0, 1)]
    private float soundVolume;

    public float MusicVolume 
    { 
        get
        {
            return GameSettings.musicVolume;
        }
        set
        {
            GameSettings.musicVolume = value;
            VolumeChanged?.Invoke(InGameAudioType.music, value);
        }
    }
    public float SoundVolume
    {
        get
        {
            return GameSettings.soundVolume;
        }
        set
        {
            GameSettings.soundVolume = value;
            VolumeChanged?.Invoke(InGameAudioType.sounds, value);
        }
    }

    public Action<InGameAudioType, float> VolumeChanged;

    private void Awake()
    {
        GameCenter.mainSource = mainSource;
    }

    private void Start()
    {
        MusicVolume = musicVolume;
        SoundVolume = soundVolume;
    }
}
public enum InGameAudioType
{
    music,
    sounds
}


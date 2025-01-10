using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVolumeChanger : MonoBehaviour
{
    [SerializeField]
    private AudioType type;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        try
        {
            AudioVolumeSystem audioCenter = FindObjectOfType<AudioVolumeSystem>();
            audioCenter.VolumeChanged += SetValue;
        }
        catch(NullReferenceException)
        {
            Debug.LogError("Объект с настройками громкости звука не найден");
        }
    }

    private void Start()
    {
        if (type == AudioType.sounds)
        {

            source.volume = GameSettings.soundVolume;
        }
        else if (type == AudioType.music)
        {
            source.volume = GameSettings.musicVolume;
        }
    }

    public void SetValue(AudioType targetType, float newWalue)
    {
        if(targetType == type)
        {
            source.volume = newWalue;
        }
    }
}


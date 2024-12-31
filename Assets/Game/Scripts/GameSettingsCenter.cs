using System;
using TMPro;
using UnityEngine;

public class GameSettingsCenter : MonoBehaviour
{
    [SerializeField, Range(30, 120)]
    private int FPS = 60;
    [SerializeField]
    private AudioSource mainSource;
    [SerializeField]
    private TMP_Text waveNameText;

    void Awake()
    {
        Application.targetFrameRate = FPS;
        GameCenter.mainSource = mainSource;
        GameCenter.settings = this;
        GameCenter.waveNameText = waveNameText;
    }
}

public static class GameCenter
{
    public static int maxLevel = 0;

    public static GameSettingsCenter settings;
    public static AudioSource mainSource;
    public static TMP_Text waveNameText;

    public static float CurrentRageValue
    {
        get
        {
            return _currnetRageValue;
        }
        set
        {
            _currnetRageValue = Mathf.Clamp(value, 0 , 100);
            CurrentRageChanged?.Invoke(_currnetRageValue);
        }
    }
    private static float _currnetRageValue;

    public static event Action<float> CurrentRageChanged;
}

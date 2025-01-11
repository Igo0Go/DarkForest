using System;
using UnityEngine;

public class GameSettingsCenter : MonoBehaviour
{
    [SerializeField, Range(30, 120)]
    private int FPS = 60;


    void Awake()
    {
        Application.targetFrameRate = FPS;
        GameCenter.settings = this;
    }
}



public static class GameCenter
{
    public static int maxLevel = 0;

    public static GameSettingsCenter settings;
    public static AudioSource mainSource;
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

    public static int CurrentRageMultiplicator
    {
        get
        {
            return _currentRageMultiplicator;
        }
        set
        {
            _currentRageMultiplicator = value;
            CurrentRageMultiplicatorChanged?.Invoke(_currentRageMultiplicator);
        }
    }
    private static int _currentRageMultiplicator;

    public static Transform CurrentArenaCenter;

    public static event Action<float> CurrentRageChanged;
    public static event Action<int> CurrentRageMultiplicatorChanged;

    public static void ClearEvents()
    {
        CurrentRageChanged = null;
        CurrentRageMultiplicatorChanged = null;
    }
}

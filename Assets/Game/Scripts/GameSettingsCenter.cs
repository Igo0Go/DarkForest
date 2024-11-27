using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private Transform arenaCenter;
    [SerializeField]
    private AudioSource musicSource;

    void Awake()
    {
        Application.targetFrameRate = FPS;
        GameCenter.mainSource = mainSource;
        GameCenter.settings = this;
        GameCenter.waveNameText = waveNameText;
        GameCenter.arenaCenter = arenaCenter;
        GameCenter.musicSource = musicSource;
    }
}

public static class GameCenter
{
    public static Transform arenaCenter;
    public static GameSettingsCenter settings;
    public static AudioSource mainSource;
    public static TMP_Text waveNameText;
    public static AudioSource musicSource;
}

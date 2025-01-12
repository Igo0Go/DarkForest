using System;
using System.Collections;
using UnityEngine;


public class MusicRageSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource defaultMusicSource;
    [SerializeField]
    private AudioSource arenaMusicSource;
    [SerializeField]
    private MusicRagePack pack;

    public Action<float, float, float, int> RageInfoChanged;

    [SerializeField, Min(1)]
    private float rage2Threshold = 25;
        [SerializeField, Min(1)]
    private float rage3Threshold = 75;

    private bool inFight = false;
    private float currentMin;
    private float currentMax;

    public void Init()
    {
        GameCenter.CurrentRageChanged += OnRageValueChanged;
        GameCenter.CurrentRageMultiplicator = 1;
        GameCenter.CurrentRageValue = 0;

        currentMin = 0;
        currentMax = rage2Threshold;

        RageInfoChanged.Invoke(GameCenter.CurrentRageValue, currentMin, currentMax, 
            GameCenter.CurrentRageMultiplicator);
    }

    public void ChangeMusicToArena()
    {
        inFight = true;
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ChangeMusicToArenaCoroutine());
    }
    public void ChangeMusicToDefault()
    {
        GameCenter.CurrentRageValue = 0;
        inFight = false;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ChangeMusicToDefaultCoroutine());
    }

    private void OnRageValueChanged(float value)
    {
        if (!inFight)
        {
            return;
        }

        if (GameCenter.CurrentRageMultiplicator != 1 && value < rage2Threshold)
        {
            ChangeMusic(pack.rage1Clip);
            GameCenter.CurrentRageMultiplicator = 1;
            currentMin = 0;
            currentMax = rage2Threshold;

        }
        else if (GameCenter.CurrentRageMultiplicator != 2 && value >= rage2Threshold && value < rage3Threshold)
        {
            ChangeMusic(pack.rage2Clip);
            GameCenter.CurrentRageMultiplicator = 2;
            currentMin = rage2Threshold;
            currentMax = rage3Threshold;

        }
        else if (GameCenter.CurrentRageMultiplicator != 3 && value >= rage3Threshold)
        {
            ChangeMusic(pack.rage3Clip);
            GameCenter.CurrentRageMultiplicator = 3;
            currentMin = currentMax = rage3Threshold;
        }

        RageInfoChanged.Invoke(GameCenter.CurrentRageValue, currentMin, currentMax,
    GameCenter.CurrentRageMultiplicator);
    }
    private void ChangeMusic(AudioClip clip)
    {
        arenaMusicSource.Stop();
        arenaMusicSource.PlayOneShot(pack.changer);
        arenaMusicSource.clip = clip;
        arenaMusicSource.Play();
    }

    private Coroutine currentCoroutine;
    private IEnumerator ChangeMusicToArenaCoroutine()
    {
        while (defaultMusicSource.volume > 0)
        {
            defaultMusicSource.volume -= Time.deltaTime;
            yield return null;
        }
        defaultMusicSource.volume = 0;
        yield return null;
        defaultMusicSource.Stop();
        arenaMusicSource.clip = pack.rage1Clip;
        arenaMusicSource.volume = 0;
        arenaMusicSource.Play();

        while (arenaMusicSource.volume < GameSettings.musicVolume)
        {
            arenaMusicSource.volume += Time.deltaTime;
            yield return null;
        }

        arenaMusicSource.volume = GameSettings.musicVolume;
        currentCoroutine = null;
    }
    private IEnumerator ChangeMusicToDefaultCoroutine()
    {
        while (arenaMusicSource.volume > 0)
        {
            arenaMusicSource.volume -= Time.deltaTime;
            yield return null;
        }

        arenaMusicSource.Stop();
        defaultMusicSource.volume = 0;
        defaultMusicSource.Play();

        while (defaultMusicSource.volume < GameSettings.musicVolume)
        {
            defaultMusicSource.volume += Time.deltaTime;
            yield return null;
        }

        defaultMusicSource.volume = GameSettings.musicVolume;
        currentCoroutine = null;
    }
}

public enum RageStage
{
    x1,
    x2,
    x3
}

[System.Serializable]
public class MusicRagePack
{
    public AudioClip rage1Clip;
    public AudioClip rage2Clip;
    public AudioClip rage3Clip;
    public AudioClip changer;
}

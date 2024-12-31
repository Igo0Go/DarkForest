using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class MusicRageSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource defaultMusicSource;
    [SerializeField]
    private AudioSource arenaMusicSource;
    [SerializeField]
    private MusicRagePack pack;

    [SerializeField, Min(1)]
    private float rage2Threshold = 25;
        [SerializeField, Min(1)]
    private float rage3Threshold = 75;

    private RageStage stage;

    private void Awake()
    {
        stage = RageStage.x1;
        GameCenter.CurrentRageChanged += OnRageValueChanged;
    }

    public void ChangeMusicToArena()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ChangeMusicToArenaCoroutine());
    }
    public void ChangeMusicToDefault()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ChangeMusicToDefaultCoroutine());
    }

    private void OnRageValueChanged(float value)
    {

        Debug.Log(value);

        if (!arenaMusicSource.isPlaying)
        {
            return;
        }

        if (stage != RageStage.x1 && value < rage2Threshold)
        {
            ChangeMusic(pack.rage1Clip);
            stage = RageStage.x1;
        }
        else if (stage != RageStage.x2 && value >= rage2Threshold && value < rage3Threshold)
        {
            ChangeMusic(pack.rage2Clip);
            stage = RageStage.x2;
        }
        else if (stage != RageStage.x3 && value >= rage3Threshold)
        {
            ChangeMusic(pack.rage3Clip);
            stage = RageStage.x3;
        }
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

        while (arenaMusicSource.volume < 1)
        {
            arenaMusicSource.volume += Time.deltaTime;
            yield return null;
        }

        arenaMusicSource.volume = 1;
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

        while (defaultMusicSource.volume < 1)
        {
            defaultMusicSource.volume += Time.deltaTime;
            yield return null;
        }

        defaultMusicSource.volume = 1;
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

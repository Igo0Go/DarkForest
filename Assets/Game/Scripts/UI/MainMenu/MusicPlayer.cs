using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip defaultClip;

    public void PlayNewClip(AudioClip clip)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeMusicCoroutine(clip));
    }

    public void ToDefault()
    {
        PlayNewClip(defaultClip);
    }

    private Coroutine currentCoroutine;
    private IEnumerator ChangeMusicCoroutine(AudioClip clip)
    {
        while (source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }

        source.Stop();
        source.clip = clip;
        source.Play();

        while (source.volume < 1)
        {
            source.volume += Time.deltaTime;
            yield return null;
        }

        source.volume = 1;
    }
}

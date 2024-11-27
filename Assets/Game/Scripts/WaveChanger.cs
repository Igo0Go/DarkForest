using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveChanger : MonoBehaviour
{
    [SerializeField, TextArea]
    private string WaveName;
    [SerializeField]
    private bool activateOnAwake;
    [SerializeField]
    private List<Spawner> spawners;
    [SerializeField]
    private UnityEvent endOfWaveEvent;
    [SerializeField]
    private bool useMusicClip;
    [SerializeField]
    private AudioClip musicClip;
    [SerializeField]
    private bool showEndWavePanel = false;
    [SerializeField]
    private GameObject endWavePanel;

    private void Start()
    {
        endWavePanel.SetActive(false);
        if (activateOnAwake)
        {
            Activate();
        }
    }

    public void Activate()
    {
        GameCenter.waveNameText.text = WaveName;

        foreach (Spawner spawner in spawners)
        {
            spawner.spawnerDestroyed.AddListener(OnSpawnerDestroyed);
            spawner.gameObject.SetActive(true);
        }

        if(useMusicClip)
        {
            GameCenter.musicSource.Stop();
            GameCenter.musicSource.clip = musicClip;
            GameCenter.musicSource.loop = true;
            GameCenter.musicSource.Play();
        }
    }

    private void OnSpawnerDestroyed(Spawner spawner)
    {
        spawners.Remove(spawner);

        Debug.Log("Спавнеров осталось: " + spawners.Count);

        if(spawners.Count <= 0 )
        {
            StartCoroutine(EndOfWaveCoroutine());
        }
    }

    private IEnumerator EndOfWaveCoroutine()
    {
        if(showEndWavePanel)
        {
            endWavePanel.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        endWavePanel.SetActive(false);
        endOfWaveEvent.Invoke();
        Destroy(gameObject);
    }
}

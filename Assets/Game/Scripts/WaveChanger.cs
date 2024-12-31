using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveChanger : MonoBehaviour
{
    [SerializeField, TextArea]
    private string WaveName;
    [SerializeField]
    private List<Spawner> spawners;
    [SerializeField]
    private UnityEvent endOfWaveEvent;
    [SerializeField]
    private bool showEndWavePanel = false;
    [SerializeField]
    private GameObject endWavePanel;
    [SerializeField]
    private Transform arenaCenter;
    [SerializeField]
    private float ArenaRadius;

    private void Start()
    {
        endWavePanel.SetActive(false);
    }

    public void Activate()
    {
        GameCenter.waveNameText.text = WaveName;

        foreach (Spawner spawner in spawners)
        {
            spawner.arenaCenter = arenaCenter;
            spawner.arenaRadius = ArenaRadius;
            spawner.spawnerDestroyed.AddListener(OnSpawnerDestroyed);
            spawner.gameObject.SetActive(true);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ArenaRadius);
    }
}

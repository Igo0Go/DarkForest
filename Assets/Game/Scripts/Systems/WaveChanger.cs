using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveChanger : MonoBehaviour
{
    [SerializeField]
    private List<Spawner> spawners;
    [SerializeField]
    private UnityEvent endOfWaveEvent;
    [SerializeField]
    private Transform arenaCenter;
    [SerializeField]
    private float ArenaRadius;

    public void Activate()
    {
        GameCenter.CurrentArenaCenter = arenaCenter;

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

        if(spawners.Count <= 0 )
        {
            endOfWaveEvent.Invoke();
            GameCenter.CurrentRageValue = 0;
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ArenaRadius);
    }
}

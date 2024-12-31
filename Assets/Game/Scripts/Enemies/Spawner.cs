using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<EnemyWave> waves = new List<EnemyWave>();

    private List<Enemy> currentEnemies = new List<Enemy>();

    public UnityEvent<Spawner> spawnerDestroyed;

    [HideInInspector]
    public Transform arenaCenter;
    [HideInInspector]
    public float arenaRadius;

    public void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (waves.Count > 0)
        {
            EnemyWave currentWave = waves[0];

            while (currentWave.enemies.Count > 0)
            {
                for (int i = 0; i < currentWave.enemies.Count; i++)
                {
                    EnemyWaveItem item = currentWave.enemies[i];
                    Enemy enemy = Instantiate(item.enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();

                    if(enemy is INeedArenaInfo need)
                    {
                        need.ArenaCenter = arenaCenter;
                        need.ArenaRadius = arenaRadius;
                    }

                    enemy.transform.parent = transform.parent;
                    enemy.deadEvent.AddListener(OnEnemyDead);
                    currentEnemies.Add(enemy);
                    item.count--;
                    if (item.count == 0)
                    {
                        currentWave.enemies.RemoveAt(i);
                        i--;
                    }
                    yield return new WaitForSeconds(currentWave.delay);
                }
            }

            waves.Remove(currentWave);

            while(currentEnemies.Count > 0)
            {
                yield return new WaitForSeconds(currentWave.delay);
            }
        }

        spawnerDestroyed.Invoke(this);
        Destroy(gameObject);
    }

    private void OnEnemyDead(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
    }
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemyWaveItem> enemies;
    public int delay;
}

[System.Serializable]
public class EnemyWaveItem
{
    public GameObject enemyPrefab;
    public int count;
}

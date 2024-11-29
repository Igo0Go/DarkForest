using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparksSpell : MagicSpell
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private int DPS = 1;
    [SerializeField, Min(0.1f)]
    private float sparksDistance = 1;
    [SerializeField, Min(0.1f)]
    private float sparksRadius = 1;
    [SerializeField]
    private GameObject sparksLine;
    [SerializeField]
    private Transform endSparksPoint;
    [SerializeField]
    private GameObject sparksLinePrefab;

    private float currenSparksDamage = 0;
    private Enemy mainEnemy;
    private List<EnemySparksLinePair> enemiesAndLasers = new List<EnemySparksLinePair>();

    public override void UseAltSpell()
    {
        
    }

    public override void UseMainSpel()
    {
        if(Input.GetMouseButtonDown(0))
        {
            hands.SetBool("UseTwo", true);
            ActivateSparks();
        }
        else if (Input.GetMouseButton(0))
        {
            DamageInSparks();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            hands.SetBool("UseTwo", false);
            StopSparks();
        }
    }

    public override void UseGrandSpell()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            throw new System.NotImplementedException();
        }
    }

    private void ActivateSparks()
    {
        sparksLine.SetActive(true);

        if (Physics.SphereCast(spellCamera.cam.transform.position, sparksRadius, spellCamera.cam.transform.forward,
            out RaycastHit hitInfo, sparksRadius,~spellCamera.ignoreMask))
        {

            endSparksPoint.position = hitInfo.point;
            endSparksPoint.parent = null;
        }

        Ray ray = new Ray(spellCamera.cam.transform.position, spellCamera.cam.transform.forward);

        if (Physics.Raycast(ray, out hitInfo, sparksDistance, ~spellCamera.ignoreMask))
        {

            endSparksPoint.position = hitInfo.point;
            endSparksPoint.parent = null;
        }
        else
        {
            endSparksPoint.position = spellCamera.cam.transform.position + spellCamera.cam.transform.forward * sparksDistance;
            return;
        }

        if (hitInfo.collider.TryGetComponent(out mainEnemy))
        {
            mainEnemy.deadEvent.AddListener(OnMainEnemyDead);
            currenSparksDamage = 0;
            endSparksPoint.parent = mainEnemy.transform;

            Collider[] colliders = Physics.OverlapSphere(endSparksPoint.position, sparksRadius, ~spellCamera.ignoreMask);

            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    if (collider.TryGetComponent(out Enemy newEnemy))
                    {
                        if (newEnemy != mainEnemy && !ContainsEnemy(newEnemy))
                        {
                            LaserRenderer line = Instantiate(sparksLinePrefab, null).GetComponent<LaserRenderer>();

                            line.controlPoints[0].parent = endSparksPoint;
                            line.controlPoints[0].transform.position = endSparksPoint.position;
                            line.controlPoints[1].parent = newEnemy.transform;
                            line.controlPoints[1].transform.position = newEnemy.transform.position + Vector3.up * 0.5f;
                            enemiesAndLasers.Add(new EnemySparksLinePair() { enemy = newEnemy, sparksLine = line });
                            newEnemy.deadEvent.AddListener(OnEnemyDead);
                        }
                    }
                }
            }
        }
        return;
    }

    private void DamageInSparks()
    {
        currenSparksDamage += Time.deltaTime * DPS;
        if (currenSparksDamage > 1)
        {
            mainEnemy?.GetDamage(1);

            for (int i = 0; i < enemiesAndLasers.Count; i++)
            {
                if (enemiesAndLasers[i].enemy != null)
                {
                    enemiesAndLasers[i].enemy.GetDamage(1);
                }
            }
            currenSparksDamage -= 1;
        }
    }

    private void StopSparks()
    {
        endSparksPoint.parent = transform;
        endSparksPoint.position = spawnPoint.position;
        sparksLine.SetActive(false);
        ClearAllEnemies();
    }

    private void OnMainEnemyDead(Enemy enemy)
    {
        StopSparks();
    }
    private void OnEnemyDead(Enemy enemy)
    {
        for (int i = 0; i < enemiesAndLasers.Count; i++)
        {
            EnemySparksLinePair pair = enemiesAndLasers[i];
            if (pair.enemy == enemy)
            {
                pair.ClearSparksLine();
                Destroy(pair.sparksLine.gameObject);
                enemiesAndLasers.RemoveAll(x => x.enemy == enemy);
                break;
            }
        }
        enemy.deadEvent.RemoveListener(OnEnemyDead);
    }

    private void ClearAllSparksLine()
    {
        for (int i = 0; i < enemiesAndLasers.Count; i++)
        {
            EnemySparksLinePair pair = enemiesAndLasers[i];
            pair.ClearSparksLine();
            Destroy(pair.sparksLine.gameObject);
            pair.enemy.deadEvent.RemoveListener(OnEnemyDead);
        }
    }

    private void ClearAllEnemies()
    {
        mainEnemy = null;
        ClearAllSparksLine();
        enemiesAndLasers.Clear();
    }

    private bool ContainsEnemy(Enemy newEnemy)
    {
        foreach (EnemySparksLinePair pair in enemiesAndLasers)
        {
            if (newEnemy == pair.enemy)
            {
                return true;
            }
        }
        return false;
    }
}

public class EnemySparksLinePair
{
    public Enemy enemy;
    public LaserRenderer sparksLine;

    public void ClearSparksLine()
    {
        sparksLine.controlPoints[1].parent = sparksLine.transform;
        sparksLine.controlPoints[0].parent = sparksLine.transform;
    }
}

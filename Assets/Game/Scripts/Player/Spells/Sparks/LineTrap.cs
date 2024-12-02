using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LineTrap : MonoBehaviour
{
    public Transform endPoint;

    public UnityEvent<int> damageEvent;

    private int damage;
    private LayerMask ignoreMask;
    private float lifetime;

    private List<Enemy> enemyList = new List<Enemy>();

    public void InitTrap(int damage, LayerMask layerMask, float lifetime)
    {
        this.damage = damage;
        ignoreMask = layerMask;
        this.lifetime = lifetime;

        StartCoroutine(CheckLineCoroutine());
    }
    IEnumerator CheckLineCoroutine()
    {
        float currentLifeTime = 0;
        while (currentLifeTime <= lifetime)
        {
            currentLifeTime += Time.deltaTime;
            if (Physics.Linecast(transform.position, endPoint.position, out RaycastHit hit, ~ignoreMask))
            {
                if (hit.collider.TryGetComponent(out Enemy enemy))
                {
                    if (!enemyList.Contains(enemy))
                    {
                        enemyList.Add(enemy);
                        enemy.GetDamage(damage);
                        damageEvent.Invoke(damage);
                    }
                }
            }

            yield return null;
        }

        endPoint.transform.parent = transform;
        Destroy(gameObject);
    }
}

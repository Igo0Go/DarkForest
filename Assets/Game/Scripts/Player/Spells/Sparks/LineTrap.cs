using System.Collections;
using UnityEngine;

public class LineTrap : MonoBehaviour
{
    public Transform endPoint;

    private int damage;
    private LayerMask ignoreMask;
    private float lifetime;

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
        float t = 1;
        while (currentLifeTime <= lifetime)
        {
            currentLifeTime += Time.deltaTime;
            if (Physics.Linecast(transform.position, endPoint.position, out RaycastHit hit, ~ignoreMask))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent(out Enemy enemy))
                {
                    if(t == 1)
                    {
                        enemy.GetDamage(damage);
                        t = 0;
                    }
                    else
                    {
                        t += Time.deltaTime;
                    }
                }
            }
            else
            {
                t = 1;
            }

            yield return null;
        }

        endPoint.transform.parent = transform;
        Destroy(gameObject);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TargetListMagicBullet : MagicBullet
{
    [HideInInspector]
    public List<Enemy> enemies = new List<Enemy>();

    private Transform currentTarget;

    protected override void MoveBullet()
    {
        if(currentTarget != null)
        {
            Vector3 direction = currentTarget.position - myTransform.position;
            myTransform.forward = direction.normalized;
            myTransform.position += direction.normalized * bulletSpeed * Time.deltaTime;
            CheckHit();
            oldPos = myTransform.position;
        }
    }

    protected override void CheckHit()
    {
        if (Physics.Linecast(oldPos, myTransform.position, out RaycastHit hit, ~ignoreMask))
        {
            if (useToPlayer && hit.collider.TryGetComponent(out PlayerInteraction player))
            {
                player.GetDamage(damage);
            }
            else if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.GetDamage(damage);
            }

            for (int i = 0; i < decals.Count; i++)
            {
                Instantiate(decals[i], hit.point + hit.normal * 0.1f, Quaternion.identity, hit.transform).
                    transform.forward = hit.normal;
            }


            enemies.RemoveAt(0);

            if (enemies.Count > 0)
            {
                currentTarget = enemies[0].transform;
            }
            else
            {
                bulletDestroyedEvent.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}

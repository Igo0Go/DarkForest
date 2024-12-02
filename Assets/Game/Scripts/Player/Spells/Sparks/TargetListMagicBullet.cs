using System.Collections.Generic;
using UnityEngine;

public class TargetListMagicBullet : MagicBullet
{
    private List<Enemy> enemies = new List<Enemy>();
    private Transform currentTarget;

    public void SetEnemyList(List<Enemy> enemies)
    {
        this.enemies = enemies;
        currentTarget = enemies[0].transform;
    }

    Vector3 direction;

    protected override void MoveBullet()
    {
        if(currentTarget != null)
        {
            Vector3 direction = currentTarget.position + transform.up - myTransform.position;
            myTransform.forward = direction.normalized;
            myTransform.position += direction.normalized * bulletSpeed * Time.deltaTime;
            CheckHit();
            oldPos = myTransform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void CheckHit()
    {
        if (direction.magnitude < bulletSpeed*Time.deltaTime*2)
        {
            Enemy enemy = enemies[0];
            enemy.GetDamage(damage);

            for (int i = 0; i < decals.Count; i++)
            {
                Instantiate(decals[i], enemy.transform.position + Vector3.up, Quaternion.identity, enemy.transform);
            }
            enemies.RemoveAt(0);

            while (enemies.Count > 0 && enemies[0] == null)
            {
                enemies.RemoveAt(0);
            }

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

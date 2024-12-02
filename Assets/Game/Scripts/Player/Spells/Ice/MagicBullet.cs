using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicBullet : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> decals = null;

    [SerializeField]
    protected LayerMask ignoreMask;

    [HideInInspector]
    public UnityEvent<int> DamageEvent;

    protected float bulletSpeed;
    private float bulletLifeTime;
    protected int damage;

    protected Vector3 oldPos;
    protected Transform myTransform;
    private float counter = 0;

    protected bool useToPlayer = false;

    [HideInInspector]
    public UnityEvent<MagicBullet> bulletDestroyedEvent;

    private void Awake()
    {
        myTransform = transform;
    }

    /// <summary>
    /// Запустить снаряд
    /// </summary>
    /// <param name="speed">Скорость снаряда</param>
    /// <param name="lifeTime">время жизни снаряда</param>
    /// <param name="lifeTime">урон от снаряда</param>
    public void LaunchBullet(float speed, float lifeTime, int damage, bool toPlayer)
    {
        bulletSpeed = speed;
        this.damage = damage;
        oldPos = transform.position;
        bulletLifeTime = lifeTime;
        useToPlayer = toPlayer;
    }

    private void Update()
    {
        MoveBullet();
    }

    protected virtual void MoveBullet()
    {
        if (bulletLifeTime > 0)
        {
            myTransform.position += myTransform.forward * bulletSpeed * Time.deltaTime;
            CheckHit();
            oldPos = myTransform.position;
            counter += Time.deltaTime;
            if (counter >= bulletLifeTime)
            {
                for (int i = 0; i < decals.Count; i++)
                {
                    Instantiate(decals[i], transform.position, Quaternion.identity);
                }
                bulletDestroyedEvent.Invoke(this);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void CheckHit()
    {
        if (Physics.Linecast(oldPos, myTransform.position, out RaycastHit hit, ~ignoreMask))
        {
            if(useToPlayer && hit.collider.TryGetComponent(out PlayerInteraction player))
            {
                player.GetDamage(damage);
            }
            else if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.GetDamage(damage);
                DamageEvent?.Invoke(damage);
            }

            for (int i = 0; i < decals.Count; i++)
            {
                Instantiate(decals[i], hit.point + hit.normal * 0.1f, Quaternion.identity, hit.transform).
                    transform.forward = hit.normal;
            }
            bulletDestroyedEvent.Invoke(this);
            Destroy(gameObject);
        }
    }
}

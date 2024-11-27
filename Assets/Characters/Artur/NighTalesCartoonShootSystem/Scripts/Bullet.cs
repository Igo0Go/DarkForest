using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> decals = null;

    [SerializeField]
    private LayerMask ignoreMask;

    private bool connectDecals;

    private float bulletSpeed;
    private float bulletLiveTime;

    private Vector3 oldPos;
    private Transform myTransform;
    private float counter = 0;

    private void Awake()
    {
        myTransform = transform;
    }

    /// <summary>
    /// Запустить снаряд
    /// </summary>
    /// <param name="speed">Скорость снаряда</param>
    /// <param name="liveTime">время жизни снаряда</param>
    public void LaunchBullet(float speed, float liveTime, bool connectDecal)
    {
        connectDecals = connectDecal;
        bulletLiveTime = liveTime;
        bulletSpeed = speed;
        oldPos = myTransform.position;
    }

    void Update()
    {
        myTransform.position += myTransform.forward * bulletSpeed * Time.deltaTime;
        CheckHit();
        oldPos = myTransform.position;
        counter += Time.deltaTime;
        if(counter >= bulletLiveTime)
        {
            Destroy(gameObject);
        }
    }


    private void CheckHit()
    {
        if(Physics.Linecast(oldPos, myTransform.position, out RaycastHit hit, ~ignoreMask))
        {
            var par = hit.transform.parent;
            if (par != null && par.gameObject.TryGetComponent(out EnergyShield energyShield))
            {
                energyShield.OnDamage();
                return;
            }

            for (int i = 0; i < decals.Count; i++)
            {
                if(connectDecals)
                {
                    Instantiate(decals[i], hit.point + hit.normal * 0.1f, Quaternion.identity, hit.transform).
                        transform.forward = hit.normal;
                }
                else
                {
                    Instantiate(decals[i], hit.point + hit.normal * 0.1f, Quaternion.identity).transform.forward = hit.normal;
                }
            }
            Destroy(gameObject);
        }
    }
}

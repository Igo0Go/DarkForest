using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField, Min(0)]
    private float startHeight = 10;
    [SerializeField, Range(0.1f, 1f)]
    private float targetTrackingForce;
    [SerializeField, Min(0.1f)]
    private float attackDistance = 1;

    [SerializeField]
    private AudioClip shotClip;
    [SerializeField]
    private AudioClip spawnClip;
    [SerializeField]
    private GameObject deadDecal;

    protected override IEnumerator MainCoroutine()
    {
        Vector3 target = transform.position + Vector3.up * startHeight;

        Vector3 direction = target - transform.position;

        while(transform.position.y < target.y)
        {
            transform.forward = direction;
            transform.position += direction.normalized * speed * Time.deltaTime;

            yield return null;
        }

        if(spawnClip != null)
        {
            GameCenter.mainSource.PlayOneShot(spawnClip);
        }

        while (direction.magnitude > attackDistance)
        {

            target = player.transform.position;
            direction = target - transform.position;

            transform.forward = Vector3.Lerp(transform.forward, direction, targetTrackingForce);
            transform.position += transform.forward * speed * Time.deltaTime;

            yield return null;
        }

        GameCenter.mainSource.PlayOneShot(shotClip);

        player.GetDamage(damage);
        Dead();
    }

    protected override void Dead()
    {
        Instantiate(deadDecal, transform.position, Quaternion.identity);
        base.Dead();
    }
}

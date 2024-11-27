using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ShootModule : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet = null;

    [SerializeField]
    private GameObject muzzleFlash = null;

    [SerializeField]
    private AudioClip shootClip = null;

    [SerializeField]
    private Transform startShootPoint = null;

    [SerializeField]
    private List<Transform> targetShootPoints = null;

    [SerializeField]
    private int currentActiveTargetIndex = 0;

    [SerializeField]
    [Range(1,100)]
    private float bulletSpeed = 1;

    [SerializeField]
    [Range(0, 10)]
    private float bulletLifetime = 1;

    [SerializeField]
    [Range(0, 10)]
    private float shootPause = 1;

    [SerializeField]
    private bool debug = false;

    [SerializeField]
    private bool connectDecals = true;

    private AudioSource shootSource;

    private void Awake()
    {
        shootSource = GetComponent<AudioSource>();
    }

    public void ShootOnce()
    {
        muzzleFlash.SetActive(true);
        StartCoroutine(DisactiveMuzzleFlashCoroutine(shootPause));
        shootSource.PlayOneShot(shootClip);
        InstanceBullet();
    }

    public void StartShoot()
    {
        StopShoot();
        StartCoroutine(LoopShootCoroutine());
    }

    public void StopShoot()
    {
        StopAllCoroutines();
        muzzleFlash.SetActive(false);
    }

    private IEnumerator LoopShootCoroutine()
    {
        while(true)
        {
            ShootOnce();
            yield return new WaitForSeconds(shootPause + 0.2f);
        }
    }

    private void InstanceBullet()
    {
        var bul = Instantiate(bullet, startShootPoint.position, Quaternion.identity);
        bul.transform.forward = targetShootPoints[currentActiveTargetIndex].position - startShootPoint.position;
        bul.GetComponent<Bullet>().LaunchBullet(bulletSpeed, bulletLifetime, connectDecals);
        shootSource.PlayOneShot(shootClip);
    }

    private IEnumerator DisactiveMuzzleFlashCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        muzzleFlash.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(debug && currentActiveTargetIndex >= 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startShootPoint.position, targetShootPoints[currentActiveTargetIndex].position);
        }
    }
#endif
}

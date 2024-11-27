using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class IceSpell : MagicSpell
{
    [SerializeField]
    private GameObject iceBulletPrefab;
    [SerializeField, Min(1)]
    private int damage;
    [SerializeField, Min(1)]
    private float bulletSpeed;
    [SerializeField]
    private float bulletLiveTime;
    [SerializeField]
    private GameObject altSplash;
    [SerializeField, Range(0, 2)]
    private float spellDelay;
    

    [SerializeField]
    private Transform spawnPoint;


    private bool useSpell;

    private void Awake()
    {
        useSpell = false;
        altSplash.SetActive(false);

    }

    private void Start()
    {

    }

    public override void UseAltSpell()
    {
        if(Input.GetMouseButton(1) && !useSpell)
        {
            StartCoroutine(UseSplashCoroutine());
        }
    }

    public override void UseMainSpel()
    {
        if (Input.GetMouseButton(0) && !useSpell)
        {
            StartCoroutine(SpawnBulletCoroutine());
        }
    }

    private IEnumerator SpawnBulletCoroutine()
    {
        useSpell = true;
        MagicBullet currentBullet = Instantiate(iceBulletPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<MagicBullet>();
        currentBullet.transform.parent = null;
        currentBullet.transform.forward = spellCamera.GetSpellTargetPoint() - transform.position;
        currentBullet.LaunchBullet(bulletSpeed, bulletLiveTime, damage, false);

        hands.SetBool("UseLeft", !hands.GetBool("UseLeft"));
        hands.SetTrigger("Push");
        
        yield return new WaitForSeconds(spellDelay);
        useSpell = false;
    }

    private IEnumerator UseSplashCoroutine()
    {
        useSpell = true;
        altSplash.SetActive(true);
        hands.SetTrigger("Splash");
        yield return new WaitForSeconds(spellDelay);
        altSplash.SetActive(false);
        useSpell = false;
    }
}

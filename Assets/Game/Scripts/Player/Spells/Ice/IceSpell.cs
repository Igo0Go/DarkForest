using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell : MagicSpell
{
    [Header("Ледяной разброс")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField, Min(1)]
    private int damage;
    [SerializeField, Range(1,45)]
    private float angle;
    [SerializeField, Min(0.01f)]
    private float spawnDelay;
    [SerializeField, Min(0.01f)]
    private float bulletSpeed;
    [SerializeField, Min(0.01f)]
    private float bulletLifetime;
    [SerializeField, Min(3)]
    private int bulletsPierceCount = 3;

    private List<MagicBullet> bulletList = new List<MagicBullet>();
    private Coroutine spawnCoroutine;
    private const int minimalBulletsCount = 3;

    [Header("Морозная руна")]
    [SerializeField]
    private GameObject freezeRune;
    [SerializeField, Min(1)]
    private float runeLifeTime = 5;
    [SerializeField, Min(1)]
    private int runeDamage = 5;
    [SerializeField, Min(1)]
    private int runeScale = 1;
    [SerializeField]
    private int requredConcentrationCount = 5;
    [SerializeField, Min(1)]
    private int grandDamage;

    [Header("Морозный круг")]
    [SerializeField, Range(6, 36)]
    private int grandSpellBulletsCount = 6;
    [SerializeField, Min(0.01f)]
    private float grandSpellDelay;
    [SerializeField]
    private Transform helper;


    private bool useSpell;

    public override void InitSpell()
    {
        AnimatorEventObserver observer = hands.gameObject.GetComponent<AnimatorEventObserver>();

        observer.OneHandPushEnd += OnEndPush;
        observer.RingSpellActivated += OnEndRing;

        SetUpSpell();
    }

    public override void SetUpSpell()
    {
        useSpell = false;
        LaunchBullets(damage);
    }

    public override void UseMainSpel()
    {
        if (Input.GetMouseButtonDown(0) && !useSpell)
        {
            useSpell = true;
            spawnCoroutine = StartCoroutine(PrepareMainSpellCoroutine());
            hands.SetBool("PrepareRight", useSpell);
        }
        else if (Input.GetMouseButtonUp(0) && useSpell)
        {
            hands.SetBool("UseLeft", false);
            hands.SetBool("PrepareRight", false);
            hands.SetTrigger("Push");
            if(spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);
            LaunchBullets(damage);
        }
    }
    private IEnumerator PrepareMainSpellCoroutine()
    {
        for (int i = 0; i < minimalBulletsCount; i++)
        {
            PiercingBullet currentBullet = Instantiate(bullet, spawnPoint.position,
    spawnPoint.rotation * GetRandomDirection(), spawnPoint).GetComponent<PiercingBullet>();

            currentBullet.pierceCount = bulletsPierceCount;
            bulletList.Add(currentBullet);

            currentBullet.DamageEvent.AddListener(OnEnemyGetDamage);
        }

        while (bulletList.Count < MagicStats.IceMaxBulletsCount)
        {
            MagicBullet currentBullet = Instantiate(bullet, spawnPoint.position,
                spawnPoint.rotation * GetRandomDirection(), spawnPoint).GetComponent<MagicBullet>();

            bulletList.Add(currentBullet);

            currentBullet.DamageEvent.AddListener(OnEnemyGetDamage);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    private void OnEnemyGetDamage(int damage)
    {
        GrandSpellValue += damage * GameCenter.CurrentRageMultiplicator;
        GameCenter.CurrentRageValue++;
    }
    private Quaternion GetRandomDirection()
    {
        return Quaternion.Euler(Random.Range(-angle, angle), Random.Range(-angle, angle), 0);
    }

    private void OnEndPush()
    {
        useSpell = false;
    }

    public override void UseAltSpell()
    {
        if(Input.GetMouseButtonDown(1) && !useSpell && GrandSpellValue >= requredConcentrationCount)
        {
            useSpell = true;
            GrandSpellValue -= requredConcentrationCount;
            hands.SetTrigger("UseRing");
        }
    }
    private void OnEndRing()
    {
        Vector3 normal = Vector3.up;
        Vector3 pos = spellCamera.GetSpellTargetPoint(out normal);
        GameObject rune = Instantiate(freezeRune, pos, Quaternion.identity);
        rune.transform.forward = normal;
        rune.transform.localScale = Vector3.one * runeScale;
        rune.GetComponent<ExplosionRune>().damage = runeDamage;
        rune.GetComponent<Decal>().lifeTime = runeLifeTime;
        useSpell = false;
    }

    public override void UseGrandSpell()
    {
        if(Input.GetKeyDown(KeyCode.E) && GrandSpellValue >= GrandSpellRate)
        {
            hands.SetTrigger("UseGrand");
            GrandSpellValue = 0;
            StartCoroutine(LaucnGrandSpell());
        }
    }

    private void LaunchBullets(int damage)
    {
        foreach (MagicBullet bullet in bulletList)
        {
            bullet.transform.parent = null;

            bullet.LaunchBullet(bulletSpeed, bulletLifetime, damage * GameCenter.CurrentRageMultiplicator, false);
        }
        bulletList.Clear();
    }
    private IEnumerator LaucnGrandSpell()
    {
        float GrandAngle = 360 / grandSpellBulletsCount;

        bulletList.Clear();

        for (int i = 0; i < grandSpellBulletsCount; i++)
        {
            MagicBullet current_bullet = Instantiate(bullet).GetComponent<MagicBullet>();
            current_bullet.transform.forward = helper.forward;
            current_bullet.transform.Rotate(Vector3.up, GrandAngle * i);
            current_bullet.transform.position = helper.position + current_bullet.transform.forward * 0.5f;
            bulletList.Add(current_bullet);
            yield return null;
        }

        yield return new WaitForSeconds(grandSpellDelay);

        LaunchBullets(grandDamage);
    }
}

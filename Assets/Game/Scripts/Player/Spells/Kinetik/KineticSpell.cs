using System.Collections;
using UnityEngine;

public class KineticSpell : MagicSpell
{
    [Header("Кинетический разряд")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField, Min(1)]
    private int damage;
    [SerializeField, Min(1)]
    private float bulletSpeed;
    [SerializeField]
    private float bulletLiveTime;
    [SerializeField]
    private Transform spawnPoint;

    private bool useSpell;


    [Space(20)]
    [Header("Разрез")]
    [SerializeField]
    private GameObject altSplash;


    [Space(20)]
    [Header("Кинетический поток")]
    [SerializeField]
    private SpellRune rune;
    [SerializeField, Range(1, 20)]
    private float grandSpellValueLostSpeed = 5;
    [SerializeField, Range(0.01f, 2)]
    private float grandSpellShootDelay = 5;
    private bool useGrand;


    public override void InitSpell()
    {
        AnimatorEventObserver observer = hands.gameObject.GetComponent<AnimatorEventObserver>();

        observer.OneHandPushEnd += OnEndSpell;
        observer.SplashEnd += OnEndSplash;

        SetUpSpell();
    }

    private void OnEndSplash()
    {
        OnEndSpell();
        altSplash.SetActive(false);
    }

    private void OnEndSpell()
    {
        useSpell = false;
    }

    public override void SetUpSpell()
    {
        useGrand = false;
        useSpell = false;
        GrandSpellValue = GrandSpellValue;//эвент в интерфейс
        altSplash.SetActive(false);
        StopAllCoroutines();
        rune.Stop();
    }

    public override void UseMainSpel()
    {
        if (useGrand)
            return;


        if (Input.GetMouseButton(0) && !useSpell)
        {
            hands.SetFloat("AnimationSpeed", MagicStats.IceMainSpeedMultiplicator);
            useSpell = true;
            MagicBullet currentBullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation).
                GetComponent<MagicBullet>();
            currentBullet.transform.parent = null;
            currentBullet.transform.forward = spellCamera.GetSpellTargetPoint() - transform.position;
            currentBullet.DamageEvent.AddListener(OnEnemyGetDamage);
            currentBullet.LaunchBullet(bulletSpeed * MagicStats.IceMainSpeedMultiplicator, bulletLiveTime,
                damage * GameCenter.CurrentRageMultiplicator, false);
            hands.SetBool("UseLeft", !hands.GetBool("UseLeft"));
            hands.SetTrigger("Push");
        }
    }

    public override void UseAltSpell()
    {
        if (useGrand)
            return;

        if (Input.GetMouseButton(1) && !useSpell)
        {
            useSpell = true;
            altSplash.SetActive(true);
            hands.SetTrigger("Splash");
        }
    }

    public override void UseGrandSpell()
    {
        if (useGrand)
            return;

        if (Input.GetKeyDown(KeyCode.E) && GrandSpellValue >= GrandSpellRate)
        {
            useGrand = true;
            hands.SetBool("UseTwo", true);

            rune.damage = damage * 5 * GameCenter.CurrentRageMultiplicator;
            rune.spellLifeTime = bulletLiveTime;
            rune.spellSpeed = bulletSpeed;
            rune.shootDelay = grandSpellShootDelay;
            rune.bullet = bulletPrefab;

            rune.Activate();
            StartCoroutine(GrandSpellCoroutine());
        }
    }

    private IEnumerator GrandSpellCoroutine()
    {
        while(GrandSpellValue > 0)
        {
            GrandSpellValue -= Time.deltaTime * grandSpellValueLostSpeed;
            yield return null;
        }

        GrandSpellValue = 0;
        rune.Stop();
        hands.SetBool("UseTwo", false);
        useGrand = false;
    }

    private void OnEnemyGetDamage(int damage)
    {
        GrandSpellValue += damage;
    }
}

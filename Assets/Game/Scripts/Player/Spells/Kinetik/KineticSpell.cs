using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private Splash altSplash;


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
        observer.SplashAttackPhase += OnSplashActivePhase;
        observer.SplashEnd += OnEndSplash;
        observer.PrepareNewAttack += OnEndSpell;

        SetUpSpell();
    }



    public override void SetUpSpell()
    {
        base.SetUpSpell();
        useGrand = false;
        useSpell = false;
        GrandSpellValue = GrandSpellValue;//эвент в интерфейс
        altSplash.Active = false;
        StopAllCoroutines();
        rune.Stop();
    }

    public override void UseMainSpel()
    {
        if (useGrand)
            return;


        if (Input.GetMouseButton(0) && !useSpell && !EventSystem.current.IsPointerOverGameObject())
        {
            hands.SetFloat("AnimationSpeed", MagicStats.KineticMainSpeedMultiplicator);
            useSpell = true;
            MagicBullet currentBullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation).
                GetComponent<MagicBullet>();
            currentBullet.transform.parent = null;
            currentBullet.transform.forward = spellCamera.GetSpellTargetPoint() - transform.position;
            currentBullet.DamageEvent.AddListener(OnEnemyGetDamage);
            currentBullet.LaunchBullet(bulletSpeed * MagicStats.KineticMainSpeedMultiplicator, bulletLiveTime,
                damage * GameCenter.CurrentRageMultiplicator, false);
            hands.SetBool("UseLeft", !hands.GetBool("UseLeft"));
            hands.SetTrigger("Push");
        }
    }

    public override void UseAltSpell()
    {
        if (useGrand)
            return;

        if (Input.GetMouseButton(1) && !useSpell && !GameCenter.pause)
        {
            useSpell = true;
            altSplash.Active = true;
            hands.SetTrigger("Splash");
        }
    }

    public override void UseGrandSpell()
    {
        if (useGrand)
            return;

        if (Input.GetKeyDown(KeyCode.E) && !GameCenter.pause && GrandSpellValue >= GrandSpellRate)
        {
            useGrand = true;
            hands.SetBool("UseTwo", true);

            rune.damage = damage * GameCenter.CurrentRageMultiplicator;
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
        useSpell = false;
    }

    private void OnEnemyGetDamage(int damage)
    {
        GrandSpellValue += damage * GameCenter.CurrentRageMultiplicator;
        GameCenter.CurrentRageValue++;
    }

    private void OnSplashActivePhase()
    {
        altSplash.Attack();
    }

    private void OnEndSplash()
    {
        altSplash.Active = false;
    }

    private void OnEndSpell()
    {
        useSpell = false;
    }
}

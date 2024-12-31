using UnityEngine;

public class KineticSpell : MagicSpell
{
    [Header("Ледяная стрела")]
    [SerializeField]
    private GameObject iceBulletPrefab;
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
    [Header("Ледяной нож")]
    [SerializeField]
    private GameObject altSplash;


    [Space(20)]
    [Header("Ледяной дождь")]
    [SerializeField]
    private GameObject spellRune;
    [SerializeField, Min(4)]
    private float runeHeight = 4;
    [SerializeField, Min(1)]
    private float runeTime = 1;

    private void Start()
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
        useSpell = false;
        GrandSpellValue = GrandSpellValue;
        altSplash.SetActive(false);
    }

    public override void UseMainSpel()
    {
        if (Input.GetMouseButton(0) && !useSpell)
        {
            hands.SetFloat("AnimationSpeed", MagicStats.IceMainSpeedMultiplicator);
            useSpell = true;
            MagicBullet currentBullet = Instantiate(iceBulletPrefab, spawnPoint.position, spawnPoint.rotation).
                GetComponent<MagicBullet>();
            currentBullet.transform.parent = null;
            currentBullet.transform.forward = spellCamera.GetSpellTargetPoint() - transform.position;
            currentBullet.DamageEvent.AddListener(OnEnemyGetDamage);
            currentBullet.LaunchBullet(bulletSpeed * MagicStats.IceMainSpeedMultiplicator, bulletLiveTime, damage, false);
            hands.SetBool("UseLeft", !hands.GetBool("UseLeft"));
            hands.SetTrigger("Push");
        }
    }

    public override void UseAltSpell()
    {
        if(Input.GetMouseButton(1) && !useSpell)
        {
            useSpell = true;
            altSplash.SetActive(true);
            hands.SetTrigger("Splash");
        }
    }

    public override void UseGrandSpell()
    {
        if (Input.GetKeyDown(KeyCode.E) && GrandSpellValue >= GrandSpellRate)
        {
            hands.SetTrigger("UseGrand");
            GrandSpellValue = 0;
            SpellRune rune = Instantiate(spellRune, Vector3.zero + Vector3.up * runeHeight,
    spawnPoint.rotation).GetComponent<SpellRune>();
            rune.transform.forward = Vector3.down;

            rune.damage = damage * 5;
            rune.spellLifeTime = bulletLiveTime;
            rune.spellSpeed = bulletSpeed;
            rune.shootDelay = Time.deltaTime;
            rune.bullet = iceBulletPrefab;
            rune.GetComponent<Decal>().lifeTime = runeTime;

            rune.Activate();
        }
    }

    private void OnEnemyGetDamage(int damage)
    {
        GrandSpellValue += damage;
    }
}

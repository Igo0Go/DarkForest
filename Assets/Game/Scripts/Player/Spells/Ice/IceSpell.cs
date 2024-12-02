using System.Collections;
using UnityEngine;

public class IceSpell : MagicSpell
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
    [SerializeField, Range(0, 2)]
    private float spellDelay;

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



    private void Awake()
    {
        SetUpSpell();
    }

    public override void SetUpSpell()
    {
        useSpell = false;
        altSplash.SetActive(false);
    }

    public override void UseMainSpel()
    {
        if (Input.GetMouseButton(0) && !useSpell)
        {
            StartCoroutine(SpawnBulletCoroutine());
        }
    }

    public override void UseAltSpell()
    {
        if(Input.GetMouseButton(1) && !useSpell)
        {
            StartCoroutine(UseSplashCoroutine());
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


    private IEnumerator SpawnBulletCoroutine()
    {
        useSpell = true;
        MagicBullet currentBullet = Instantiate(iceBulletPrefab, spawnPoint.position, spawnPoint.rotation).
            GetComponent<MagicBullet>();
        currentBullet.transform.parent = null;
        currentBullet.transform.forward = spellCamera.GetSpellTargetPoint() - transform.position;
        currentBullet.DamageEvent.AddListener(OnEnemyGetDamage);
        currentBullet.LaunchBullet(bulletSpeed, bulletLiveTime, damage, false);
        hands.SetBool("UseLeft", !hands.GetBool("UseLeft"));
        hands.SetTrigger("Push");

        yield return new WaitForSeconds(spellDelay);

        useSpell = false;
    }

    private void OnEnemyGetDamage(int damage)
    {
        GrandSpellValue += damage;
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

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

    [Space(20)]

    [Header("Ледяной нож")]
    [SerializeField]
    private GameObject altSplash;


    [Space(20)]
    [Header("Ледяной дождь")]
    [SerializeField]
    private GameObject SpellRune;
    [SerializeField, Min(4)]
    private float RuneHeight = 4;
    [SerializeField, Min(1)]
    private float RuneTime = 1;



    private bool useSpell;

    private void Awake()
    {
        useSpell = false;
        altSplash.SetActive(false);

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
        GrandSpellValue++;
        useSpell = false;
    }

    private IEnumerator UseSplashCoroutine()
    {
        GrandSpellValue += 3;
        useSpell = true;
        altSplash.SetActive(true);
        hands.SetTrigger("Splash");
        yield return new WaitForSeconds(spellDelay);
        altSplash.SetActive(false);
        useSpell = false;
    }

    public override void UseGrandSpell()
    {
        if(Input.GetKeyDown(KeyCode.E) && GrandSpellValue >= GrandSpellRate)
        {
            GrandSpellValue = 0;
            StartCoroutine(SpawnRuneCoroutine());
        }
    }
    private IEnumerator SpawnRuneCoroutine()
    {
        SpellRune rune = Instantiate(SpellRune, spellCamera.GetSpellTargetPoint() + Vector3.up * RuneHeight,
    spawnPoint.rotation).GetComponent<SpellRune>();
        rune.transform.forward = Vector3.down;

        rune.damage = damage * 5;
        rune.spellLifeTime = bulletLiveTime;
        rune.spellSpeed = bulletSpeed;
        rune.shootDelay = Time.deltaTime;
        rune.bullet = iceBulletPrefab;

        rune.Activate();

        yield return new WaitForSeconds(RuneTime);

        Destroy(rune.gameObject);
    }
}

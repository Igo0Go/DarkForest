using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireSpell : MagicSpell
{
    [Header("�������� ���")]
    [SerializeField]
    private GameObject fireBallPrefab;
    [SerializeField]
    private Transform startSpawnPoint;
    [SerializeField]
    private Transform endSpawnPoint;
    [SerializeField, Min(1)]
    private int minDamage = 1;
    [SerializeField, Min(2)]
    private int maxDamage = 50;
    [SerializeField, Min(0.01f)]
    private float fireballSpeed = 1;
    [SerializeField, Min(0.01f)]
    private float fireballLifeTime = 1;
    [SerializeField, Range(0.01f, 2)]
    private float fireballForceAccumulateSpeed = 1;

    private bool useFireBall = false;
    private FireBall currentFireBall;
    private int currentDamage = 0;

    [Space(20)]
    [Header("�������� ���")]
    [SerializeField]
    private Transform rayDecal;
    [SerializeField]
    private LaserRenderer laserRenderer;
    [SerializeField, Range(0.1f, 10f)]
    private float damagePerSecond = 1;

    private bool useRay;
    private float currentRayDamage = 0;
    private Vector3 currentRayPoint;

    [Space(20)]
    [Header("������� ��� ����")]
    [SerializeField] private GameObject explosionPrefab;

    public override void InitSpell()
    {
        AnimatorEventObserver observer = hands.gameObject.GetComponent<AnimatorEventObserver>();

        observer.GrandSpellActivated += OnGrandSpellActivated;
        SetUpSpell();
    }

    private void LateUpdate()
    {
        laserRenderer.controlPoints[1].position = currentRayPoint;
        rayDecal.position = currentRayPoint;
    }

    public override void SetUpSpell()
    {
        useFireBall = false;
        if (currentFireBall != null)
        {
            Destroy(currentFireBall.gameObject);
        }
        useRay = false;
        hands.SetBool("UseTwo", useRay);
        laserRenderer.gameObject.SetActive(false);
        rayDecal.gameObject.SetActive(false);
        GrandSpellValue = GrandSpellValue;
    }

    public override void UseMainSpel()
    {
        if (Input.GetMouseButtonDown(0) && !useFireBall)
        {
            StartCoroutine(GrowFireBall());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            useFireBall = false;
        }
    }

    private IEnumerator GrowFireBall()
    {
        useFireBall = true;
        float t = 0;

        currentFireBall = Instantiate(fireBallPrefab, startSpawnPoint.position, Quaternion.identity, startSpawnPoint).
            GetComponent<FireBall>();
        currentFireBall.DamageEvent.AddListener(OnEnemyGetDamage);

        hands.SetTrigger("UseFireBall");
        hands.SetFloat("SpellForce", t);

        while (useFireBall)
        {
            t += Time.deltaTime * fireballForceAccumulateSpeed * MagicStats.FireMainSpeedMultiplicator;
            hands.SetFloat("SpellForce", t);
            currentFireBall.transform.position = Vector3.Lerp(startSpawnPoint.position, endSpawnPoint.position, t);
            currentDamage = (int)Mathf.Round(Mathf.Lerp(minDamage, maxDamage, t));
            currentFireBall.SetFireballScale(t);
            yield return null;
        }
        hands.SetBool("UseLeft", !hands.GetBool("UseLeft"));
        hands.SetTrigger("Push");

        currentFireBall.transform.parent = null;
        currentFireBall.transform.forward = spellCamera.GetSpellTargetPoint() - currentFireBall.transform.position;
        currentFireBall.LaunchBullet(fireballSpeed, fireballLifeTime, currentDamage, false);

        useFireBall = false;
    }

    public override void UseAltSpell()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentDamage = 0;
            useRay = true;
            hands.SetBool("UseTwo", useRay);
            rayDecal.gameObject.SetActive(true);
            laserRenderer.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(1))
        {
            Ray ray = new Ray(spellCamera.cam.transform.position, spellCamera.cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, ~spellCamera.ignoreMask))
            {

                currentRayPoint = hitInfo.point;

                if (hitInfo.collider.TryGetComponent(out Enemy enemy))
                {
                    currentRayDamage += Time.deltaTime * damagePerSecond;
                    if (currentRayDamage > 1)
                    {

                        enemy.GetDamage(1);
                        GrandSpellValue += 1;
                        currentRayDamage -= 1;
                    }
                }
            }
            else
            {
                currentRayPoint = spellCamera.cam.transform.position + spellCamera.cam.transform.forward * 100;
                currentRayDamage = 0;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            useRay = false;
            hands.SetBool("UseTwo", useRay);
            laserRenderer.gameObject.SetActive(false);
            rayDecal.gameObject.SetActive(false);
        }
    }

    public override void UseGrandSpell()
    {
        if(Input.GetKeyDown(KeyCode.E) && GrandSpellValue == GrandSpellRate)
        {
            GrandSpellValue = 0;
            ChangeSwitchKey.Invoke(false);
            hands.SetTrigger("UseGrand");
        }
    }

    private IEnumerator GrandSpellDamageForAllEnemies()
    {
        List<Enemy> enemies = FindObjectsOfType<Enemy>().ToList();

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
            }
            yield return null;
        }
        ChangeSwitchKey.Invoke(true);
    }

    private void OnGrandSpellActivated()
    {
        if(gameObject.activeSelf)
        {
            StartCoroutine(GrandSpellDamageForAllEnemies());
        }
    }

    private void OnEnemyGetDamage(int damage)
    {
        GrandSpellValue += damage;
    }
}

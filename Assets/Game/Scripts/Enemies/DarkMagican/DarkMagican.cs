using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class DarkMagican : Enemy
{
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private Animator animator;
    [SerializeField, Min(0.1f)]
    private float hitDelay = 1;
    [SerializeField, Min(0.1f)]
    private float deadDelay = 5;

    [Space(20)]
    [Header("Атака заклинанием")]
    [SerializeField, Min(5)]
    private float farAttackDistance = 10;
    [SerializeField]
    private GameObject bullet;
    [SerializeField, Min(1)]
    private int spellDamage = 1;
    [SerializeField, Range(0.01f, 1)]
    private float targetTrackingForce = 0.5f;
    [SerializeField]
    private Transform spellSpawnPoint;
    [SerializeField, Min(0.1f)]
    private float spellSpeed = 1;
    [SerializeField, Min(0.1f)]
    private float spellLifeTime = 1;
    [SerializeField, Min(0.1f)]
    private float spellReloadTime = 5;

    [Space(20)]
    [Header("Атака посохом")]
    [SerializeField, Min(1)]
    private float nearAttackDistance = 1.5f;
    [SerializeField]
    private GameObject slashEffect;

    [Space(20)]
    [Header("Лечение")]
    [SerializeField, Min(1)]
    private int enemiesForHealCount = 3;
    [SerializeField, Min(0.01f)]
    private float healDelayTime = 0.2f;
    [SerializeField, Min(0.01f)]
    private float healReloadTime = 50f;
    [SerializeField]
    private GameObject healAura;
    [SerializeField, Min(1)]
    private int healForce = 10;
    [SerializeField]
    private AudioClip healSound;
    [SerializeField]
    private GameObject viewZone;


    private NavMeshAgent agent;
    private Transform target;
    private float shieldTime;
    private float currentSpellAttackReloadTime = 0;
    private float currentHealReloadTime = 0;
    private List<MagicBullet> targetTrackerBullets = new List<MagicBullet>();
    private List<Enemy> enemiesForHeal = new List<Enemy>();
    private AudioSource source;
    private Vector3 directionToPlayer;

    protected override void SetDefaultState()
    {
        shield.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        target = player.transform;
        source = GetComponent<AudioSource>();
        slashEffect.SetActive(false);

        NecromancerAnimatorEventObserver observer = animator.GetComponent<NecromancerAnimatorEventObserver>();
        observer.NearAttackActivePhase += OnNearAttackActivePhase;
        observer.NearAttackEnd += OnStopNearAttack;
        observer.UseSpellAnimation += OnCastSpell;
        observer.HealAnimationEnd += StartMoveToPlayer;
        observer.HitAnimationEnd += StartMoveToPlayer;

        StartMoveToPlayer();
    }

    private void State_MoveToPlayer()
    {
        ReturnShield();
        ReloadForces();

        agent.destination = target.position;
        directionToPlayer = target.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (enemiesForHeal.Count >= enemiesForHealCount && currentHealReloadTime == 0)
        {
            StartHeal();
        }
        else if (distance <= nearAttackDistance)
        {
            StartNearAttack();
        }
        else if (distance > nearAttackDistance && distance <= farAttackDistance)
        {
            if (currentSpellAttackReloadTime == 0)
            {
                StartSpellAttack();
            }
        }
    }
    private void State_LookToPlayer()
    {
        directionToPlayer = target.position - transform.position;
        transform.forward = directionToPlayer;
    }
    private void State_NavigateSpell()
    {
        if (targetTrackerBullets.Count > 0)
        {
            directionToPlayer = target.position - transform.position;
            transform.forward = directionToPlayer;
        }
        else
        {
            StopCastSpells();
        }
    }

    private void StartMoveToPlayer()
    {
        animator.SetBool("Walk", true);
        agent.isStopped = false;
        currentAction = State_MoveToPlayer;
    }

    private void StartHeal()
    {
        currentAction = emptyAction;
        currentHealReloadTime = healReloadTime;
        agent.isStopped = true;
        animator.SetTrigger("Heal");
        source.PlayOneShot(healSound);
        StartCoroutine(HealAllEnemies());
    }
    private IEnumerator HealAllEnemies()
    {
        for (int i = 0; i < enemiesForHealCount; i++)
        {
            Enemy enemy = enemiesForHeal[i];

            Instantiate(healAura, enemiesForHeal[i].transform.position,
                Quaternion.identity, enemy.transform);

            yield return new WaitForSeconds(healDelayTime);

            if (enemy != null)
            {
                enemy.Heal(healForce);
            }
        }

        enemiesForHeal.Clear();

        enemiesForHealCount++;
        healForce += 3;

        currentHealReloadTime = healReloadTime;
    }

    private void StartSpellAttack()
    {
        agent.isStopped = true;
        animator.SetBool("Walk", false);
        animator.SetBool("UseFarAttack", true);
        currentAction = State_LookToPlayer;
    }
    private void OnCastSpell()
    {
        LaunchSpell(target);
        currentAction = State_NavigateSpell;
    }
    private void StopCastSpells()
    {
        currentSpellAttackReloadTime = spellReloadTime;
        animator.SetBool("UseFarAttack", false);
        StartMoveToPlayer();
    }

    private void StartNearAttack()
    {
        agent.isStopped = true;
        animator.SetBool("Walk", false);
        animator.SetTrigger("UseNearAttack");
        currentAction = State_LookToPlayer;
    }
    private void OnNearAttackActivePhase()
    {
        slashEffect.SetActive(true);
        directionToPlayer = target.position - transform.position;
        float distance = directionToPlayer.magnitude;
        if (distance <= nearAttackDistance)
        {
            player.GetDamage(damage);
        }
    }
    private void OnStopNearAttack()
    {
        slashEffect.SetActive(false);
        directionToPlayer = target.position - transform.position;
        float distance = directionToPlayer.magnitude;
        if (distance <= nearAttackDistance)
        {
            StartNearAttack();
        }
        else
        {
            StartMoveToPlayer();
        }
    }

    public override void GetDamage(int damage)
    {
        shield.SetActive(true);
        shieldTime = 1;
    }

    protected override void Dead()
    {
        currentAction = emptyAction;
        GetComponent<Collider>().enabled = false;
        viewZone.SetActive(false);
        agent.isStopped = true;
        deadEvent.Invoke(this);
        animator.SetTrigger("Dead");
        Destroy(gameObject, deadDelay);
    }

    private void LaunchSpell(Transform target)
    {
        MagicanTargetTrackerBullet currentBullet = Instantiate(bullet, spellSpawnPoint.position, Quaternion.identity).
            GetComponent<MagicanTargetTrackerBullet>();
        targetTrackerBullets.Add(currentBullet);
        currentBullet.bulletDestroyedEvent.AddListener(OnBulletDestroyed);
        currentBullet.target = target;
        currentBullet.targetTrackingForce = targetTrackingForce;
        currentBullet.transform.parent = null;
        Vector3 direction = player.transform.position + Vector3.up*0.5f - spellSpawnPoint.position;
        currentBullet.transform.forward = direction;

        currentBullet.LaunchBullet(spellSpeed, spellLifeTime, spellDamage, true);
    }

    private void ReturnShield()
    {
        if(!shield.activeSelf)
        {
            return;
        }

        if(shieldTime > 0)
        {
            shieldTime -= Time.deltaTime;
        }
        else
        {
            shield.SetActive(false);
            shieldTime = 0;
        }
    }

    private void ReloadForces()
    {
        currentSpellAttackReloadTime -= Time.deltaTime;
        currentSpellAttackReloadTime = Mathf.Clamp(currentSpellAttackReloadTime, 0, spellReloadTime);

        currentHealReloadTime -= Time.deltaTime;
        currentHealReloadTime = Mathf.Clamp(currentHealReloadTime, 0, healReloadTime);
    }

    private void OnBulletDestroyed(MagicBullet magicBullet)
    {
        targetTrackerBullets.Remove(magicBullet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Splash"))
        {
            if(shield.activeSelf)
            {
                return;
            }


            agent.isStopped = true;
            HP -= damage;

            if (HP <= 0)
            {
                Dead();
            }
            else
            {
                animator.SetTrigger("Hit");
            }
        }
    }

    public void AddEnemyToHeal(Enemy enemy)
    {
        enemy.deadEvent.AddListener(OnEnemyDead);
        enemiesForHeal.Add(enemy);
    }

    private void OnEnemyDead(Enemy enemy)
    {
        enemiesForHeal.Remove(enemy);
    }
}

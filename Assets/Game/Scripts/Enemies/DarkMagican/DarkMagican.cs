using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

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
    private float spellCountInAttack = 1;
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
    [SerializeField, Min(0.01f)]
    private float spellCastTime = 0.1f;
    [SerializeField, Min(0.1f)]
    private float spellReloadTime = 5;

    [Space(20)]
    [Header("Атака посохом")]
    [SerializeField, Min(1)]
    private float nearAttackDistance = 1.5f;
    [SerializeField, Min(0.01f)]
    private float nearAttackReloadTime = 0.3f;
    [SerializeField]
    private GameObject slashEffect;

    [Space(20)]
    [Header("Лечение")]
    [SerializeField, Min(1)]
    private int enemiesForHealCount = 3;
    [SerializeField, Min(0.01f)]
    private float healDelayTime = 0.3f;
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

    private List<MagicBullet> targetTrackerBullets = new List<MagicBullet>();
    ActionType actionType = ActionType.Move;

    private List<Enemy> enemiesForHeal = new List<Enemy>();

    protected override IEnumerator MainCoroutine()
    {
        shield.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        target = player.transform;

        AudioSource source = GetComponent<AudioSource>();

        float currentSpellAttackReloadTime = 0;
        float currentNearAttackReloadTime = 0;
        float currentHealReloadTime = 0;

        slashEffect.SetActive(false);

        Vector3 directionToPlayer;

        animator.SetBool("Walk", false);


        bool navigateSpells = false;

        while(HP > 0)
        {

            if (actionType == ActionType.Hit)
            {
                yield return null;
                continue;
            }

            agent.destination = target.position;

            ReturnShield();

            actionType = ActionType.Move;

            currentSpellAttackReloadTime -= Time.deltaTime;
            currentSpellAttackReloadTime = Mathf.Clamp(currentSpellAttackReloadTime, 0, spellReloadTime);

            currentNearAttackReloadTime -= Time.deltaTime;
            currentNearAttackReloadTime = Mathf.Clamp(currentNearAttackReloadTime, 0, nearAttackReloadTime);

            currentHealReloadTime -= Time.deltaTime;
            currentHealReloadTime = Mathf.Clamp(currentHealReloadTime, 0, healReloadTime);

            directionToPlayer = target.position - transform.position;
            float distance = directionToPlayer.magnitude;

            if (actionType == ActionType.UseHeal || 
                (enemiesForHeal.Count >= enemiesForHealCount && currentHealReloadTime == 0))
            {
                agent.isStopped = true;
                actionType = ActionType.UseHeal;
                animator.SetTrigger("Heal");
                source.PlayOneShot(healSound);

                for (int i = 0; i < enemiesForHealCount; i++)
                {
                    Enemy enemy = enemiesForHeal[i];

                    Instantiate(healAura, enemiesForHeal[i].transform.position,
                        Quaternion.identity, enemy.transform);
                    yield return new WaitForSeconds(healDelayTime);

                    if(enemy != null)
                    {
                        enemy.Heal(healForce);
                    }
                }

                for (int i = 0; i < enemiesForHealCount; i++)
                {
                    enemiesForHeal.RemoveAt(0);
                }

                enemiesForHealCount++;
                healForce += 3;

                currentHealReloadTime = healReloadTime;
            }
            else if (distance <= nearAttackDistance)
            {
                if (currentNearAttackReloadTime == 0)
                {
                    actionType = ActionType.NearAttack;
                }
                else
                {
                    actionType = ActionType.Move;
                }
            }
            else if (distance > nearAttackDistance && distance <= farAttackDistance)
            {
                if(currentSpellAttackReloadTime == 0)
                {
                    actionType = ActionType.AttackSpell;
                }
                else
                {
                    actionType = ActionType.Move;
                }
            }
            else if(distance > farAttackDistance)
            {
                actionType=ActionType.Move;
            }

            switch (actionType)
            {
                case ActionType.NearAttack:
                    {
                        directionToPlayer = target.position - transform.position;
                        transform.forward = directionToPlayer;
                        agent.isStopped = true;
                        animator.SetBool("Walk", false);
                        animator.SetTrigger("UseNearAttack");
                        yield return new WaitForSeconds(nearAttackReloadTime);
                        slashEffect.SetActive(true);
                        directionToPlayer = target.position - transform.position;
                        transform.forward = directionToPlayer;
                        distance = directionToPlayer.magnitude;
                        if (distance <= nearAttackDistance)
                        {
                            player.GetDamage(damage);
                        }
                        yield return new WaitForSeconds(nearAttackReloadTime * 2);
                        currentNearAttackReloadTime = nearAttackReloadTime;
                        slashEffect.SetActive(false);
                    }
                    break;

                case ActionType.AttackSpell:
                    {
                        if(!navigateSpells)
                        {
                            agent.isStopped = true;
                            animator.SetBool("Walk", false);
                            animator.SetBool("UseFarAttack", true);
                            yield return new WaitForSeconds(spellCastTime);
                            directionToPlayer = target.position - transform.position;
                            transform.forward = directionToPlayer;
                            for (int i = 0; i < spellCountInAttack; i++)
                            {
                                LaunchSpell(target);
                                yield return new WaitForSeconds(spellCastTime);
                            }

                            navigateSpells = true;
                        }
                        else
                        {
                            while (targetTrackerBullets.Count > 0)
                            {
                                directionToPlayer = target.position - transform.position;
                                transform.forward = directionToPlayer;
                                yield return null;
                            }

                            currentSpellAttackReloadTime = spellReloadTime;
                            navigateSpells = false;
                            animator.SetBool("UseFarAttack", false);
                        }
                    }
                    break;
                case ActionType.Move:
                    MoveAction();
                    break;

                default:
                    MoveAction();
                    break;
            }

            yield return null;
        }
    }

    public override void GetDamage(int damage)
    {
        shield.SetActive(true);
        shieldTime = 1;
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
        Vector3 direction = (player.transform.position + Vector3.up*0.5f) - spellSpawnPoint.position;
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

    private void MoveAction()
    {
        agent.isStopped = false;
        agent.destination = target.position;
        animator.SetBool("Walk", true);
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
            actionType = ActionType.Hit;

            HP -= damage;
            //Debug.Log(gameObject.name + ": " + HP);

            if (HP <= 0)
            {
                Dead();
            }
            else
            {
                animator.SetTrigger("Hit");
                StopCoroutine(ReturnActive());
                StartCoroutine(ReturnActive());
            }
        }
    }

    private IEnumerator ReturnActive()
    {
        yield return new WaitForSeconds(hitDelay);
        actionType = ActionType.Move;
    }

    protected override void Dead()
    {
        GetComponent<Collider>().enabled = false;
        viewZone.SetActive(false);
        agent.isStopped = true;
        deadEvent.Invoke(this);
        animator.SetTrigger("Dead");
        StopAllCoroutines();
        Destroy(gameObject, deadDelay);
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

public enum ActionType
{
    Move,
    UseHeal,
    AttackSpell,
    NearAttack,
    Hit
}

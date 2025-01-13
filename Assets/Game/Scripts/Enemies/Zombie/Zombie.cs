using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : Enemy
{
    [SerializeField]
    private float attackDistance;
    [SerializeField, Min(0)]
    private float hitDelay;
    [SerializeField, Min(0)]
    private float deadDelay = 4;

    [SerializeField]
    private Animator m_Animator;

    private bool isStunned = false;
    private NavMeshAgent m_Agent;
    private Vector3 direction;

    protected override void SetDefaultState()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.speed = speed;
        m_Agent.isStopped = false;
        m_Agent.destination = player.transform.position;

        ZombieAnimatorEventObserver observer = m_Animator.GetComponent<ZombieAnimatorEventObserver>();
        observer.EnemyAttackActivePhase += TryPushDamageToTarget;
        observer.HitAnimationEnd += ReturnActive;

        currentAction = MoveToPlayer;
    }

    private void MoveToPlayer()
    {
        direction = player.transform.position - transform.position;

        if (direction.magnitude > attackDistance)
        {
            m_Agent.destination = player.transform.position;
        }
        else
        {
            m_Agent.isStopped = true;
            m_Animator.SetTrigger("Attack");
            currentAction = emptyAction;
        }
    }

    public void TryPushDamageToTarget()
    {
        direction = player.transform.position - transform.position;
        transform.forward = direction;
        if (direction.magnitude < attackDistance && !isStunned)
        {
            player.GetDamage(damage);
        }
        m_Agent.isStopped = false;
        currentAction = MoveToPlayer;
    }

    private void ReturnActive()
    {
        m_Agent.isStopped = isStunned = false;
        currentAction = MoveToPlayer;
    }

    public override void GetDamage(int damage)
    {
        currentAction = emptyAction;
        m_Agent.isStopped = isStunned = true;
        m_Animator.SetTrigger("Hit");
        base.GetDamage(damage);
    }

    protected override void Dead()
    {

        m_Agent.isStopped = true;
        deadEvent.Invoke(this);
        m_Animator.SetTrigger("Dead");
        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        Destroy(gameObject, deadDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Splash"))
        {
            GetDamage(other.GetComponent<Splash>().Damage);
        }
        else if(other.CompareTag("HealView"))
        {
            other.transform.parent.GetComponent<DarkMagican>().AddEnemyToHeal(this);
        }
    }
}

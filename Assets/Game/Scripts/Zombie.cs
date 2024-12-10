using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : Enemy
{
    [SerializeField]
    private float attackDistance;
    [SerializeField, Min(0)]
    private float attackDeleay;
    [SerializeField, Min(0)]
    private float hitDelay;
    [SerializeField, Min(0)]
    private float deadDelay = 4;

    [SerializeField]
    private Animator m_Animator;

    private bool isStunned = false;
    private NavMeshAgent m_Agent;

    protected IEnumerator MainCoroutine()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        m_Agent.speed = speed;

        Vector3 direction;

        while (true)
        {
            if (isStunned )
            {
                yield return null;
                continue;
            }

            direction = player.transform.position - transform.position;

            if(direction.magnitude > attackDistance)
            {
                m_Agent.destination = player.transform.position;
            }
            else
            {
                m_Agent.isStopped = true;
                m_Animator.SetTrigger("Attack");
                yield return new WaitForSeconds(attackDeleay);
                if(direction.magnitude < attackDistance && !isStunned)
                {
                    player.GetDamage(damage);
                }
            }

            yield return null;
        }
    }

    private Coroutine currentReturnActiveCoroutine;
    public override void GetDamage(int damage)
    {
        m_Agent.isStopped = isStunned = true;
        m_Animator.SetTrigger("Hit");
        if(currentReturnActiveCoroutine != null)
        {
            StopCoroutine(currentReturnActiveCoroutine);
        }
        currentReturnActiveCoroutine = StartCoroutine(ReturnActive());
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

    private IEnumerator ReturnActive()
    {
        yield return new WaitForSeconds(hitDelay);
        m_Agent.isStopped = isStunned = false;
        currentReturnActiveCoroutine = null;
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

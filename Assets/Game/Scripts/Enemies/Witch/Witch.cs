using System.Collections;
using UnityEngine;

public class Witch : Enemy
{
    [SerializeField]
    private Animator animator;
    [SerializeField, Min(0)]
    private float flyingHeight = 10;
    [SerializeField, Min(0.1f)]
    private float hitDelay;
    [SerializeField, Min(0.1f)]
    private float deadDelay;

    [Header("TP")]
    [Space(20)]
    [SerializeField]
    private GameObject TPDecal;
    [SerializeField, Min(1)]
    private float arenaRadius;
    [SerializeField, Min(0.1f)]
    private float tpDelay;

    [Header("Spell")]
    [Space(20)]
    [SerializeField, Min(1f)]
    private float spellLifeTime = 15;
    [SerializeField, Min(1f)]
    private float spellSpeed = 15;
    [SerializeField, Min(0.01f)]
    private float shootDelay = 0.01f;
    [SerializeField]
    private Transform spellRunePrefab;
    [SerializeField]
    private GameObject spellBulletPrefab;

    private Vector3 PlayerTarget => player.transform.position + Vector3.up * 0.5f;
    private Transform arenaCenter;
    private SpellRune rune;
    private float currentSpellTime = 0;

    protected override void SetDefaultState()
    {
        rune = spellRunePrefab.GetComponent<SpellRune>();
        rune.damage = damage;
        rune.spellLifeTime = spellLifeTime;
        rune.spellSpeed = spellSpeed;
        rune.shootDelay = shootDelay;
        rune.bullet = spellBulletPrefab;

        spellRunePrefab.gameObject.SetActive(false);

        arenaCenter = GameCenter.arenaCenter;

        WitchAnimatorEventObserver observer = animator.GetComponent<WitchAnimatorEventObserver>();
        observer.RuneActivated += ActivateRune;
        observer.HitEnd += StartTeleport;


        currentAction = State_MoveUp;
    }

    private void State_MoveUp()
    {
        if (transform.position.y < flyingHeight)
        {
            State_LookAtPlayer();
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else
        {
            StartUseRune();
        }
    }
    private void State_LookAtPlayer()
    {
        transform.forward = GetLoocAtPlayerVector();
    }
    private void State_UseRune()
    {
        if (currentSpellTime < spellLifeTime)
        {
            currentSpellTime += Time.deltaTime;
            transform.forward = GetLoocAtPlayerVector();

            Vector3 dir = PlayerTarget - spellRunePrefab.position;
            spellRunePrefab.forward = dir;
        }
        else
        {
            StartTeleport();
        }
    }

    private void StartTeleport()
    {
        currentAction = emptyAction;
        StartCoroutine(TPCoroutine());
    }
    private IEnumerator TPCoroutine()
    {
        spellRunePrefab.gameObject.SetActive(false);
        Vector3 target = GetRandomPointInArena();
        animator.SetTrigger("TP");
        GameObject tp1 = Instantiate(TPDecal, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(tpDelay);
        GameObject tp2 = Instantiate(TPDecal, target, Quaternion.identity);
        yield return new WaitForSeconds(tpDelay);
        transform.position = target;
        Destroy(tp1, tpDelay);
        Destroy(tp2, tpDelay * 2);
        currentAction = StartUseRune;
    }

    private void StartUseRune()
    {
        currentAction = State_LookAtPlayer;
        animator.SetTrigger("Attack");
    }
    private void ActivateRune()
    {
        spellRunePrefab.gameObject.SetActive(true);
        spellRunePrefab.GetComponent<SpellRune>().Activate();
        currentSpellTime = 0;
        currentAction = State_UseRune;
    }

    private Vector3 GetRandomPointInArena()
    {
        float x = Random.Range(-arenaRadius, arenaRadius);
        float z = Random.Range(-arenaRadius, arenaRadius);

        return arenaCenter.position + new Vector3(x, flyingHeight, z);
    }
    private Vector3 GetLoocAtPlayerVector()
    {
        return player.transform.position - new Vector3(transform.position.x, arenaCenter.position.y, transform.position.z);
    }

    public override void GetDamage(int damage)
    {
        HP -= damage;
        spellRunePrefab.gameObject.SetActive(false);

        if (HP < 0)
        {
            HP = 0;
            Dead();
            return;
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }
    protected override void Dead()
    {

        deadEvent.Invoke(this);
        animator.SetBool("StartFall", true);
        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        StartCoroutine(FallCoroutine());
    }
    private IEnumerator FallCoroutine()
    {
        float speed = 0;

        while(transform.position.y > arenaCenter.position.y)
        {
            speed += Time.deltaTime;
            transform.position += Vector3.down * speed;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, arenaCenter.position.y, transform.position.z);
        animator.SetTrigger("Dead");
        Destroy(gameObject, deadDelay);
    }
}

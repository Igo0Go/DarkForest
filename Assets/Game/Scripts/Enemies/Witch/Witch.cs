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
    private float spellTime = 7;
    [SerializeField, Min(0.01f)]
    private float shootDelay = 0.01f;
    [SerializeField]
    private Transform spellRunePrefab;
    [SerializeField]
    private GameObject spellBulletPrefab;

    private WitchActionType actionType;

    private Vector3 PlayerTarget => player.transform.position + Vector3.up * 0.5f;
    private Transform arenaCenter;

    protected override IEnumerator MainCoroutine()
    {
        SpellRune rune = spellRunePrefab.GetComponent<SpellRune>();
        rune.damage = damage;
        rune.spellLifeTime = spellLifeTime;
        rune.spellSpeed = spellLifeTime;
        rune.shootDelay = shootDelay;
        rune.bullet = spellBulletPrefab;

        spellRunePrefab.gameObject.SetActive(false);

        arenaCenter = GameCenter.arenaCenter;

        while(transform.position.y < flyingHeight)
        {
            transform.forward = GetLoocAtPlayerVector();
            transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }
        actionType = WitchActionType.Attack;


        while (HP > 0)
        {
            switch (actionType)
            {
                case WitchActionType.Attack:
                    {

                        animator.SetTrigger("Attack");
                        yield return new WaitForSeconds(1);
                        spellRunePrefab.gameObject.SetActive(true);

                        spellRunePrefab.GetComponent<SpellRune>().Activate();


                        float currentSpellTime = 0;
                        while(currentSpellTime < spellLifeTime)
                        {
                            currentSpellTime += Time.deltaTime;
                            transform.forward = GetLoocAtPlayerVector();

                            Vector3 dir = PlayerTarget - spellRunePrefab.position;
                            spellRunePrefab.forward = dir;

                            if(actionType == WitchActionType.TP)
                            {
                                break;
                            }

                            yield return null;
                        }

                        spellRunePrefab.gameObject.SetActive(false);

                        actionType = WitchActionType.TP;
                    }
                    break;
                case WitchActionType.TP:
                    {
                        Vector3 target = GetRandomPointInArena();
                        animator.SetTrigger("TP");
                        GameObject tp1 = Instantiate(TPDecal, transform.position, Quaternion.identity);
                        yield return new WaitForSeconds(tpDelay);
                        GameObject tp2 = Instantiate(TPDecal, target, Quaternion.identity);
                        yield return new WaitForSeconds(tpDelay);
                        transform.position = target;
                        Destroy(tp1, tpDelay);
                        Destroy(tp2, tpDelay * 2);
                        yield return new WaitForSeconds(2);
                        actionType = WitchActionType.Attack;
                    }
                    break;
            }

            yield return null;
        }
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
            StopCoroutine(ReturnActive());
            StartCoroutine(ReturnActive());
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

    private IEnumerator ReturnActive()
    {
        yield return new WaitForSeconds(hitDelay);
        actionType = WitchActionType.TP;
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

public enum WitchActionType
{
    Attack,
    TP,
    Dead,
}

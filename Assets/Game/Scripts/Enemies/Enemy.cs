using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField, Min(1)]
    protected int HP = 1;
    [SerializeField, Min(1)]
    protected int damage = 1;
    [SerializeField, Min(0.1f)]
    protected float speed = 1;

    public UnityEvent<Enemy> deadEvent;

    protected PlayerInteraction player;

    private void Start()
    {
        player = FindObjectOfType<PlayerInteraction>();
        StartCoroutine(MainCoroutine());
    }

    protected abstract IEnumerator MainCoroutine();

    public virtual void GetDamage(int damage)
    {

        HP-=damage;
        //Debug.Log(gameObject.name + ": " + HP);

        if (HP <= 0)
        {
            Dead();
        }

    }

    public void Heal(int hp)
    {
        HP += hp;
    }

    protected virtual void AttackPalyer()
    {
        player.GetDamage(damage);
    }

    protected virtual void Dead()
    {
        deadEvent.Invoke(this);
        Destroy(gameObject);
    }
}

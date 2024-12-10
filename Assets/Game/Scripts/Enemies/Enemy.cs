using System;
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

    public float Speed => speed;
    public PlayerInteraction Player => player;

    public UnityEvent<Enemy> deadEvent;

    protected PlayerInteraction player;

    protected Action currentAction;

    protected Action emptyAction = () => { };

    private void Start()
    {
        player = FindObjectOfType<PlayerInteraction>();
        SetDefaultState();
    }

    protected virtual void SetDefaultState()
    {
        currentAction = emptyAction;
    }

    private void Update()
    {
        currentAction();
    }

    public virtual void GetDamage(int damage)
    {

        HP-=damage;

        if (HP <= 0)
        {
            Dead();
        }

    }

    public void Heal(int hp)
    {
        HP += hp;
    }

    public virtual void AttackPalyer()
    {
        player.GetDamage(damage);
    }

    protected virtual void Dead()
    {
        deadEvent.Invoke(this);
        Destroy(gameObject);
    }
}
using System;
using System.Collections;
using UnityEngine;

public class PlayerInteraction : PlayerPart
{
    [SerializeField, Min(1)]
    private float maxHp = 100;
    [SerializeField, Min(0)]
    private float regenReloadDeleyTime;
    [SerializeField, Min(1)]
    private float regenSpeed = 3;

    public float HP
    {
        get 
        { 
            return hp;
        }
        set 
        {  
            hp = Mathf.Clamp(value, 0, maxHp);
            HPValueChanget.Invoke(hp);
            if (hp <= 0)
            {
                DeadEvent.Invoke();
            }
        }
    }
    private float hp;

    public Action DeadEvent;
    public Action<float> HPValueChanget;
    public Action<float> HPMaxValueChanget;
    public Action<float> DamageValueChanged;

    private float regenReloadTime = 0;

    public override void Activate()
    {
        hp = maxHp;
        HPMaxValueChanget.Invoke(maxHp);
        HPValueChanget.Invoke(hp);
        DamageValueChanged.Invoke(0);
        StartCoroutine(RegenCoroutine());
    }

    public void GetDamage(int  damage)
    {
        hp -= damage;
        GameCenter.CurrentRageValue -= 10;
        if (hp < 0)
        {

        }
        regenReloadTime = regenReloadDeleyTime;
        DamageValueChanged.Invoke(1);

        HPValueChanget.Invoke(hp);
    }

    private IEnumerator RegenCoroutine()
    {
        while (hp > 0)
        {
            if (regenReloadTime == 0)
            {
                DamageValueChanged.Invoke(0);
                HP += Time.deltaTime * regenSpeed;
                yield return null;
            }
            else
            {
                regenReloadTime -= Time.deltaTime;

                float T = Mathf.Lerp(0, regenReloadDeleyTime, regenReloadTime / regenReloadDeleyTime);

                DamageValueChanged.Invoke(T);

                if (regenReloadTime < 0)
                {
                    regenReloadTime = 0;
                }
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("InteractiveTrigger"))
        {
            other.GetComponent<InteractiveTrigger>().Activate();
        }
    }
}

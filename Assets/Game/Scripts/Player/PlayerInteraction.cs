using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Slider hpSlider;

    [SerializeField, Min(1)]
    private float hp;
    [SerializeField, Min(0)]
    private float regenReloadDeleyTime;
    [SerializeField, Min(1)]
    private float regenSpeed = 3;

    [SerializeField]
    private UnityEvent deadEvent;

    private float regenReloadTime = 0;
    

    private void Awake()
    {
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

        StartCoroutine(RegenCoroutine());
    }


    private IEnumerator RegenCoroutine()
    {
        while(hp > 0)
        {
            if (regenReloadTime == 0)
            {
                hp += Time.deltaTime * regenSpeed;
                hp = Mathf.Clamp(hp, 0, (int)hpSlider.maxValue);
                hpSlider.value = hp;
                yield return null;
            }
            else
            {
                regenReloadTime -= Time.deltaTime;
                if (regenReloadTime < 0)
                {
                    regenReloadTime = 0;
                }
                yield return null;
            }
        }
    }

    public void GetDamage(int  damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            hp = 0;
            deadEvent.Invoke();
            Time.timeScale = 0;
        }
        regenReloadTime = regenReloadDeleyTime;

        hpSlider.value = hp;
    }
}

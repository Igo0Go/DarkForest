using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Image damagePanel;

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

        SetAlphaForDamagePanel(0);

        StartCoroutine(RegenCoroutine());
    }


    private IEnumerator RegenCoroutine()
    {
        while(hp > 0)
        {
            if (regenReloadTime == 0)
            {
                SetAlphaForDamagePanel(0);
                hp += Time.deltaTime * regenSpeed;
                hp = Mathf.Clamp(hp, 0, (int)hpSlider.maxValue);
                hpSlider.value = hp;
                yield return null;
            }
            else
            {
                regenReloadTime -= Time.deltaTime;

                float T = Mathf.Lerp(0, regenReloadDeleyTime, regenReloadTime/regenReloadDeleyTime);

                Debug.Log(T);

                SetAlphaForDamagePanel(T);

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
        SetAlphaForDamagePanel(1);

        hpSlider.value = hp;
    }

    private void SetAlphaForDamagePanel(float alpha)
    {
        damagePanel.color = new Color(damagePanel.color.r, damagePanel.color.g, damagePanel.color.b, alpha);
    }
}

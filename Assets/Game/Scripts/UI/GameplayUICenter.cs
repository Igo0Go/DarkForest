using UnityEngine;
using UnityEngine.UI;

public class GameplayUICenter : MonoBehaviour
{
    [SerializeField]
    private Slider magicSlider;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private GameObject deadPanel;
    [SerializeField]
    private Image damagePanel;


    private void Awake()
    {
        PlayerMagic playerMagic = FindObjectOfType<PlayerMagic>();
        playerMagic.MagicMaxValueChanget += SetMaxValueForMagicSlider;
        playerMagic.MagicValueChanget+= SetValueForMagicSlider;

        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        playerInteraction.HPMaxValueChanget += SetMaxValueForHPSlider;
        playerInteraction.HPValueChanget += SetValueForHPSlider;
        playerInteraction.DeadEvent += OnDead;
        playerInteraction.DamageValueChanged += SetAlphaForDamagePanel;


        deadPanel.SetActive(false);
    }

    private void SetMaxValueForMagicSlider(float value)
    {
        magicSlider.maxValue = value;
    }
    private void SetValueForMagicSlider(float value)
    {
        magicSlider.value = value;
    }

    private void SetMaxValueForHPSlider(float value)
    {
        hpSlider.maxValue = value;
    }
    private void SetValueForHPSlider(float value)
    {
        hpSlider.value = value;
    }

    private void OnDead()
    {
        deadPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void SetAlphaForDamagePanel(float alpha)
    {
        damagePanel.color = new Color(damagePanel.color.r, damagePanel.color.g, damagePanel.color.b, alpha);
    }
}

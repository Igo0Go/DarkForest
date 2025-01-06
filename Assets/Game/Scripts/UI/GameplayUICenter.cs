using TMPro;
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
    [SerializeField]
    private Slider rageSlider;
    [SerializeField]
    private TMP_Text rageMultiplicatorText;

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

        GameCenter.CurrentRageMultiplicatorChanged += OnRageMultiplicatorChanged;
        GameCenter.CurrentRageChanged += OnRageValueChanged;

        MusicRageSystem musicRageSystem = FindObjectOfType<MusicRageSystem>();
        musicRageSystem.MaxRageChanged += OnRageMaxChanged;
        musicRageSystem.MinRageChanged += OnRageMinChanged;

        deadPanel.SetActive(false);
        rageSlider.value = 0;
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

    private void OnRageMultiplicatorChanged(int multiplicator)
    {
        rageMultiplicatorText.text = "X" + multiplicator.ToString();
    }
    private void OnRageValueChanged(float value)
    {
        rageSlider.value = value;
    }

    private void OnRageMaxChanged(float value)
    {
        rageSlider.maxValue = value;
    }
    private void OnRageMinChanged(float value)
    {
        rageSlider.minValue = value;
    }
}

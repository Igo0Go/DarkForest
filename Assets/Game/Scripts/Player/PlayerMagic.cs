using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PlayerMagic : MonoBehaviour
{
    [SerializeField]
    private List<MagicSpell> spells;
    [SerializeField]
    public Animator handsAnimator;
    [SerializeField]
    private Slider magicSlider;

    private MagicSpell currentSpell;

    private bool spellSwitchKey = true;

    private void Awake()
    {
        PlayerLook look = GetComponent<PlayerLook>();

        foreach (MagicSpell spell in spells)
        {
            spell.spellCamera = look;
            spell.hands = handsAnimator;
            spell.ChangeGrandSpellValue.AddListener(OnSpellGrandValueChanged);
            spell.ChangeSwitchKey.AddListener(SetSpellSwitchKey);
            spell.gameObject.SetActive(false);
        }

        currentSpell = spells[0];
        currentSpell.gameObject.SetActive(true);
        magicSlider.maxValue = currentSpell.GrandSpellRate;
        magicSlider.value = 0;
    }

    private void Update()
    {
        if (spellSwitchKey)
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                SetSpell(0);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                SetSpell(1);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                SetSpell(2);
            }
        }

        currentSpell.UseMainSpel();
        currentSpell.UseAltSpell();
        currentSpell.UseGrandSpell();
    }

    private void SetSpell(int number)
    {
        foreach (MagicSpell spell in spells)
        {
            spell.gameObject.SetActive(false);
        }
        currentSpell = spells[number];
        currentSpell.gameObject.SetActive(true);
        currentSpell.SetUpSpell();
        magicSlider.maxValue = currentSpell.GrandSpellRate;
        magicSlider.value = currentSpell.GrandSpellValue;
        handsAnimator.SetBool("UseTwo", false);
    }

    private void OnSpellGrandValueChanged(float value)
    {
        magicSlider.value = value;
        GameCenter.CurrentRageValue++;
    }

    private void SetSpellSwitchKey(bool switchKey)
    {
        spellSwitchKey = switchKey;
    }
}

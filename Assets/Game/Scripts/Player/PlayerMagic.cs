using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : PlayerPart
{
    [SerializeField]
    private List<MagicSpell> spells;
    [SerializeField]
    public Animator handsAnimator;

    [HideInInspector]
    public Action<float> MagicValueChanget;
    [HideInInspector]
    public Action<float> MagicMaxValueChanget;

    private MagicSpell currentSpell;

    private bool spellSwitchKey = true;

    public override void Activate()
    {
        PlayerLook look = GetComponent<PlayerLook>();

        foreach (MagicSpell spell in spells)
        {

            spell.spellCamera = look;
            spell.hands = handsAnimator;
            spell.InitSpell();
            spell.ChangeGrandSpellValue.AddListener(OnSpellGrandValueChanged);
            spell.ChangeSwitchKey.AddListener(SetSpellSwitchKey);
            spell.gameObject.SetActive(false);
        }

        currentSpell = spells[0];
        currentSpell.gameObject.SetActive(true);
        MagicMaxValueChanget?.Invoke(currentSpell.GrandSpellRate);
        MagicValueChanget?.Invoke(0);
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
                if(!MagicStats.availableSpells.Contains(MagicSpellType.Ice))
                {
                    return;
                }

                SetSpell(1);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                if (!MagicStats.availableSpells.Contains(MagicSpellType.Fire))
                {
                    return;
                }
                SetSpell(2);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                if (!MagicStats.availableSpells.Contains(MagicSpellType.Sparks))
                {
                    return;
                }
                SetSpell(3);
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
        MagicMaxValueChanget.Invoke(currentSpell.GrandSpellRate);
        MagicValueChanget.Invoke(currentSpell.GrandSpellValue);
        handsAnimator.SetBool("UseTwo", false);
    }

    private void OnSpellGrandValueChanged(float value)
    {
        MagicValueChanget.Invoke(currentSpell.GrandSpellValue);
    }

    private void SetSpellSwitchKey(bool switchKey)
    {
        spellSwitchKey = switchKey;
    }
}

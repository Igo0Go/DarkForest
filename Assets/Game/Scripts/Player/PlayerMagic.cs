using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PlayerMagic : MonoBehaviour
{
    [SerializeField]
    private List<MagicSpell> spells;
    [SerializeField]
    public Animator handsAnimator;

    private MagicSpell currentSpell;

    private void Awake()
    {
        PlayerLook look = GetComponent<PlayerLook>();

        foreach (MagicSpell spell in spells)
        {
            spell.spellCamera = look;
            spell.hands = handsAnimator;
            spell.gameObject.SetActive(false);
        }

        currentSpell = spells[0];
        currentSpell.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
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


        currentSpell.UseMainSpel();
        currentSpell.UseAltSpell();
    }

    private void SetSpell(int number)
    {
        foreach (MagicSpell spell in spells)
        {
            spell.gameObject.SetActive(false);
        }
        currentSpell = spells[number];
        currentSpell.gameObject.SetActive(true);
    }
}

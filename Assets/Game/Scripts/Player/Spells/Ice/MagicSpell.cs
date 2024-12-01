using UnityEngine;
using UnityEngine.Events;

public abstract class MagicSpell :MonoBehaviour
{
    public string Name;

    [HideInInspector]
    public PlayerLook spellCamera;
    [HideInInspector]
    public Animator hands;
    [Min(1)]
    public float GrandSpellRate;

    public UnityEvent<float> ChangeGrandSpellValue;
    public UnityEvent<bool> ChangeSwitchKey;

    private float _grandSpellValue;

    public float GrandSpellValue
    {
        get
        { 
            return _grandSpellValue;
        }
        set
        {
            _grandSpellValue = Mathf.Clamp(value, 0, GrandSpellRate);
            ChangeGrandSpellValue.Invoke(_grandSpellValue);
        }
    }


    public abstract void SetUpSpell();
    public abstract void UseMainSpel();
    public abstract void UseAltSpell();
    public abstract void UseGrandSpell();
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class MagicSpell :MonoBehaviour
{
    [HideInInspector]
    public PlayerLook spellCamera;
    [HideInInspector]
    public Animator hands;
    [Min(1)]
    public float GrandSpellRate;
    [SerializeField]
    protected Image grandSpellImage;
    [SerializeField]
    protected Sprite grandSpellSprite;

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
            CheckGrandSpell();
        }
    }

    public virtual void InitSpell()
    {

    }
    public abstract void SetUpSpell();
    public abstract void UseMainSpel();
    public abstract void UseAltSpell();
    public abstract void UseGrandSpell();
    protected void CheckGrandSpell()
    {
        if (_grandSpellValue >= GrandSpellRate)
        {
            grandSpellImage.gameObject.SetActive(true);
            grandSpellImage.sprite = grandSpellSprite;
        }
        else
        {
            grandSpellImage.gameObject.SetActive(false);
        }
    }
}

public static class MagicStats
{
    public static bool opportunityToIceUpgrade => _iceMainSpeedMultiplicator < 3;
    public static float IceMainSpeedMultiplicator => _iceMainSpeedMultiplicator;
    private static float _iceMainSpeedMultiplicator = 1;

    public static void UpgradeIceSpeed()
    {
        _iceMainSpeedMultiplicator += 0.5f;
    }

    public static bool opportunityToFireUpgrade => _fireMainSpeedMultiplicator < 4;
    public static float FireMainSpeedMultiplicator => _fireMainSpeedMultiplicator;
    private static float _fireMainSpeedMultiplicator = 1;

    public static void UpgradeFireSpeed()
    {
        _fireMainSpeedMultiplicator += 0.5f;
    }

    public static bool opportunityToSparksUpgrade => _sparksMaxTargetsCount < 27;
    public static int SparksMaxTargetsCount => _sparksMaxTargetsCount;
    private static int _sparksMaxTargetsCount = 3;

    public static void UpgradeSparksSpeed()
    {
        _sparksMaxTargetsCount *= 3;
    }

}

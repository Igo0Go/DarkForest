using UnityEngine;

public abstract class MagicSpell :MonoBehaviour
{
    public string Name;

    [HideInInspector]
    public PlayerLook spellCamera;
    [HideInInspector]
    public Animator hands;

    public abstract void UseMainSpel();
    public abstract void UseAltSpell();
}

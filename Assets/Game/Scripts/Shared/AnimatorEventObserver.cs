using System;
using UnityEngine;

public class AnimatorEventObserver : MonoBehaviour
{
    public Action OneHandPushEnd;

    public Action GrandSpellActivated;
    public Action RingSpellActivated;
    public Action SplashAttackPhase;
    public Action SplashEnd;
    public Action PrepareNewAttack;

    public void OnEndPush()
    {
        OneHandPushEnd?.Invoke();
    }

    public void OnSplashEnd()
    {
        SplashEnd?.Invoke();
    }

    public void OnGrandSpell()
    {
        GrandSpellActivated?.Invoke();
    }

    public void OnRingSpell()
    {
        RingSpellActivated?.Invoke();
    }

    public void OnSplashAttackPhase()
    {
        SplashAttackPhase?.Invoke();
    }

    public void OnPrepareNewAttack()
    {
        PrepareNewAttack?.Invoke();
    }
}

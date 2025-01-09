using System;
using UnityEngine;

public class AnimatorEventObserver : MonoBehaviour
{
    public Action OneHandPushEnd;
    public Action SplashEnd;
    public Action GrandSpellActivated;
    public Action RingSpellActivated;

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
}

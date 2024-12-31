using System;
using UnityEngine;

public class AnimatorEventObserver : MonoBehaviour
{
    public Action OneHandPushEnd;
    public Action SplashEnd;
    public Action GrandSpellActivated;


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
}

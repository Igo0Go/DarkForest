using System;
using UnityEngine;

public class AnimatorEventObserver : MonoBehaviour
{
    public Action OneHandPushEnd;
    public Action SplashEnd;

    public void OnEndPush()
    {
        OneHandPushEnd?.Invoke();
    }

    public void OnSplashEnd()
    {
        SplashEnd?.Invoke();
    }
}

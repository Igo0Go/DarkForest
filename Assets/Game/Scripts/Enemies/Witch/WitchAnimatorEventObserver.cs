using UnityEngine;
using System;

public class WitchAnimatorEventObserver : MonoBehaviour
{
    public Action RuneActivated;
    public Action HitEnd;

    public void OnRuneActivated()
    {
        RuneActivated?.Invoke();
    }

    public void OnHitEnd()
    {
        HitEnd?.Invoke();
    }
}

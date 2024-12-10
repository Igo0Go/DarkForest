using UnityEngine;
using System;

public class NecromancerAnimatorEventObserver : MonoBehaviour
{
    public Action UseSpellAnimation;
    public Action HitAnimationEnd;
    public Action HealAnimationEnd;
    public Action NearAttackActivePhase;
    public Action NearAttackEnd;

    public void OnEnemyUseSpell()
    {
        UseSpellAnimation?.Invoke();
    }
    public void OnEnemyHitEnd()
    {
        HitAnimationEnd?.Invoke();
    }
    public void OnEnemyHealEnd()
    {
        HealAnimationEnd?.Invoke();
    }
    public void OnEnemyMiliAttackStart()
    {
        NearAttackActivePhase?.Invoke();
    }
    public void OnEnemyMiliAttackEnd()
    {
        NearAttackEnd?.Invoke();
    }

}

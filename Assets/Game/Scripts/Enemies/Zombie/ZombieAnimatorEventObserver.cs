using UnityEngine;
using System;

public class ZombieAnimatorEventObserver : MonoBehaviour
{
    public Action EnemyAttackActivePhase;
    public Action HitAnimationEnd;

    public void OnEnemyAttackPhase()
    {
        EnemyAttackActivePhase?.Invoke();
    }
    public void OnEnemyHitEnd()
    {
        HitAnimationEnd?.Invoke();
    }
}

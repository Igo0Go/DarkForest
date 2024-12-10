using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField, Min(0)]
    private float startHeight = 10;
    [SerializeField, Range(0.1f, 1f)]
    private float targetTrackingForce;
    [SerializeField, Min(0.1f)]
    private float attackDistance = 1;

    [SerializeField]
    private AudioClip shotClip;
    [SerializeField]
    private AudioClip spawnClip;
    [SerializeField]
    private GameObject deadDecal;

    public float TargetTrackingForce => targetTrackingForce;
    public float AttackDistance => attackDistance;
    public float StartHeight => startHeight;
    public AudioClip SpawnClip => spawnClip;

    protected override void SetDefaultState()
    {
        currentAction = new GhostFlyUp(this);
    }

    public override void AttackPalyer()
    {
        GameCenter.mainSource.PlayOneShot(shotClip);
        player.GetDamage(damage);
        Dead();
    }

    protected override void Dead()
    {
        Instantiate(deadDecal, transform.position, Quaternion.identity);
        base.Dead();
    }
}

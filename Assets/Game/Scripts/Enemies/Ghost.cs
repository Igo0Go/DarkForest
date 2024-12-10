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

    private Vector3 target;
    private Vector3 direction;

    protected override void SetDefaultState()
    {
        target = transform.position + Vector3.up * startHeight;
        direction = target - transform.position;
        currentAction = FlyUp;
    }

    public void FlyUp()
    {
        if (transform.position.y < target.y)
        {
            transform.forward = direction;
            transform.position += direction.normalized * speed * Time.deltaTime;
        }
        else
        {
            if (spawnClip != null)
            {
                GameCenter.mainSource.PlayOneShot(spawnClip);
            }

            direction = player.transform.position - transform.position;
            currentAction = MoveToPlayer;
        }
    }

    public void MoveToPlayer()
    {
        if (direction.magnitude > attackDistance)
        {
            target = player.transform.position;
            direction = target - transform.position;

            transform.forward = Vector3.Lerp(transform.forward, direction, targetTrackingForce);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            AttackPalyer();
        }
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

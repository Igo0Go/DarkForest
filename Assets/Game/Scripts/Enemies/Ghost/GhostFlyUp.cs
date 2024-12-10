using UnityEngine;

public class GhostFlyUp : EnemyState
{
    private Vector3 target;
    private Vector3 direction;
    private float speed;
    private AudioClip clip;
    private Ghost ghost;

    public GhostFlyUp(Ghost ghost) 
        : base(ghost)
    {
        this.ghost = ghost;
        target = ghost.transform.position + Vector3.up * ghost.StartHeight;
        direction = target - ghost.transform.position;
        speed = ghost.Speed;
        clip = ghost.SpawnClip;
    }

    public override void UseState()
    {
        if (ghost.transform.position.y < target.y)
        {
            ghost.transform.forward = direction;
            ghost.transform.position += direction.normalized * speed * Time.deltaTime;
        }
        else
        {
            if (clip != null)
            {
                GameCenter.mainSource.PlayOneShot(clip);
            }

            enemy.SetState(new GhostMoveState(ghost));
        }
    }
}

using UnityEngine;

public class GhostMoveState : EnemyState
{
    private Vector3 direction;
    private Ghost ghost;
    private Vector3 target;

    public GhostMoveState(Ghost enemy) : base(enemy)
    {
        direction = enemy.Player.transform.position - enemy.transform.position;
        ghost = enemy;
    }

    public override void UseState()
    {
        if (direction.magnitude > ghost.AttackDistance)
        {

            target = ghost.Player.transform.position;
            direction = target - ghost.transform.position;

            ghost.transform.forward = Vector3.Lerp(ghost.transform.forward, direction, ghost.TargetTrackingForce);
            ghost.transform.position += ghost.transform.forward * ghost.Speed * Time.deltaTime;
        }
        else
        {
            ghost.AttackPalyer();
        }
    }
}

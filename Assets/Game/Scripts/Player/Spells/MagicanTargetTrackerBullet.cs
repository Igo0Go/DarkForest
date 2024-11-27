using UnityEngine;

public class MagicanTargetTrackerBullet : MagicBullet
{
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public float targetTrackingForce = 0.5f;

    private Vector3 offset = Vector3.up * 0.5f;

    protected override void MoveBullet()
    {
        if (target != null)
        {
            Vector3 direction = target.position + offset - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, targetTrackingForce);
        }
        base.MoveBullet();
    }
}

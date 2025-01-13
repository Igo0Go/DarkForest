using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : MagicBullet
{
    public int pierceCount = 3;

    private int currentPierceCount = 0;
    protected override void CheckHit()
    {
        if (Physics.Linecast(oldPos, myTransform.position, out RaycastHit hit, ~ignoreMask))
        {
            if (useToPlayer && hit.collider.TryGetComponent(out PlayerInteraction player))
            {
                player.GetDamage(damage);
            }
            else if (hit.collider.TryGetComponent(out ICanGetDamage target))
            {
                target.GetDamage(damage);
                DamageEvent?.Invoke(damage);
            }
            else if (hit.collider.TryGetComponent(out Rigidbody targetRb))
            {
                targetRb.AddForce(transform.forward * damage * 10, ForceMode.Impulse);
            }

            for (int i = 0; i < decals.Count; i++)
            {
                Instantiate(decals[i], hit.point + hit.normal * 0.1f, Quaternion.identity).
                    transform.forward = hit.normal;
            }
            bulletDestroyedEvent.Invoke(this);
            currentPierceCount++;
            if (currentPierceCount >= pierceCount)
            {
                Destroy(gameObject);
            }
        }
    }
}

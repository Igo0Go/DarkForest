using UnityEngine;
using UnityEngine.UIElements;

public class FireBall : MagicBullet
{
    [SerializeField, Min(0.1f)]
    private float minScale = 1f;
    [SerializeField, Min(0.1f)]
    private float maxScale = 1f;

    public void SetFireballScale(float t)
    {
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, t);
    }

    protected override void CheckHit()
    {
        if (Physics.Linecast(oldPos, myTransform.position, out RaycastHit hit, ~ignoreMask))
        {
            if (useToPlayer && hit.collider.TryGetComponent(out PlayerInteraction player))
            {
                player.GetDamage(damage);
            }
            else if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.GetDamage(damage);
            }

            for (int i = 0; i < decals.Count; i++)
            {
                GameObject decal = Instantiate(decals[i], hit.point + hit.normal * 0.1f, Quaternion.identity, hit.transform);
                decal.transform.forward = hit.normal;
                decal.GetComponent<Splash>().Damage = damage;
            }
            bulletDestroyedEvent.Invoke(this);
            Destroy(gameObject);
        }

        float step = (myTransform.position - oldPos).magnitude;

        if (Physics.SphereCast(oldPos, myTransform.localScale.x/2, myTransform.forward * step, out hit, ~ignoreMask))
        {
            if (useToPlayer && hit.collider.TryGetComponent(out PlayerInteraction player))
            {
                player.GetDamage(damage);
            }
            else if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.GetDamage(damage);
            }

            for (int i = 0; i < decals.Count; i++)
            {
                GameObject decal = Instantiate(decals[i], hit.point + hit.normal * 0.1f, Quaternion.identity, hit.transform);
                decal.transform.forward = hit.normal;
                decal.GetComponent<Splash>().Damage = damage;
            }
            bulletDestroyedEvent.Invoke(this);
            Destroy(gameObject);
        }
    }
}

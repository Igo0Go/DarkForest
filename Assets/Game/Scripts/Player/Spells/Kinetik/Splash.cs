using UnityEngine;

public class Splash : MonoBehaviour
{
    [SerializeField, Min(1)]
    public int Damage = 10;
    [SerializeField]
    private Transform splashZone;
    [SerializeField]
    private LayerMask activeMask;
    [SerializeField]
    private GameObject splashObject;

    public bool Active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;
            splashObject.SetActive(_active);
        }
    }
    private bool _active = false;

    public void Attack()
    {
        Collider[] targets = Physics.OverlapBox(splashZone.position, splashZone.localScale, splashZone.rotation, activeMask);

        if (targets.Length > 0)
        {
            ICanGetDamage bufer;

            foreach (Collider target in targets)
            {
                if (target.gameObject.TryGetComponent(out bufer))
                {
                    bufer.GetDamage(Damage, transform.forward);
                }
            }
        }
    }
}

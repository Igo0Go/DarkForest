using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ExplosionRune : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionParticles;

    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Instantiate(explosionParticles, transform.position, transform.rotation).GetComponent<Splash>().Damage = damage;
            Destroy(gameObject);
        }
    }
}

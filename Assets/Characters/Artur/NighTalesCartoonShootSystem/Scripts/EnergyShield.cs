using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnergyShield : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnDamage()
    {
        animator.SetTrigger("Damage");
    }
}

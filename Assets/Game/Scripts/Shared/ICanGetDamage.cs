using UnityEngine;

public interface ICanGetDamage
{
    void GetDamage(int damage);
    void GetDamage(int damage, Vector3 direction);
}

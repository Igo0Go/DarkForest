using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleObject : MonoBehaviour, ICanGetDamage
{
    [SerializeField]
    private GameObject defaultObject;
    [SerializeField]
    private GameObject afterDamageObject;
    [SerializeField]
    private UnityEvent afterDamage;
    [SerializeField]
    private bool splashOnly = false;
    [SerializeField]
    private List<Rigidbody> physicsPart;

    private const float forceFromSplash = 40;

    private void Awake()
    {
        defaultObject.SetActive(true); 
        afterDamageObject.SetActive(false);
    }

    public void GetDamage(int damage)
    {
        if(!splashOnly)
        {
            Destruct();
        }
    }

    public void GetDamage(int damage, Vector3 direction)
    {
        Destruct();
        if (physicsPart != null)
        {
            foreach (var part in physicsPart)
            {
                part.AddForce(direction.normalized * forceFromSplash, ForceMode.Impulse);
            }
        }
    }

    private void Destruct()
    {
        defaultObject.SetActive(false);
        afterDamageObject.SetActive(true);
        afterDamageObject.transform.parent = null;
        afterDamage.Invoke();
        Destroy(gameObject);
    }
}

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

    private void Awake()
    {
        defaultObject.SetActive(true); 
        afterDamageObject.SetActive(false);
    }

    public void GetDamage(int damage)
    {
        defaultObject.SetActive(false);
        afterDamageObject.SetActive(true);
        afterDamage.Invoke();
        Destroy(this);
    }
}

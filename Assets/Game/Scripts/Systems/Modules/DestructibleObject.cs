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

    private void Destruct()
    {
        defaultObject.SetActive(false);
        afterDamageObject.SetActive(true);
        afterDamageObject.transform.parent = null;
        afterDamage.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Splash"))
        {
            Destruct();
        }
    }
}

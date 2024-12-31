using UnityEngine;
using UnityEngine.Events;

public class InteractiveTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent action;

    public void Activate()
    {
        action.Invoke();
        Destroy(gameObject);
    }
}

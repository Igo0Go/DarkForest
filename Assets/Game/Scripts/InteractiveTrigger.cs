using UnityEngine;
using UnityEngine.Events;

public class InteractiveTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent action;
    [SerializeField]
    private bool destroy = true;

    public void Activate()
    {
        action.Invoke();
        if (destroy)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent afterTimeAction;
    [SerializeField, Min(0)]
    private float time;
    [SerializeField]
    private bool playOnAwake = false;

    void Start()
    {
        if(playOnAwake)
        {
            Activate();
        }
    }

    public void Activate()
    {
        StartCoroutine(TimerCoroutine());
    }
    public void Stop()
    {
        StopAllCoroutines();
    }

    private IEnumerator TimerCoroutine()
    {
        float t = 0;

        while (t < time)
        {
            t += Time.deltaTime;
            yield return null;
        }

        afterTimeAction.Invoke();
    }
}

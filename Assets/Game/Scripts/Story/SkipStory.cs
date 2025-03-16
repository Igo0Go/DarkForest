using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkipStory : MonoBehaviour
{
    [SerializeField]
    private GameObject tip;
    [SerializeField]
    private UnityEvent action;

    private void Awake()
    {
        tip.SetActive(false);
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            tip.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            action.Invoke();
        }
    }
}

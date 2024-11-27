using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TwoStateAnimatorController : MonoBehaviour
{
    [SerializeField] private string paramName = "Active";

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetValueForParam(bool value) => anim.SetBool(paramName, value);
}

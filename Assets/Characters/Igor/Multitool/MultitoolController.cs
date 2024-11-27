using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void FunctionWithDelay();

public class MultitoolController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string stageParamName = "Stage";
    [SerializeField] private string tool1ParamName = "Tool1";
    [SerializeField] private string tool2ParamName = "Tool2";
    [SerializeField, Range(1,10)] private float stagechangeSpeed = 1;

    [ContextMenu("Default")]
    public void ToDefault()
    {
        float delay = 0;

        if(anim.GetBool(tool1ParamName) || anim.GetBool(tool2ParamName))
        {
            SetTool(tool1ParamName, false, 0);
            SetTool(tool2ParamName, false, 0);

            delay = 1;
        }
        SetStage(0, delay);
    }

    [ContextMenu("Stage1")]
    public void ToFirst()
    {
        float delay = 0;

        if (anim.GetBool(tool1ParamName) || anim.GetBool(tool2ParamName))
        {
            SetTool(tool1ParamName, false, 0);
            SetTool(tool2ParamName, false, 0);

            delay = 1;
        }
        SetStage(1, delay);
    }

    [ContextMenu("Stage2")]
    public void ToSecond()
    {
        float delay = 0;

        if (anim.GetBool(tool1ParamName) || anim.GetBool(tool2ParamName))
        {
            SetTool(tool1ParamName, false, 0);
            SetTool(tool2ParamName, false, 0);

            delay = 1;
        }
        SetStage(2, delay);
    }

    [ContextMenu("Tool1")]
    public void UseTool1()
    {
        float delay = 0;
        if(anim.GetFloat(stageParamName) != 1)
        {
            ToFirst();
            delay = 1/stagechangeSpeed;
        }
        SetTool(tool1ParamName, true, delay);
    }

    [ContextMenu("Tool2")]
    public void UseTool2()
    {
        float delay = 0;
        if (anim.GetFloat(stageParamName) != 2)
        {
            ToSecond();
            delay = 2/stagechangeSpeed;
        }
        SetTool(tool2ParamName, true, delay);
    }


    public void SetStage(float value, float delay)
    {
        StartCoroutine(SetStageForAnimatorCoroutine(value, delay));
    }

    public void SetTool(string toolName, bool value, float delay)
    {
        StartCoroutine(SetToolForAnimatorCorroutine(toolName, value, delay));
    }

    private IEnumerator SetStageForAnimatorCoroutine(float targetStageValue, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        float currentValue = anim.GetFloat(stageParamName);

        if(currentValue > targetStageValue)
        {
            while(currentValue > targetStageValue)
            {
                currentValue -= Time.deltaTime * stagechangeSpeed;
                anim.SetFloat(stageParamName, currentValue);
                yield return null;
            }
        }
        else if (currentValue < targetStageValue)
        {
            while (currentValue < targetStageValue)
            {
                currentValue += Time.deltaTime * stagechangeSpeed;
                anim.SetFloat(stageParamName, currentValue);
                yield return null;
            }
        }
        currentValue = targetStageValue;
        anim.SetFloat(stageParamName, currentValue);
    }

    private IEnumerator SetToolForAnimatorCorroutine(string toolName, bool value, float delay)
    {
        yield return new WaitForSeconds(delay);

        anim.SetBool(toolName, value);
    }
}

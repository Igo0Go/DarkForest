using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveModule : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private Transform movingObject;
    [SerializeField, Min(0.01f)]
    private float speed = 1;
    [SerializeField]
    private AnimationCurve moveCurve;

    private float currentMoveValue = 0;

    private void Awake()
    {
        movingObject.position = startPoint.position;
    }

    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(OpenCoroutine());
    }
    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(CloseCoroutine());
    }

    private IEnumerator OpenCoroutine()
    {
        while (currentMoveValue < 1)
        {
            currentMoveValue += Time.deltaTime * speed;
            movingObject.position = Vector3.Lerp(startPoint.position,
                endPoint.position, moveCurve.Evaluate(currentMoveValue));

            yield return null;
        }

        movingObject.position = endPoint.position;
    }

    private IEnumerator CloseCoroutine()
    {
        while (currentMoveValue > 0)
        {
            currentMoveValue -= Time.deltaTime * speed;
            movingObject.position = Vector3.Lerp(startPoint.position,
                endPoint.position, moveCurve.Evaluate(currentMoveValue));

            yield return null;
        }

        movingObject.position = startPoint.position;
    }
}

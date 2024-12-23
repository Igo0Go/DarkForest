using System.Collections.Generic;
using UnityEngine;

public class LaserRenderer : MonoBehaviour
{
    [SerializeField]
    public List<Transform> controlPoints = null;
    [SerializeField]
    private LineRenderer centerRenderer = null;
    [SerializeField]
    private LineRenderer outlineRenderer = null;
    [SerializeField]
    private bool useWorldSpace;


    private void Start()
    {
        centerRenderer.positionCount = outlineRenderer.positionCount = controlPoints.Count;
    }

    void Update()
    {
        CheckLineRenderer();
    }

    private void CheckLineRenderer()
    {
        for (int i = 0; i < controlPoints.Count; i++)
        {
            if(useWorldSpace)
            {
                outlineRenderer.SetPosition(i, controlPoints[i].position);
                centerRenderer.SetPosition(i, controlPoints[i].position);
            }
            else
            {
                outlineRenderer.SetPosition(i, controlPoints[i].localPosition);
                centerRenderer.SetPosition(i, controlPoints[i].localPosition);
            }


        }
    }

}

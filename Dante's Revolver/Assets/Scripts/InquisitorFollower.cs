using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class InquisitorFollower : MonoBehaviour
{
    [SerializeField] public Transform[] _transformList = new Transform[2];
    List<Material> _materials;
    public void SetLineRenderer(LineRenderer _lineRendererObj)
    {
        //gameObject.GetComponent<LineRenderer>()
        gameObject.GetComponent<LineRenderer>().widthCurve = _lineRendererObj.widthCurve;
    }
    public void UpdateLineRenderer()
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0, _transformList[0].position);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, _transformList[1].position);
    }
    private void FixedUpdate()
    {
        UpdateLineRenderer();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class InquisitorFollower : MonoBehaviour
{
    private void Start()
    {
        gameObject.AddComponent<LineRenderer>();
    }

    public LineRenderer GetLineRenderer()
    {
        return gameObject.GetComponent<LineRenderer>();
    }
    private void FixedUpdate()
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(1, transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class InitInverted2DEdges : BaseBehavior
{
    public int NumEdges;
    public float Radius;

    [InspectorButton()]
    void Setup()
    {
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] points = new Vector2[NumEdges];

        for (int i = 0; i < NumEdges; i++)
        {
            float angle = 2 * Mathf.PI * i / NumEdges;
            float x = Radius * Mathf.Cos(angle);
            float y = Radius * Mathf.Sin(angle);

            points[i] = new Vector2(x, y);
        }
        edgeCollider.points = points;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MyTrailRenderer : MonoBehaviour
{
    Trail trail;

    void Update()
    {
        if (trail == null)
            NewTrail();

        trail.debugDraw = true;
        trail.Update();
    }

    void OnSegmentAdded()
    {
        //TODO
    }

    void OnSegmentRemoved()
    {

    }

    void NewTrail()
    {
        trail = new Trail(transform, 5, 0.2f);
        trail.onSegmentAdded += OnSegmentAdded;
        trail.onSegmentRemoved += OnSegmentRemoved;
    }
}

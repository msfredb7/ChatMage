using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAV_SmartSurface : NAV_SmartMover
{
    public Vector2 destinationPoint;

    public Vector2 WorldDestinationPoint { get { return (Vector2)transform.position + destinationPoint; } }

    public override Vector2 Smartify(RaycastHit2D hit)
    {
        return WorldDestinationPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 1, 1);

        Gizmos.DrawSphere(WorldDestinationPoint, 0.15f);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.EditorUtil;

public class NAV_SmartSurface : NAV_SmartMover
{
    public Vector2 borderPoint = Vector2.right * 0.5f;

    [ReadOnly]
    public Vector2 worldBorderPoint;

    public float direction;
    [ReadOnly]
    public Vector2 worldDirection;

    public override Vector2 Smartify(RaycastHit2D hit, Vector2 start, Vector2 end, float unitWidth)
    {
        return worldBorderPoint + worldDirection.normalized * (unitWidth + 0.25f);
    }

    private void Verif()
    {
        worldBorderPoint = transform.localToWorldMatrix.MultiplyPoint3x4(borderPoint);
        worldDirection = (direction + transform.rotation.eulerAngles.z).ToVector();
    }

    void OnDrawGizmosSelected()
    {
        Verif();

        Gizmos.color = new Color(0.5f, 0.5f, 1, 1);

        Gizmos.DrawSphere(worldBorderPoint, 0.15f);

        Gizmos.color = new Color(1, 0.5f, 0.5f, 1);
        Gizmos.DrawLine(worldBorderPoint, worldBorderPoint + worldDirection);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public bool canBeRotatedAndScaled = false;
    public bool willBeMoved = false;
    public Box2D area;
    [ReadOnly]
    public Box2D worldBounds;

    private Transform tr;

    void Awake()
    {
        tr = transform;
    }

    public bool OverlapsPoint(Vector2 point)
    {
        if (willBeMoved)
            Verif(tr);

        if (!canBeRotatedAndScaled)
        {
            return worldBounds.OverlapsPoint(point);
        }
        else
        {
            if (!worldBounds.OverlapsPoint(point))
                return false;

            Vector2 localPoint = point - (Vector2)tr.position; ///YOOOO
            Vector2 veryLocalPoint = tr.worldToLocalMatrix.MultiplyVector(localPoint);

            return area.OverlapsPoint(veryLocalPoint);
        }
    }

    void OnDrawGizmosSelected()
    {
        Verif(transform);

        //Bounds
        if (canBeRotatedAndScaled)
        {
            Gizmos.color = Color.white.ChangedAlpha(0.25f);
            Gizmos.DrawCube(worldBounds.Center, worldBounds.Size);
        }

        //Area
        Gizmos.color = Color.cyan.ChangedAlpha(0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(area.Center, area.Size);
    }

    void Verif(Transform tr)
    {
        if (canBeRotatedAndScaled)
        {
            //Set Bounds
            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxY = float.NegativeInfinity;
            float maxX = float.NegativeInfinity;

            Vector2[] corners = new Vector2[4];
            Matrix4x4 matrix = tr.localToWorldMatrix;
            corners[0] = matrix.MultiplyPoint3x4(area.min);
            corners[1] = matrix.MultiplyPoint3x4(new Vector3(area.min.x, area.max.y));
            corners[2] = matrix.MultiplyPoint3x4(area.max);
            corners[3] = matrix.MultiplyPoint3x4(new Vector3(area.max.x, area.min.y));

            for (int i = 0; i < corners.Length; i++)
            {
                if (corners[i].x < minX)
                    minX = corners[i].x;
                if (corners[i].x > maxX)
                    maxX = corners[i].x;
                if (corners[i].y < minY)
                    minY = corners[i].y;
                if (corners[i].y > maxY)
                    maxY = corners[i].y;
            }

            worldBounds = new Box2D(new Vector2(minX, minY), new Vector2(maxX, maxY));
        }
        else
        {
            tr.rotation = Quaternion.identity;
            if (tr.lossyScale != Vector3.one)
            {
                tr.localScale = Vector3.one;
                Debug.LogError("Le 'transform.scale' de la Box (" + gameObject.name + ")" + " devrais etre == (1,1,1)");
            }
            Vector2 position2D = tr.position;
            worldBounds = new Box2D(area.min + position2D, area.max + position2D);
        }

    }
}

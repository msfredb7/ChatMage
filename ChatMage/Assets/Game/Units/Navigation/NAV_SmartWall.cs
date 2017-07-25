using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NAV_SmartWall : NAV_SmartMover
{
    public Vector2 center = Vector2.zero;

    [ReadOnly]
    public Vector2 worldCenter;


    private Transform tr;
    private float xOnYScale;

    void Awake()
    {
        tr = transform;
        Vector2 lossyScale = tr.lossyScale;
        xOnYScale = lossyScale.x / lossyScale.y;
    }

    public override Vector2 Smartify(RaycastHit2D hit, Vector2 start, Vector2 end, float unitWidth)
    {
        bool sensHoraire = Vector3.Cross(end - start, worldCenter - start).z < 0;
        Vector2 localStart = tr.worldToLocalMatrix.MultiplyPoint3x4(start);

        float stretchExtract = 0.5f * Mathf.Sign(localStart.x);
        float stretchedX = (localStart.x - stretchExtract) * xOnYScale + stretchExtract;
        localStart.Set(stretchedX, localStart.y);

        Vector2 quadrantDirection = GetQuadrantDirection(localStart);
        if (!sensHoraire)
            quadrantDirection = -quadrantDirection;

        Vector2 boxCorner = GetQuadrantHeight(localStart) + quadrantDirection;

        //Un vecteur.one / 2
        Vector2 extra = boxCorner.Rotate(tr.rotation.eulerAngles.z);

        //On les transforme de local -> world
        boxCorner = tr.localToWorldMatrix.MultiplyVector(boxCorner);

        //On ajoute la largeur de la unit
        boxCorner += extra * unitWidth;

        Vector2 dest = boxCorner + (Vector2)tr.position;

        Vector2 v = dest - start;

        return start + v + (v.normalized * 0.4f);
    }

    Vector2 GetQuadrantHeight(Vector2 pos)
    {
        float x = pos.x;
        if (pos.y > x)
        {
            if (pos.y > -x)
                return Vector2.up / 2;   // 0
            else
                return Vector2.left / 2;      // 3
        }
        else
        {
            if (pos.y > -x)
                return Vector2.right / 2;    // 1
            else
                return Vector2.down / 2;    // 2
        }
    }
    Vector2 GetQuadrantDirection(Vector2 pos)
    {
        float x = pos.x;
        if (pos.y > x)
        {
            if (pos.y > -x)
                return Vector2.right / 2;   // 0
            else
                return Vector2.up / 2;      // 3
        }
        else
        {
            if (pos.y > -x)
                return Vector2.down / 2;    // 1
            else
                return Vector2.left / 2;    // 2
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 1, 1);

        Gizmos.DrawSphere(worldCenter, 0.15f);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(NAV_SmartWall))]
public class NAV_SmartWallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NAV_SmartWall wall = (target as NAV_SmartWall);
        wall.worldCenter = wall.transform.localToWorldMatrix.MultiplyPoint3x4(wall.center);
    }
}
#endif
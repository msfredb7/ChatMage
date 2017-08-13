using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;
using CCC.Math;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Box)), ExecuteInEditMode]
public class PositionDisplacer : BaseBehavior
{
    public enum DisplacementType { VectorToAdd, CrossBorder }

    public DisplacementType type;

    [InspectorShowIf("DispIsVectorToAdd")]
    public Vector2 displacement = Vector2.one;

    [InspectorShowIf("DispIsCrossBorder")]
    public Vector2 dir = Vector2.up;
    [InspectorShowIf("DispIsCrossBorder")]
    public Vector2 point = Vector2.right * 0.5f;

    protected override void Awake()
    {
        base.Awake();

        if (!Application.isPlaying)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.isLoaded)
            {
                Map map = scene.FindRootObject<Map>();
                if (map != null)
                {
                    if (!map.positionDisplacers.Contains(this))
                    {
                        map.positionDisplacers.Add(this);
                        Debug.Log("Added to map automatically...");
                    }
                }
                else
                {
                    Debug.LogWarning("Map non-existant, be sure to add this position displacer manually later.");
                }
            }
        }
    }

    void OnDestroy()
    {
        if (!Application.isPlaying)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.isLoaded)
            {
                Map map = scene.FindRootObject<Map>();
                if (map != null)
                {
                    map.positionDisplacers.Remove(this);
                    Debug.Log("Removed from map automatically...");
                }
            }
        }
    }

    [NotSerialized, fsIgnore]
    private Box box;

    public bool Displace(Vector2 position, float unitWidth, out Vector2 newPosition)
    {
        if (box == null)
            box = GetComponent<Box>();

        if (!box.OverlapsPoint(position))
        {
            newPosition = position;
            return false;
        }

        switch (type)
        {
            default:
            case DisplacementType.VectorToAdd:
                newPosition = displacement + position;
                break;
            case DisplacementType.CrossBorder:
                Vector2 p = LinAlg2D.GetClosestPointOnLine(position, dir, point + (Vector2)transform.position);
                Vector2 v = (p - position).normalized;
                newPosition = p + v * unitWidth;
                break;
        }

        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.2f, 0.8f);
        switch (type)
        {
            case DisplacementType.VectorToAdd:
                Gizmos.DrawLine(transform.position, transform.position + (Vector3)displacement);
                break;
            case DisplacementType.CrossBorder:
                Gizmos.DrawRay((point + (Vector2)transform.position) - dir * 5, dir * 10);
                break;
        }

        //Vector2 somePoint = Vector2.up;
        //Vector2 newPoint = Vector2.zero;
        //Displace(somePoint, 0.5f, out newPoint);
        //Gizmos.DrawSphere(newPoint, 0.5f);
    }

    bool DispIsVectorToAdd { get { return type == DisplacementType.VectorToAdd; } }
    bool DispIsCrossBorder { get { return type == DisplacementType.CrossBorder; } }
}

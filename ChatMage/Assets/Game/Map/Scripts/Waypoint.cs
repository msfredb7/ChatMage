using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Waypoint : BaseBehavior
{

    public enum WaypointType
    {
        PlayerSpawn = 0,
        items = 1,
        enemySpawn = 2,
        BossSpawn = 3,
        Other = 4,
    }
    
    public bool useTag = true;
    [InspectorHideIf("useTag")]
    public WaypointType type;
    [InspectorShowIf("useTag")]
    public string[] tags;

    public WaypointType Type { get { return type; } }

    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = TypeToColor();
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    Color TypeToColor()
    {
        if (!useTag)
            switch (type)
            {
                case WaypointType.PlayerSpawn:
                    return Color.blue;
                case WaypointType.items:
                    return Color.green;
                case WaypointType.enemySpawn:
                    return Color.red;
                case WaypointType.BossSpawn:
                    return Color.black;
                case WaypointType.Other:
                    return Color.white;
            }

        return Color.magenta;
    }
}

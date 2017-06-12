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

    private bool alreadyConverted = false;

    public WaypointType Type { get { return type; } }

    public Vector2 RawPosition { get { return transform.position; } }

    public Vector2 AdjustedPosition
    {
        get
        {
            if (!alreadyConverted)
                AdjustToMap();
            return RawPosition;
        }
    }

    private Waypoint AdjustToMap()
    {
        alreadyConverted = true;
        Game.instance.map.Adjust(gameObject);
        return this;
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

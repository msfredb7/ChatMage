using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public enum WaypointType
    {
        PlayerSpawn = 0,
        items = 1,
        enemySpawn = 2,
        BossSpawn = 3,
        Other = 4,
        Tags = 5,
    }

    [SerializeField]
    private WaypointType type;

    public bool alreadyConverted = false;

    public WaypointType GetWaypointType()
    {
        return type;
    }

    public Waypoint Convert()
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
            case WaypointType.Tags:
                return Color.yellow;
        }
        return Color.gray;
    }
}

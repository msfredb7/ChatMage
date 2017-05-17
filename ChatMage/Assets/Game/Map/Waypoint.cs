using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public enum WaypointType
    {
        PlayerSpawn = 0,
        items = 1,
        enemySpawn = 2,
        BossSpawn = 3
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
}

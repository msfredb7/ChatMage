using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    private enum WaypointType
    {
        items = 0,
        enemySpawn = 1,
        PlayerSpawn = 2,
        BossSpawn = 3
    }

    [SerializeField]
    private WaypointType type;
}

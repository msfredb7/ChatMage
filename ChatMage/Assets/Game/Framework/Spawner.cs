using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private Mapping map;

    public void Init()
    {
        map = Game.instance.map.mapping;
    }

    public Unit SpawnUnitAtRandomLocation(Unit unitPrefab, Waypoint.WaypointType spawnType, Action<Unit> function = null)
    {
        return SpawnUnitAtLocation(unitPrefab, map.GetRandomSpawnPoint(spawnType), function);
    }

    public Unit SpawnUnitAtLocation(Unit unitPrefab, Waypoint waypoint, Action<Unit> function = null)
    {
        return Game.instance.SpawnUnit(unitPrefab, new Vector2(waypoint.transform.position.x, waypoint.transform.position.y), function);
    }

    public List<Unit> SpawnUnitAtMultipleDefinedLocation(Unit unitPrefab, List<Waypoint> waypoint, Action<Unit> function = null)
    {
        List<Unit> units = new List<Unit>();
        for(int i = 0; i < waypoint.Count; i++)
        {
            units.Add(Game.instance.SpawnUnit(unitPrefab, new Vector2(waypoint[i].transform.position.x, waypoint[i].transform.position.y), function));
        }
        return units;
    }

    public List<Unit> SpawnUnitAtRandomMultipleDefinedLocation(Unit unitPrefab, List<Waypoint> waypoint, Action<Unit> function = null)
    {
        List<Unit> units = new List<Unit>();
        for (int i = 0; i < waypoint.Count; i++)
        {
            units.Add(SpawnUnitAtLocation(unitPrefab, waypoint[UnityEngine.Random.Range(0,waypoint.Count-1)], function));
        }
        return units;
    }

    public List<Unit> SpawnUnitAtRandomMultipleLocation(Unit unitPrefab, Waypoint.WaypointType spawnType, int amount, Action<Unit> function = null)
    {
        List<Waypoint> waypoints = map.GetRandomMultipleSpawnPoint(spawnType, amount);
        return SpawnUnitAtMultipleDefinedLocation(unitPrefab, waypoints, function);
    }

    /* AU BESOIN

    public void SpawnUnitAtLocationRandomRange(string unitName)
    {

    }

    public void SpawnUnitAtMultipleLocationRange(string unitName)
    {

    }

    public void SpawnUnitAtRandomMultipleLocationRange(string unitName)
    {

    }

        public void SpawnUnitAtLocationRange(string unitName, int indexStart, int indexEnd)
    {

    }

    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private Mapping map;

    public void Init()
    {
        Debug.Log("Spawner Init");
        map = Game.instance.map.map;
    }

    public Unit SpawnUnitAtRandomLocation(Unit unitPrefab, Waypoint.WaypointType spawnType)
    {
        return SpawnUnitAtLocation(unitPrefab, map.GetRandomSpawnPoint(spawnType));
    }

    public Unit SpawnUnitAtLocation(Unit unitPrefab, Waypoint waypoint)
    {
        return Game.instance.SpawnUnit(unitPrefab, new Vector2(waypoint.transform.position.x, waypoint.transform.position.y));
    }

    public List<Unit> SpawnUnitAtMultipleLocation(Unit unitPrefab, List<Waypoint> waypoint)
    {
        List<Unit> units = new List<Unit>();
        for(int i = 0; i < waypoint.Count; i++)
        {
            units.Add(Game.instance.SpawnUnit(unitPrefab, new Vector2(waypoint[i].transform.position.x, waypoint[i].transform.position.y)));
        }
        return units;
    }

    public List<Unit> SpawnUnitAtRandomMultipleLocation(Unit unitPrefab, Waypoint.WaypointType spawnType, int amount)
    {
        List<Waypoint> waypoints = map.GetRandomMultipleSpawnPoint(spawnType, amount);
        return SpawnUnitAtMultipleLocation(unitPrefab, waypoints);
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

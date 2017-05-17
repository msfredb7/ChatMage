using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private Mapping map;

    public void Init()
    {
        map = Game.instance.map.map;
    }

    public GameObject SpawnUnitAtRandomLocation(string unitName, Waypoint.WaypointType spawnType)
    {
        Vector3 position = map.GetRandomSpawnPoint(spawnType).transform.position;
        return Instantiate((Resources.Load(unitName,typeof(GameObject)) as GameObject), position, Quaternion.identity);
    }

    /* TODO : A FAIRE AVEC LA FACADE

    public GameObject SpawnUnitAtLocation(string unitName, Waypoint waypoint)
    {

    }

    public GameObject SpawnUnitAtMultipleLocation(string unitName, List<Waypoint> waypoint)
    {

    }

    public GameObject SpawnUnitAtRandomMultipleLocation(string unitName, List<Waypoint> waypoint)
    {

    }

    */

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

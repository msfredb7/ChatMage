using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptSpawnEnemy : MonoBehaviour {

    public GameObject enemyTest;
    private Mapping map;

	// Update is called once per frame
	void Update () {
        /* TODO: A refaire en utilisant le spawner
        map = Game.instance.map.map;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 position = map.GetRandomSpawnPoint(Waypoint.WaypointType.enemySpawn).transform.position;
            Vector3 direction = position - Game.instance.Player.transform.position;
            direction.Normalize();
            Instantiate(enemyTest,position,Quaternion.Euler(direction));
        }
        */
	}
}

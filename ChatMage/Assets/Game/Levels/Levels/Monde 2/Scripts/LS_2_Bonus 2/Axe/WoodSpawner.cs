using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawner : MonoBehaviour {

    public GameObject woodPrefab;
    public Transform spawnPoint;
    public SimpleEvent woodCut;
    public float spawnRate;

    private float countdown;
    private Wood currentWood;

	void Start ()
    {
        countdown = 0;
    }

    void Update()
    {
        if(currentWood == null)
        {
            if (countdown <= 0)
                SpawnWood();
        }

        if(Game.instance != null)
        {
            if (Game.instance.Player != null)
                countdown -= Game.instance.Player.vehicle.DeltaTime();
        }
    }

    private void SpawnWood()
    {
        currentWood = Instantiate(woodPrefab, spawnPoint).GetComponent<Wood>();

        currentWood.onDeath += delegate (Unit unit)
        {
            woodCut.Invoke();
            countdown = spawnRate;
        };
    }
}

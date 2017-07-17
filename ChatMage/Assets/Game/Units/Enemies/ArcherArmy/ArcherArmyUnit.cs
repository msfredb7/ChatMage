using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArmyUnit : Unit {

    public ArcherArrow arrowPrefab;
    public string unitSpawnTag;
    public float cooldown;
    public float minAngle = 45;
    public float maxAngle = 135;

    public bool debug = false;

    private float countdown;

    void Start ()
    {
        countdown = 0;
        if(debug)
            Debug.Log("Archer Army Spawn");
    }

    protected override void Update ()
    {
        base.Update();
        if(countdown < 0)
        {
            Shoot();
        }
        countdown -= DeltaTime();
	}

    private void Shoot()
    {
        List<UnitSpawn> spawnLocations = Game.instance.map.mapping.GetSpawns_NewList(unitSpawnTag);

        int randomIndex = Random.Range(0, spawnLocations.Count - 1);
        ArcherArrow currentArrow = spawnLocations[randomIndex].SpawnUnit(arrowPrefab);
        
        if(Game.instance.Player != null)
        {   // Si on a un joueur, on envoie les fleches vers lui
            if(debug)
                Debug.Log("Arrow Spawned Towards Player"); 
            Vector2 angleTowardsPlayer = new Vector3(Game.instance.Player.vehicle.Position.x, Game.instance.Player.vehicle.Position.y, 0) - spawnLocations[randomIndex].transform.position;
            currentArrow.Init(angleTowardsPlayer);
        }
        else
        {
            if(debug)
                Debug.Log("Arrow Spawned Randomly");
            currentArrow.Init(Vehicle.AngleToVector(Random.Range(45, 135))); // Sinon c'est random
        }
       
        currentArrow.Init(this);

        countdown = cooldown;
    }
}

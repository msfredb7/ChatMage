using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class ArcherArmyUnit : Unit
{
    public ArcherArrow arrowPrefab;
    public string unitSpawnTag;
    public float cooldown;

    [Header("Targets"), Forward]
    public Targets targets;

    [Header("If no target")]
    public float minAngle = 45;
    public float maxAngle = 135;

    public bool debug = false;

    private float countdown;

    void Start()
    {
        countdown = 0;
        if (debug)
            Debug.Log("Archer Army Spawn");
    }

    protected override void Update()
    {
        base.Update();

        if (countdown <= 0)
        {
            Shoot();
        }
        countdown -= DeltaTime();
    }

    protected override void Die()
    {
        base.Die();
        Destroy();
    }

    private void Shoot()
    {
        ReadOnlyCollection<UnitSpawn> spawnLocations = Game.Instance.map.mapping.GetSpawns(unitSpawnTag);

        int randomIndex = Random.Range(0, spawnLocations.Count - 1);
        ArcherArrow currentArrow = spawnLocations[randomIndex].SpawnUnit(arrowPrefab);

        Vector2 dir = Vector2.zero;

        PlayerVehicle player = Game.Instance.Player.vehicle;
        if (player != null)
        {   // Si on a un joueur, on envoie les fleches vers lui
            if (debug)
                Debug.Log("Arrow Spawned Towards Player");

            dir = player.Position - (Vector2)currentArrow.transform.position;
        }
        else
        {
            if (debug)
                Debug.Log("Arrow Spawned Randomly");

            dir = Vehicle.AngleToVector(Random.Range(minAngle, maxAngle));
        }

        currentArrow.Init(this, dir, targets);

        countdown = cooldown;
    }
}

using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Wormhole : Item
{
    [InspectorHeader("Linking")]
    public Wormhole wormholePrefab;
    private Wormhole wormholeSpawned;

    [InspectorHeader("Settings")]
    public float cooldown;

    // Variable to verify turning
    private PlayerDriver playerInput;
    private Vehicle playerVehicle;
    private bool wasTurning = false;

    // Compteur
    private float countdown = 0;

    // Circle, Spawnpoint, etc.
    private Vector2 initialPos;
    public float rayon;

    public override void OnGameReady()
    {
        playerInput = Game.instance.Player.playerDriver;
        playerVehicle = Game.instance.Player.vehicle;
        wasTurning = false;
        countdown = 0;
    }

    public override void OnGameStarted()
    {
        cooldown *= Game.instance.Player.playerStats.cooldownMultiplier;
    }

    public override void OnUpdate()
    {
        // Si le vehicule est en train de tourner
        if (playerInput.LastHorizontalInput != 0)
        {
            // Et qu'on etait deja en train de tourner
            if (wasTurning)
            {
                // TODO FAIRE UN TRIGGER EXIT QUE ON PEUT SORTIR APRES UN COOLDOWN ET LE WORMOLE VA SE FAIRE
                // Si son temps est termine
                countdown -= player.vehicle.DeltaTime();
            } else // Et qu'on etait pas deja en train de tourner
            {
                // Spawn le wormhole
                wasTurning = true;
                initialPos = playerVehicle.Position;
                StartWormhole(playerInput.LastHorizontalInput);
            }
        } else // Si le vehicule est pas en train de tourner
        {
            if (wasTurning) // Et qu'on etait en train de tourner
            {
                wasTurning = false;
                DestroyWormhole();
            }
        }
    }

    // TODO: A REFAIRE POUR l'ANGLE
    private void StartWormhole(float input)
    {
        countdown = cooldown;
        // Si on tourne a gauche
        if (input < 0)
        {
            // Si on va vers le haut
            if (0 < playerVehicle.targetDirection && playerVehicle.targetDirection < 180)
            {
                // Spawn le wormhole a gauche du vehicule
                SpawnWormhole(CalculatePosition(-197.5f));
            }
            else // Si on va vers le bas
            {
                // Spawn le wormhole a droite du vehicule
                SpawnWormhole(CalculatePosition(197.5f));
            }
        } // Si on tourne a droite
        else if (input > 0)
        {
            // Si on va vers le haut
            if (0 < playerVehicle.targetDirection && playerVehicle.targetDirection < 180)
            {
                // Spawn le wormhole a droite du vehicule
                SpawnWormhole(CalculatePosition(-197.5f));
            }
            else // Si on va vers le bas
            {
                // Spawn le wormhole a gauche du vehicule
                SpawnWormhole(CalculatePosition(197.5f));
            }
        }
        else
            return;
    }

    Vector2 CalculatePosition(float angle)
    {
        var x = rayon * Mathf.Cos(angle * Mathf.Deg2Rad);
        var y = rayon * Mathf.Sin(angle * Mathf.Deg2Rad);
        var newPosition = initialPos;
        newPosition.x += x;
        newPosition.y += y;
        return newPosition;
    }

    void SpawnWormhole(Vector2 position)
    {
        Debug.Log("Spawing Wormhole");
        wormholeSpawned = Game.instance.SpawnUnit(wormholePrefab, position);
        //wormholeSpawned.zone.onTriggerExit += Zone_onTriggerExit; ;
    }

    private void Zone_onTriggerExit(ColliderInfo other, ColliderListener listener)
    {
        
    }

    void DestroyWormhole()
    {
        if(wormholeSpawned != null)
        {
            Debug.Log("Destroying Wormhole");
            Destroy(wormholeSpawned.gameObject);
        }
    }

    bool ComparePositions(float gap)
    {
        if (Vector2.Distance(playerVehicle.Position, initialPos) < gap)
            return true;
        else
            return false;
    }
}

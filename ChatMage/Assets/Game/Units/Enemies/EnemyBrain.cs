using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Brain, with vehicle type T
/// </summary>
/// <typeparam name="T">Vehicle Type</typeparam>
public abstract class EnemyBrain<T> : MonoBehaviour where T :EnemyVehicle
{

    protected T vehicle;
    protected PlayerController player;

    protected virtual void Start()
    {
        vehicle = GetComponent<T>();
        if (vehicle == null)
            Debug.LogError("Could not find vehicle of type: " + typeof(T) + ".");
        player = Game.instance.Player;
        vehicle.Stop();
    }
}

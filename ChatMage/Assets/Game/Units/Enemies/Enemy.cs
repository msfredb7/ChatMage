using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public EnemyBrain brain;
    public EnemyVehicle vehicle;

    void Start()
    {
        brain = GetComponent<EnemyBrain>();
        vehicle = GetComponent<EnemyVehicle>();
    }
}

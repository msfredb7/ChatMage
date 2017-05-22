using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour {

    protected Enemy mySelf;
    protected PlayerController player;

    protected virtual void Start()
    {
        mySelf = GetComponent<Enemy>();
        player = Game.instance.Player;
        mySelf.vehicle.Idle();
    }
}

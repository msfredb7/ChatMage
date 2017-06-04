using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : Vehicle
{
    PlayerController controller;
    void Init(PlayerController controller)
    {
        this.controller = controller;
    }

    public void Kill()
    {
        Die();
    }

    protected override void Die()
    {
        base.Die();

        //Death animation
        Destroy(gameObject);
    }
}

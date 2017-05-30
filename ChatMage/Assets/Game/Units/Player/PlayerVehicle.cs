using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : Vehicle
{
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

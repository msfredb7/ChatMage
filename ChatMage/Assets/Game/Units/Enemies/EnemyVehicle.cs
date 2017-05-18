using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVehicle : Vehicle {

    public void Goto(Vector2 position)
    {
        targetDirection = VectorToAngle(position - new Vector2(transform.position.x, transform.position.y));
    }

    public void Teleport(Vector2 position)
    {
        Teleport(position);
    }

    public void FleePlayer()
    {
        if(Game.instance.Player.transform.position.x > 8)
        {
            if (Game.instance.Player.transform.position.y > 4.5f)
            {
                Goto(new Vector2(0, 0));
            } else
            {
                Goto(new Vector2(0, 9));
            }
        } else
        {
            if (Game.instance.Player.transform.position.y > 4.5f)
            {
                Goto(new Vector2(16, 0));
            }
            else
            {
                Goto(new Vector2(16, 9));
            }
        }
    }

    public void ChargePlayer()
    {
        if(Game.instance.Player != null && Game.instance.Player.GetComponent<PlayerStats>().isVisible)
            Goto(Game.instance.Player.transform.position);
    }

    public void Idle()
    {
        Goto(transform.position);
    }
}

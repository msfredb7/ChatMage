using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVehicule : EnemyVehicle {

    public void Hit(Unit unit)
    {
        if (unit.gameObject == Game.instance.Player.gameObject)
        {
            Destroy(gameObject);
            Game.instance.Player.playerStats.Hit();
        }
    }

    public void SetDestination()
    {
        GotoDirection(VectorToAngle(GetPlayerPosition() - GetPosition()));
    }

    Vector2 GetPlayerPosition()
    {
        return new Vector2(Game.instance.Player.transform.position.x, Game.instance.Player.transform.position.y);
    }

    Vector2 GetPosition()
    {
        return new Vector2(tr.position.x, tr.position.y);
    }

}

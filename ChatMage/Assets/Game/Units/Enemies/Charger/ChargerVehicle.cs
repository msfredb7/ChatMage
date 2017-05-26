using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerVehicle : EnemyVehicle
{
    public void Init()
    {
        SetBounds(Game.instance.ScreenBounds, 1);
        GetComponent<CollisionListener>().onEnter += Hit;
    }

	public void Hit(Unit unit)
    {
        if(unit.gameObject == Game.instance.Player.gameObject)
            Game.instance.Player.GetComponent<PlayerStats>().Hit();
        Destroy(gameObject);
    }

    public void ChargePlayer()
    {
        if (Game.instance.Player != null && Game.instance.Player.GetComponent<PlayerStats>().isVisible)
            GotoPosition(Game.instance.Player.transform.position);
    }
}

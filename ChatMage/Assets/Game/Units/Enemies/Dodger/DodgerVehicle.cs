using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerVehicle : EnemyVehicle {

    private Vector2 startingPosition;
    public GameObject visualAspect; // sprite qui regarde vers ou on tire

    public void Init()
    {
        startingPosition = transform.position;
        SetBounds(Game.instance.ScreenBounds, 1);
    }

    public void Hit(Unit unit)
    {
        if (unit.gameObject == Game.instance.Player.gameObject)
        {

            Destroy(gameObject);
        }
    }

    public void DodgeLeft()
    {
        GotoDirection(VectorToAngle(GetPlayerPosition() - GetPosition()) + 90);
    }

    public void DodgeRight()
    {
        GotoDirection(VectorToAngle(GetPlayerPosition() - GetPosition()) - 90);
    }

    public void LookAtPlayer()
    {
        // Rotation du sprite vers le joueur
        visualAspect.transform.rotation = Quaternion.Euler(0,0,VectorToAngle(GetPlayerPosition() - GetPosition()));
    }

    public Vector2 GetPlayerPosition()
    {
        return new Vector2(Game.instance.Player.transform.position.x, Game.instance.Player.transform.position.y);
    }

    public Vector2 GetPosition()
    {
        return new Vector2(tr.position.x, tr.position.y);
    }
}

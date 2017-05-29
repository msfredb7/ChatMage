using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChomp : MonoBehaviour
{
    public Rigidbody2D anchor;

    private Transform target;

    public void Init(Transform target)
    {
        Game.instance.Player.vehicle.onTeleportPosition += OnPlayerTeleport;

        this.target = target;
    }

    void FixedUpdate()
    {
        if (target != null)
            anchor.MovePosition(target.position);
    }

    void OnPlayerTeleport(Unit player, Vector2 delta)
    {
        Debug.Log("player teleported!");
    }
}
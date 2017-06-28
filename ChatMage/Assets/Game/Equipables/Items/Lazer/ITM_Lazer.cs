using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Lazer : Item
{
    public LazerController lazerPrefab;
    private LazerController lazer;

    public LayerMask layer;

    private bool lazerAlreadyShoot = false;

    public override void OnGameReady()
    {
        lazerAlreadyShoot = false;
    }

    public override void OnGameStarted()
    {
        
    }

    public override void OnUpdate()
    {
        PlayerController player = Game.instance.Player;
        RaycastHit2D hit = Physics2D.Raycast(player.vehicle.transform.position, player.vehicle.WorldDirection2D(),Mathf.Infinity,layer);
        if (hit.collider != null)
        {
            if(lazer == null && !lazerAlreadyShoot)
            {
                lazer = Instantiate(lazerPrefab, Game.instance.Player.vehicle.transform);
                lazer.onComplete += delegate () { lazerAlreadyShoot = false; };
                lazerAlreadyShoot = true;
            }
        } else
        {
            if (lazer != null)
            {
                lazer.Delete();
                lazerAlreadyShoot = false;
            }
        }
    }
}

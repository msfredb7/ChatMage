using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Tambour : Item
{
    public GameObject impactCircle;
    public float impactCooldown;

    private float timer;

    public override void Init(PlayerController player)
    {
        base.Init(player);
    }

    public override void OnGameReady()
    {

    }

    public override void OnGameStarted()
    {
        timer = impactCooldown;
    }

    public override void OnUpdate()
    {
        if(timer < 0)
        {
            Impact();
        }
        timer -= Time.deltaTime;
    }

    void Impact()
    {
        GameObject newImpact = Instantiate(impactCircle);
        Game.instance.gameCamera.vectorShaker.Hit(-player.vehicle.WorldDirection2D().normalized * player.playerStats.onHitShakeStrength);
        timer = impactCooldown;
    }
}

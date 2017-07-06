using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Tambour : Item
{
    public GameObject impactCircle;
    public float impactCooldown;
    public float cameraShakeStrength = 0.2f;

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
        timer -= player.vehicle.DeltaTime();
    }

    void Impact()
    {
        Instantiate(impactCircle, player.body.transform);
        Game.instance.gameCamera.vectorShaker.Shake(cameraShakeStrength);
        timer = impactCooldown;
    }
}

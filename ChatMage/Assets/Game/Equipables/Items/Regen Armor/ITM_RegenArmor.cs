using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_RegenArmor : Item
{
    public float shieldCooldown;
    private float timer;
    private bool shieldDown;

    public override void Init(PlayerController player)
    {
        base.Init(player);
        player.playerStats.onHit += ResetShield;
        player.playerStats.armor++;
        shieldDown = false;
        timer = -1;
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
        if(timer < 0)
        {
            if (shieldDown)
            {
                player.playerStats.armor++;
                shieldDown = false;
            }
        }
        timer -= Time.deltaTime;
    }

    void ResetShield()
    {
        if(!shieldDown)
            timer = shieldCooldown;
        shieldDown = true;
    }
}
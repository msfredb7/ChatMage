using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Mine : Item {

    public GameObject minePrefab;
    public float cooldown;
    private float countdown;

    public override void OnGameReady()
    {
        
    }

    public override void OnGameStarted()
    {
        cooldown = cooldown * Game.instance.Player.playerStats.cooldownReduction;
    }

    public override void OnUpdate()
    {
        if (countdown < 0)
        {
            LaunchBomb();
        }
        countdown -= Time.deltaTime;
    }

    void LaunchBomb()
    {
        countdown = cooldown;
        GameObject mine = Instantiate(minePrefab);
        new Mine(mine);
    }
}

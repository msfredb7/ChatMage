using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_SmashShield : Item {

    public GameObject shieldPrefab;
    private GameObject shield;

    public float limitCooldown = 10;
    private float counter;
    private bool startCounter;

    public override void OnGameReady()
    {
        startCounter = false;
        counter = 0;
    }

    public override void OnGameStarted()
    {
        player.playerSmash.onSmashGained += PlayerSmash_onSmashGained;
        player.playerSmash.onSmashStarted += PlayerSmash_onSmashStarted;
    }

    private void PlayerSmash_onSmashGained()
    {
        shield = Instantiate(shieldPrefab,player.body.transform);
        player.playerStats.damagable = false;

        startCounter = true;
        counter = limitCooldown;
    }

    private void PlayerSmash_onSmashStarted()
    {
        Destroy(shield);
        player.playerStats.damagable = true;

        startCounter = false;
        counter = 0;
    }

    public override void OnUpdate()
    {
        if(counter < 0 && startCounter)
        {
            PlayerSmash_onSmashStarted();
        }
        counter -= player.vehicle.DeltaTime();
    }
}

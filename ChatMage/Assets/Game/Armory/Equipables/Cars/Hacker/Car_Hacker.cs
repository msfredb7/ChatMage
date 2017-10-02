using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Hacker : StdCar
{
    public HackerCarZone zonePrefab;
    public float boostedAOESizeMultiplier = 1.5f;

    public override void OnGameReady()
    {
        base.OnGameReady();

        //Il faut faire ceci dans le OnGameReady et non le Init() parce qu'on veut etre sur que stats.boosterAOE se fasse avant.
        HackerCarZone zone = Instantiate(zonePrefab.gameObject, player.transform).GetComponent<HackerCarZone>();
        if (player.playerStats.boostedAOE)
            zone.SetSizeMultiplier(boostedAOESizeMultiplier);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_BombThrower : Item
{
    public float cooldown;
    public GameObject yourBombPrefab;
    public float bombSpeed = 1;
    public float explosionForce = 1;
    public int amountOfBumps = 3;

    private float countdown;

    public override void OnGameReady()
    {
        countdown = cooldown;
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
        if(countdown < 0)
        {
            LaunchBomb();
        }
        countdown -= Time.deltaTime;
    }

    void LaunchBomb()
    {
        countdown = cooldown;
        GameObject bomb = Instantiate(yourBombPrefab);
        Bomb myBomb = new Bomb(bomb, amountOfBumps);
        myBomb.Throw(new Vector2(UnityEngine.Random.Range(-5,5) + myBomb.movingBomb.Position.x, UnityEngine.Random.Range(-5, 5) + myBomb.movingBomb.Position.y)
            ,myBomb.movingBomb.Position, bombSpeed
            ,explosionForce);
    }
}

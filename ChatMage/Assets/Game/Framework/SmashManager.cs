using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashManager : MonoBehaviour
{
    [SerializeField]
    private SmashBall ballPrefab;
    [SerializeField]
    private float totalCooldown;

    [System.NonSerialized]
    private SmashBall currentSmashBall;

    private float remainingTime = 0;
    private bool inCooldown = true;

    public void DecreaseCooldown(float amount)
    {
        remainingTime -= amount;
    }

    public bool IsInCooldown { get { return inCooldown; } }

    public float RemainingTime { get { return remainingTime; } }

    void Start()
    {
        Game.instance.onGameStarted.AddListener(OnGameStarted);
        enabled = false;
    }

    void OnGameStarted()
    {
        enabled = true;
        ResetCooldown();
    }

    void ResetCooldown()
    {
        float multiplier = 1;
        if (Game.instance.Player != null)
            multiplier = Game.instance.Player.playerStats.smashCooldownRate;
        remainingTime = totalCooldown * multiplier;

        inCooldown = true;
    }

    void Update()
    {
        //On ne diminue pas le cooldown si une smash ball est en vie
        if (!inCooldown)
            return;

        float multiplier = 1;
        if (Game.instance.Player != null)
            multiplier = Game.instance.Player.vehicle.TimeScale * Game.instance.Player.playerStats.smashRefreshRate;
        remainingTime -= Time.deltaTime * multiplier;

        if (remainingTime <= 0)
            SpawnSmashBall();
    }

    private void SpawnSmashBall()
    {
        inCooldown = false;
        Vector2 borders = Game.instance.ScreenBounds;
        Vector2 spawnPoint = new Vector2(Random.Range(0, borders.x), Random.Range(0, borders.y));

        currentSmashBall = Game.instance.SpawnUnit(ballPrefab, spawnPoint) as SmashBall;

        currentSmashBall.onDeath += OnSmashTaken;
    }

    private void OnSmashTaken(Unit smashUnit)
    {
        Game.instance.Player.playerSmash.GainSmash();
        Game.instance.Player.playerSmash.onSmashUsed += OnSmashUsed;
    }

    private void OnSmashUsed()
    {
        ResetCooldown();
    }
}

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

    void Start()
    {
        Game.instance.onGameStarted.AddListener(OnGameStarted);
    }

    void OnGameStarted()
    {
        ResetCooldown();
    }

    void ResetCooldown()
    {
        float multiplier = 1;
        if (Game.instance.Player != null)
            multiplier = Game.instance.Player.playerStats.smashCooldownRate;
        remainingTime = totalCooldown * multiplier;
    }

    void Update()
    {
        if (!Game.instance.gameStarted)
            return;

        float timeScale = 1;
        if (Game.instance.Player != null)
            timeScale = Game.instance.Player.vehicle.TimeScale;
        remainingTime -= Time.deltaTime * timeScale;

        if (remainingTime <= 0)
            SpawnSmashBall();
    }

    public void SHITNIGGA()
    {
        SpawnSmashBall();
    }

    private void SpawnSmashBall()
    {
        Vector2 borders = Game.instance.ScreenBounds;
        Vector2 spawnPoint = new Vector2(Random.Range(0, borders.x), Random.Range(0, borders.y));

        currentSmashBall = Game.instance.SpawnUnit(ballPrefab, spawnPoint) as SmashBall;

        //Reset
        ResetCooldown();
    }
}

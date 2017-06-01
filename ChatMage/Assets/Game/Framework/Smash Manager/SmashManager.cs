using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashManager : MonoBehaviour
{
    [SerializeField]
    private SmashBall ballPrefab;
    [SerializeField]
    private float totalCooldown;
    [SerializeField]
    private Animator followTarget;
    [SerializeField]
    private Transform followTargetParent;

    [System.NonSerialized]
    private SmashBall currentSmashBall;

    private float remainingTime = 0;
    private bool inCooldown = true;

    public void DecreaseCooldown(float amount)
    {
        remainingTime -= amount;
    }

    public SmashBall CurrentSmashBall { get { return currentSmashBall; } }

    public bool IsInCooldown { get { return inCooldown; } }

    public float RemainingTime { get { return remainingTime; } }

    void Start()
    {
        Game.instance.onGameStarted += OnGameStarted;
        Game.instance.onGameReady += OnGameReady;
        enabled = false;

        if (followTargetParent != null)
            followTargetParent.gameObject.SetActive(false);
    }

    void OnGameReady()
    {
        if (followTarget != null)
        {
            followTargetParent.localScale = Game.instance.gameCamera.AdjustVector(Vector3.one);
            followTargetParent.transform.localPosition = Game.instance.gameCamera.AdjustVector(followTargetParent.transform.localPosition);
        }
    }

    void OnGameStarted()
    {
        enabled = Game.instance.Player.playerSmash.SmashEquipped;
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
        Vector2 borders = Game.instance.gameCamera.ScreenSize;
        Vector2 spawnPoint = new Vector2(Random.Range(0, borders.x), Random.Range(0, borders.y));

        currentSmashBall = Game.instance.SpawnUnit(ballPrefab, spawnPoint) as SmashBall;

        currentSmashBall.onDeath += OnSmashTaken;

        if (followTargetParent != null)
        {
            followTargetParent.gameObject.SetActive(true);
            currentSmashBall.followTarget = followTarget.transform;
        }
    }

    private void OnSmashTaken(Unit smashUnit)
    {
        if (followTargetParent != null)
            followTargetParent.gameObject.SetActive(false);

        Game.instance.Player.playerSmash.GainSmash();
        Game.instance.Player.playerSmash.onSmashUsed += OnSmashUsed;
    }

    private void OnSmashUsed()
    {
        ResetCooldown();
    }
}

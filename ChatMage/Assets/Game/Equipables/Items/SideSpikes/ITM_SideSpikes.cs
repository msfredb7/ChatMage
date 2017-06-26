using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ITM_SideSpikes : Item
{
    [InspectorHeader("Linking")]
    public GameObject spikeVisuals;
    public GameObject killEffect;

    [InspectorHeader("Settings")]
    public Vector2 collidersScale = new Vector2(1, 1.5f);
    public Vector2 boostedCollidersScale = new Vector2(1, 2);
    public bool scaleCollisioners = false;
    public bool scaleTriggers = true;
    public float smashCooldownDecreaseByKill = 5;

    public override void OnGameReady()
    {
        PlayerCarTriggers car = player.playerCarTriggers;

        Vector2 scale = collidersScale;
        float visualScale = 1;
        if (player.playerStats.boostedAOE)
        {
            scale = boostedCollidersScale;
            visualScale = (boostedCollidersScale.y - 1) / (collidersScale.y - 1);
        }


        //On ajuste la grosseur des triggers
        if (scaleCollisioners)
        {
            car.rightCol.transform.localScale = Vector2.Scale(car.rightCol.transform.localScale, scale);
            car.leftCol.transform.localScale = Vector2.Scale(car.leftCol.transform.localScale, scale);
        }
        if (scaleTriggers)
        {
            car.rightTrig.transform.localScale = Vector2.Scale(car.rightTrig.transform.localScale, scale);
            car.leftTrig.transform.localScale = Vector2.Scale(car.leftTrig.transform.localScale, scale);
        }


        //On place les spikes
        SpawnSpike(visualScale, -90, player.playerLocations.RightDoor);
        SpawnSpike(visualScale, 90, player.playerLocations.LeftDoor);

        car.onUnitKilled += Car_onUnitKilled;
    }

    private void SpawnSpike(float horizontalScale, float angle, Vector2 localPos)
    {
        Transform spike = Instantiate(spikeVisuals, player.body).transform;
        spike.localScale = new Vector2(horizontalScale, 1);
        spike.localPosition = localPos;
        spike.localRotation = Quaternion.Euler(Vector3.forward * angle);
    }

    private void Car_onUnitKilled(Unit unit, CarSide carTrigger, ColliderInfo other, ColliderListener listener)
    {
        switch (carTrigger)
        {
            case CarSide.Right:
                OnKillUnitWithSpikes(Vector2.one);
                break;
            case CarSide.Left:
                OnKillUnitWithSpikes(Vector2.one);
                break;
        }
    }

    private void OnKillUnitWithSpikes(Vector2 worldPosition)
    {
        Game.instance.smashManager.DecreaseCooldown(smashCooldownDecreaseByKill);

        if (killEffect != null)
            Instantiate(killEffect, worldPosition, Quaternion.identity);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }
}

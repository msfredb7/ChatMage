using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonyVehicle : EnemyVehicle
{
    [Header("Tony")]
    public TonyAnimator animator;
    public Unit rewardPrefab;
    public Transform zoneTransform;
    public SimpleColliderListener zoneListener;

    [Header("Teleport")]
    public float teleportWhenBelow = 10;

    public Action onSnapTaken;

    private GameCamera cam;

    void Start()
    {
        zoneListener.onTriggerEnter += ZoneListener_onTriggerEnter;
    }

    protected override void Update()
    {
        base.Update();

        if(cam == null)
            cam = Game.instance.gameCamera;
        else
        {
            float distToCam = cam.Height - Position.y;
            if(distToCam.Abs() > teleportWhenBelow)
            {
                TeleportPosition(cam.ClampToScreen(Position, 1.2f));
            }
        }
    }

    private void ZoneListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit is PlayerVehicle)
        {
            //Reward player
            Game.instance.SpawnUnit(rewardPrefab, zoneTransform.position);

            if (onSnapTaken != null)
                onSnapTaken();
        }
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        return 1;
    }
}

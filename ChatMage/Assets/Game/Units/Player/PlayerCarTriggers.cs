using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarSide { Front = 0, Back = 1, Right = 2, Left = 3 }

public class PlayerCarTriggers : PlayerComponent
{
    public delegate void UnitDetectionEvent(Unit unit, CarSide carTrigger, ColliderInfo other, ColliderListener listener);
    public event UnitDetectionEvent onUnitHit;
    public event UnitDetectionEvent onUnitKilled;

    [Header("Hit Animation")]
    public float camHitStrengthOnHit = 0.05f;


    [Header("Trigger Listeners")]
    public MultipleColliderListener masterTriggerListener;
    public ColliderListener frontTrig;
    public ColliderListener backTrig;
    public ColliderListener rightTrig;
    public ColliderListener leftTrig;

    [Header("Collision Listeners")]
    public MultipleColliderListener masterCollisionListener;
    public ColliderListener frontCol;
    public ColliderListener backCol;
    public ColliderListener rightCol;
    public ColliderListener leftCol;

    void Awake()
    {
        masterTriggerListener.onTriggerEnter += MasterTriggerListener_onTriggerEnter;

        masterCollisionListener.onCollisionEnter += MasterCollisionListener_onCollisionEnter;
    }

    private void MasterCollisionListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        Unit unit = other.parentUnit;
        if (unit == null)
            return;
        if (unit.allegiance != Allegiance.Enemy
            && unit.allegiance != Allegiance.SmashBall)
            return;

        CarSide side = ColliderToSide(listener);

        //Damage the enemy
        IAttackable attackable = unit.GetComponent<IAttackable>();
        if (attackable == null)
            return;

        HitUnit(unit, attackable, side, other, listener);
    }

    private void MasterTriggerListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)//Unit unit, RemoteTriggerListener source, GameObject other)
    {
        Allegiance al = other.parentUnit.allegiance;
        if (al == Allegiance.Ally)
            return;

        CarSide trigger = TriggerToSide(listener);

        //Damage the enemy
        IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
        if (attackable == null)
            return;

        HitUnit(other.parentUnit, attackable, trigger, other, listener);
    }

    private void HitUnit(Unit unit, IAttackable attackable, CarSide side, ColliderInfo other, ColliderListener listener)
    {
        //Event
        if (onUnitHit != null)
            onUnitHit.Invoke(unit, side, other, listener);

        int damage = 0;
        switch (side)
        {
            case CarSide.Front:
                damage = controller.playerStats.frontDamage;
                break;
            case CarSide.Back:
                damage = controller.playerStats.backDamage;
                break;
            case CarSide.Right:
                damage = controller.playerStats.rightDamage;
                break;
            case CarSide.Left:
                damage = controller.playerStats.leftDamage;
                break;
        }
        damage *= controller.playerStats.damageMultiplier;
        if (damage > 0)
        {
            if (attackable.Attacked(other, damage, controller.vehicle, listener.info) <= 0)
            {
                //Unit killed !
                controller.playerStats.RegisterKilledUnit(unit);

                //Event
                if (onUnitKilled != null)
                    onUnitKilled(unit, side, other, listener);
            }

            //Camera shake!
            Game.instance.gameCamera.vectorShaker.Hit((transform.position - other.transform.position).normalized * camHitStrengthOnHit);
            //Hit animation
            Game.instance.commonVfx.SmallHit(other.transform.position, Color.white);
        }
    }

    private CarSide TriggerToSide(ColliderListener remote)
    {
        if (remote == backTrig)
            return CarSide.Back;
        else if (remote == leftTrig)
            return CarSide.Left;
        else if (remote == rightTrig)
            return CarSide.Right;
        return CarSide.Front;
    }

    private CarSide ColliderToSide(ColliderListener remote)
    {
        if (remote == backCol)
            return CarSide.Back;
        else if (remote == leftCol)
            return CarSide.Left;
        else if (remote == rightCol)
            return CarSide.Right;
        return CarSide.Front;
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarTrigger { Front = 0, Back = 1, Right = 2, Left = 3 }

public class PlayerCarTriggers : PlayerComponent
{
    public delegate void UnitDetectionEvent(Unit unit, CarTrigger carTrigger);
    public event UnitDetectionEvent onEnter;
    public event UnitDetectionEvent onExit;

    [Header("Collider Listeners")]
    public MultipleColliderListener masterListener;
    public ColliderListener front;
    public ColliderListener back;
    public ColliderListener right;
    public ColliderListener left;

    void Awake()
    {
        masterListener.onTriggerEnter += OnEnter;
        masterListener.onTriggerExit += OnExit;
    }

    private void OnEnter(ColliderInfo otherInfo, ColliderListener listener)//Unit unit, RemoteTriggerListener source, GameObject other)
    {
        CarTrigger trigger = RemoteToTrigger(listener);
        
        if (onEnter != null)
        {
            onEnter.Invoke(otherInfo.parentUnit, trigger);
        }

        //Damage the enemy
        IAttackable attackable = otherInfo.parentUnit.GetComponent<IAttackable>();
        if (attackable == null)
            return;

        int damage = 0;
        switch (trigger)
        {
            case CarTrigger.Front:
                damage = controller.playerStats.frontDamage;
                break;
            case CarTrigger.Back:
                damage = controller.playerStats.backDamage;
                break;
            case CarTrigger.Right:
                damage = controller.playerStats.rightDamage;
                break;
            case CarTrigger.Left:
                damage = controller.playerStats.leftDamage;
                break;
        }
        damage *= controller.playerStats.damageMultiplier;
        if(damage > 0)
        {
            attackable.Attacked(otherInfo, damage, listener.info);
        }
    }

    private void OnExit(ColliderInfo otherInfo, ColliderListener listener)
    {
        if (onExit != null)
        {
            onExit.Invoke(otherInfo.parentUnit, RemoteToTrigger(listener));
        }
    }

    private CarTrigger RemoteToTrigger(ColliderListener remote)
    {
        if (remote == back)
            return CarTrigger.Back;
        else if (remote == left)
            return CarTrigger.Left;
        else if (remote == right)
            return CarTrigger.Right;
        return CarTrigger.Front;
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }
}

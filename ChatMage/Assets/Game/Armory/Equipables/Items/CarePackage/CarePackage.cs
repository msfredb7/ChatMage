using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarePackage : Unit
{
    public SimpleColliderListener listener;
    public float unitWidth;
    public Unit[] rewards;

    protected override void Awake()
    {
        base.Awake();

        listener.onTriggerEnter += Listener_onTriggerEnter;
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit != Game.Instance.Player.vehicle)
            return;

        Die();
    }

    protected override void Die()
    {
        base.Die();

        Game.Instance.SpawnUnit(rewards[Random.Range(0, rewards.Length)], Position);

        Destroy();
    }
}

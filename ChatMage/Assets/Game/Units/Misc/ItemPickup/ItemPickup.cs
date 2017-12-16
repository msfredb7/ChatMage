using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Unit
{
    public SimpleColliderListener trigger;
    public Item item;

    protected override void Awake()
    {
        base.Awake();
            trigger.onTriggerEnter += Trigger_onTriggerEnter;
    }

    private void Trigger_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        PlayerController player;
        if (IsDead || Game.instance == null || (player = Game.instance.Player) == null)
            return;

        player.playerItems.Equip(item);
        Die();
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}

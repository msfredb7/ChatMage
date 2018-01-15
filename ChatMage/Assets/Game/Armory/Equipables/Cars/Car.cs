using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;

public abstract class Car : Equipable
{
    [InspectorMargin(12), InspectorHeader("Player")]
    public PlayerController playerPrefab;

    [InspectorMargin(12), InspectorHeader("HP")]
    public int startingHPItems = 1;

    public override void Init(PlayerController player)
    {
        base.Init(player);

        Item hpItem = Game.Instance.itemSpawner.commonItemsReferences != null ? Game.Instance.itemSpawner.commonItemsReferences[0] : null;
        if(hpItem != null)
            for (int i = 0; i < startingHPItems; i++)
            {
                player.playerItems.Equip(hpItem);
            }
    }


    public abstract void OnInputUpdate(float horizontalInput);
}

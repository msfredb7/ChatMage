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
    public int startingHP = 3;
    public int startingArmor = 0;

    public override void Init(PlayerController player)
    {
        base.Init(player);

        player.playerStats.health.MAX = startingHP;
        player.playerStats.health.Set(startingHP);

        player.playerStats.armor.MAX = startingArmor;
        player.playerStats.armor.Set(startingArmor);
    }


    public abstract void OnInputUpdate(float horizontalInput);
}

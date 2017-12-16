using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ITM_CooldownReduction : Item
{
    [InspectorRange(0, 1), InspectorHeader("Formula: cooldownMult = 1 - cooldownReduc")]
    public float cooldownReduction = 0.5f;

    public override void Equip(int duplicateIndex)
    {
        throw new System.NotImplementedException();
    }

    public override void OnGameReady()
    {
        Game.instance.Player.playerStats.cooldownMultiplier.Set(1 - cooldownReduction);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Unequip()
    {
        throw new System.NotImplementedException();
    }
}

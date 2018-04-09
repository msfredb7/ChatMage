using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_AC130 : Item
{
    public AC130Effect effectPrefab;
    public float duration = 30;
    public int ammo = 6;

    [fsIgnore, NonSerialized]
    private AC130Effect effect;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);


        effect = Game.Instance.GetComponentInChildren<AC130Effect>(true);
        if (effect == null)
            effect = Instantiate(effectPrefab.gameObject, Game.Instance.transform).GetComponent<AC130Effect>();

        CoroutineLauncher.Instance.DelayedCall(DestroyAndLaunch, 0.75f);
        //effect.Smash(duration, ammo, null);
    }

    private void DestroyAndLaunch()
    {
        effect.Smash(duration, ammo, null);
        player.playerItems.Unequip(this);
    }

    public override float GetWeight()
    {
        int enemyCount = 0;
        foreach (var unit in Game.Instance.attackableUnits)
        {
            if (unit.allegiance == Allegiance.Enemy && !unit.IsDead)
                enemyCount++;
        }

        if (enemyCount < 2)
            return 0;

        if (enemyCount > 3)
            return 2;

        return 1;
    }
}

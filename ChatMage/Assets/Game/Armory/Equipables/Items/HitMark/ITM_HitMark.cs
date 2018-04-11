using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ITM_HitMark : Item
{
    public GameObject hitMarkPrefab;
    public AudioPlayable sfx;

    [InspectorHeader("Multiple Hit Marks")]
    public UnityObjectVariable mutex;
    public float interval = 0.15f;
    public float minRange = 0.1f;
    public float maxRange = 0.3f;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);
        player.playerStats.OnUnitKilled += PlayerCarTriggers_onUnitKilled;
    }

    private void PlayerCarTriggers_onUnitKilled(Unit unit)
    {
        if (mutex.Value == null)
            mutex.Value = this;

        if (mutex.Value == this)
        {
            int totalHitMarks = 0;
            var itemList = player.playerItems.items;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] is ITM_HitMark)
                    totalHitMarks++;
            }

            var spawnPos = unit.Position;

            //SpawnHitMark(spawnPos);
            for (int i = 0; i < totalHitMarks; i++)
            {
                Game.Instance.DelayedCall(() =>
                {
                    SpawnHitMark(spawnPos + CCC.Math.Vectors.RandomVector2(minRange, maxRange));
                }, i * interval);
            }
        }
    }

    private void SpawnHitMark(Vector2 position)
    {
        Instantiate(hitMarkPrefab, position, Quaternion.identity);
        DefaultAudioSources.PlayStaticSFX(sfx);
    }

    public override void Unequip()
    {
        base.Unequip();
        
        player.playerStats.OnUnitKilled -= PlayerCarTriggers_onUnitKilled;

        if (mutex.Value == this)
            mutex.Value = null;
    }

    public override void PreGameClear()
    {
        base.PreGameClear();

        mutex.Value = null;
    }
}

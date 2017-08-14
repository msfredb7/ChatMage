using UnityEngine;
using System.Collections;
using System;
using CCC.Utility;

public class RandomCompositeUnitSpawn : CompositeUnitSpawn
{
    [System.Serializable]
    public class SubSpawn : ILotteryItem
    {
        public UnitSpawn spawn;
        public float weight = 1;

        public float Weight()
        {
            return weight;
        }
    }

    public SubSpawn[] subSpawns;

    public override void DrawGizmos()
    {
        if (subSpawns != null)
            for (int i = 0; i < subSpawns.Length; i++)
            {
                if (subSpawns[i].spawn != null)
                {
                    if (subSpawns[i].spawn != this)
                        subSpawns[i].spawn.DrawGizmos();
                    else
                        Debug.LogError("The Composite Unit Spawn cannot contain itself");
                }
            }
    }

    public override T SpawnUnit<T>(T prefab)
    {
        if (subSpawns == null || subSpawns.Length == 0)
            throw new Exception("Composite unit spawn NEEDS at least a sub spawn");

        Lottery lottery = new Lottery(subSpawns);
        UnitSpawn spawn = (lottery.Pick() as SubSpawn).spawn;
        if (spawn == null)
            throw new Exception("Composite unit spawn has a NULL sub spawn");
        T unit = spawn.SpawnUnit(prefab);

        return unit;
    }

    public override void RemoveSubSpawnAt(int index)
    {
        SubSpawn[] newArray = new SubSpawn[subSpawns.Length - 1];
        int count = 0;

        for (int i = 0; i < subSpawns.Length; i++)
        {
            if (i != index)
            {
                newArray[count] = subSpawns[i];
                count++;
            }
        }
        subSpawns = newArray;
    }

    public override UnitSpawn GetSubSpawnAt(int index)
    {
        if (subSpawns == null)
            return null;
        return subSpawns[index].spawn;
    }

    public override int GetSubSpawnsCount()
    {
        if (subSpawns == null)
            return 0;
        return subSpawns.Length;
    }
}
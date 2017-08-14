using UnityEngine;
using System.Collections;
using System;
using CCC.Utility;

public class OrderedCompositeUnitSpawn : CompositeUnitSpawn
{
    public int changeSpawnEvery = 1;
    public UnitSpawn[] subSpawns;

    private int spawnCount;
    private int nextIndex;

    public override void DrawGizmos()
    {
        if (subSpawns != null)
            for (int i = 0; i < subSpawns.Length; i++)
            {
                if (subSpawns[i] != null)
                {
                    if (subSpawns[i] != this)
                        subSpawns[i].DrawGizmos();
                    else
                        Debug.LogError("The Composite Unit Spawn cannot contain itself");
                }
            }
    }

    public override T SpawnUnit<T>(T prefab)
    {
        if (subSpawns == null || subSpawns.Length == 0)
            throw new Exception("Composite unit spawn NEEDS at least a sub spawn");
        
        UnitSpawn spawn = subSpawns[nextIndex];

        spawnCount++;

        //Change spawn ?
        if(spawnCount == changeSpawnEvery)
        {
            nextIndex++;
            if (nextIndex >= subSpawns.Length)
                nextIndex = 0;
            spawnCount = 0;
        }

        if (spawn == null)
            throw new Exception("Composite unit spawn has a NULL sub spawn");

        T unit = spawn.SpawnUnit(prefab);

        return unit;
    }

    public override void RemoveSubSpawnAt(int index)
    {
        UnitSpawn[] newArray = new UnitSpawn[subSpawns.Length - 1];
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
        return subSpawns[index];
    }

    public override int GetSubSpawnsCount()
    {
        if (subSpawns == null)
            return 0;
        return subSpawns.Length;
    }
}
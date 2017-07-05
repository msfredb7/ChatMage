using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CompositeUnitSpawn : UnitSpawn
{
    public override abstract T SpawnUnit<T>(T prefab);

    public override Vector2 DefaultSpawnPosition()
    {
        throw new Exception("Should not occur");
    }

    public override float DefaultSpawnRotation()
    {
        throw new Exception("Should not occur");
    }

    public bool IsThereALoop(out int problematicIndex, CompositeUnitSpawn original)
    {
        problematicIndex = -1;

        bool loopDetected = false;
        int total = GetSubSpawnsCount();
        for (int i = 0; i < total; i++)
        {
            UnitSpawn other = GetSubSpawnAt(i);
            if (other != null && other is CompositeUnitSpawn)
            {
                if (other == original)
                {
                    problematicIndex = i;
                    return true;
                }
                else
                {
                    int bidon;
                    loopDetected = (other as CompositeUnitSpawn).IsThereALoop(out bidon, original);
                    problematicIndex = i;
                    break;
                }
            }
        }
        return loopDetected;
    }

    public abstract void RemoveSubSpawnAt(int index);
    public abstract UnitSpawn GetSubSpawnAt(int index);
    public abstract int GetSubSpawnsCount();
}

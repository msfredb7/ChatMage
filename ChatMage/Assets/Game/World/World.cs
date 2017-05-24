using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : BaseScriptableObject {

    public string displayName;
    public List<Region> regions;

    public Level GetLevel(int regionNumber, int levelNumber)
    {
        Level result = null;
        for (int i = 0; i < regions[regionNumber].levels.Count; i++)
        {
            if (regions[regionNumber].levels[i].levelNumber == levelNumber)
                result = regions[regionNumber].levels[i];
        }
        return result;
    }

    public Region GetRegion(int regionNumber)
    {
        return regions[regionNumber];
    }

    public void UnlockLevel(int regionNumber, int levelNumber)
    {
        GetLevel(regionNumber, levelNumber).unlock = true;
    }

    public void LockLevel(int regionNumber, int levelNumber)
    {
        GetLevel(regionNumber, levelNumber).unlock = false;
    }
}

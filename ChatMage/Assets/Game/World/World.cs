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
        for (int i = 0; i < regions[regionNumber-1].levels.Count; i++)
        {
            if (regions[regionNumber-1].levels[i].levelNumber == (levelNumber - 1))
                result = regions[regionNumber-1].levels[i];
        }
        return result;
    }

    public Level GetLevelByLevelScript(LevelScript levelScript)
    {
        Level result = null;
        for (int i = 0; i < regions.Count; i++)
        {
            for (int j = 0; j < regions[i].levels.Count; j++)
            {
                if (regions[i].levels[i].levelScript == levelScript)
                    result = regions[i].levels[i];
            }
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

    public void LoadUnlockInformation(GameSaves saves)
    {
        for (int i = 0; i < regions.Count; i++)
        {
            for (int j = 0; j < regions[i].levels.Count; j++)
            {
                if(saves.ContainsBool(GameSaves.Type.World, i + "-" + j))
                {
                    regions[i].levels[j].unlock = saves.GetBool(GameSaves.Type.World, i + "-" + j);
                } else
                {
                    GameSaves.instance.SetBool(GameSaves.Type.World, i + "-" + j, regions[i].levels[j].unlock);
                }
            }
        }
    }
}

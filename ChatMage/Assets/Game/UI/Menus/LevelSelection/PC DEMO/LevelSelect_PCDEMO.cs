using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect_PCDEMO : MonoBehaviour
{
    public EquipablePreview[] equipables;
    public DataSaver loadoutSaver;

    void Start()
    {
        DefaultAudioSources.StopMusicFaded();
    }


    public void GoToLevel(Level level)
    {
        GoToLevel(level.levelScriptName);
    }

    public void GoToLevel(string levelScriptName)
    {
        LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, GetLoadout(), true));
    }

    LoadoutResult GetLoadout()
    {
        LoadoutResult lr = new LoadoutResult();
        for (int i = 0; i < equipables.Length; i++)
        {
            lr.AddEquipable(equipables[i].equipableAssetName, equipables[i].type);
        }
        return lr;
    }
}

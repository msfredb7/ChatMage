using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LevelSelect
{
    public class LevelSelect_SkipLoadout : MonoBehaviour
    {
        public EquipablePreview[] equipables;
        public DataSaver loadoutSaver;

        public void LoadLevel(string levelScriptName)
        {
            DefaultAudioSources.StopMusicFaded();
            LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, GetLoadout(), true), true);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LevelSelect
{
    public class LevelSelect_SkipLoadout : MonoBehaviour
    {
        public EquipablePreview[] equipables;
        public DataSaver loadoutSaver;
        public AdsStarter adStarter;

        public void LoadLevel(string levelScriptName)
        {
            DefaultAudioSources.StopMusicFaded(0.5f,delegate() {
#if UNITY_ADS
                LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, GetLoadout(), true), true);
                adStarter.ShowRewardedAd();
                //LoadingScreen.TransitionTo(AdsRelayer.SCENENAME, new AdsRelayer(new ToGameMessage(levelScriptName, GetLoadout(), true),LevelSelection.SCENENAME,Framework.SCENENAME), false);
#else
            LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, GetLoadout(), true), true);
#endif
            });
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

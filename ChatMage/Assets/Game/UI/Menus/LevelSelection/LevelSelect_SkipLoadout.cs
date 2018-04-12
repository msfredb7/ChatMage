using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LevelSelect
{
    public class LevelSelect_SkipLoadout : MonoBehaviour
    {
        public EquipablePreview[] equipables;
        public AdsStarter adStarter;

        public void LoadLevel(string levelScriptName)
        {
            DefaultAudioSources.StopMusicFaded(0.5f, delegate ()
            {
                LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, GetLoadout(), true), true);
#if UNITY_ADS
                if (Application.isMobilePlatform && adStarter != null && adStarter.IsConnected)
                {
                    adStarter.ShowRewardedAd();
                }
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

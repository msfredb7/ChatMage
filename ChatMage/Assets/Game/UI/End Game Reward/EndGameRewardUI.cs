
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndGameReward
{
    public class EndGameRewardUI : MonoBehaviour
    {
        public const string SCENENAME = "EndGameReward";

        [Header("Linking")]
        public PinataExplosion pinataExplosion;
        public BackgroundFreezer backgroundFreezer;
        public RewardDisplay rewardDisplay;

        private string levelScriptAssetName;
        private GameReward reward;

        private Vector2 pinataCenter;

        public void Init(GameReward reward, string levelScriptAssetName)
        {
            this.levelScriptAssetName = levelScriptAssetName;
            this.reward = reward;
        }

        public void PinataHasBeenDestroyed(Vector2 explosionPosition, Camera currentCamera, Action canUnloadCallback)
        {
            SoundManager.SlowMotionEffect(false);
            backgroundFreezer.FreezeBackground(currentCamera, delegate ()
            {
                pinataExplosion.Animate(explosionPosition, PinataExplosion.BallColor.Blue);
                canUnloadCallback();
            });

            DelayManager.LocalCallTo(delegate ()
            {
                rewardDisplay.Init(reward);
            }, 1, this);
        }

        public void Continue()
        {
            if(levelScriptAssetName == "LS_ThirdLevel")
            {
                GameSaves.instance.ClearAllSaves();
                LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
            } else
                LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}
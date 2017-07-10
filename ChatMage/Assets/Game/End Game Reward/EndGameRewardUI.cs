using CCC.Manager;
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

        //private string levelScriptAssetName;
        private GameReward reward;
        private bool firstWin;

        private Vector2 pinataCenter;

        public void Init(GameReward reward, string levelScriptAssetName, bool firstWin)
        {
            //this.levelScriptAssetName = levelScriptAssetName;  //On en a pas de besoin pour l'instant
            this.reward = reward;
            this.firstWin = firstWin;
        }

        public void PinataHasBeenDestroyed(Vector2 explosionPosition, Camera currentCamera, Action canUnloadCallback)
        {

            backgroundFreezer.FreezeBackground(currentCamera, delegate ()
            {
                pinataExplosion.Animate(explosionPosition, PinataExplosion.BallColor.Blue);
                canUnloadCallback();
            });

            DelayManager.LocalCallTo(delegate ()
            {
                rewardDisplay.Init(reward, firstWin);
            }, 1, this);
        }

        public void Continue()
        {
            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}
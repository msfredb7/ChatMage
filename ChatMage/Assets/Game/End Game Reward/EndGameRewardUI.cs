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
        public EndGameRewardAnimation animations;

        public bool DEMO = false;

        //private string levelScriptAssetName;
        private GameRewards reward;
        private bool firstWin;

        private Vector2 pinataCenter;

        public void Init(GameRewards reward, string levelScriptAssetName, bool firstWin)
        {
            //this.levelScriptAssetName = levelScriptAssetName;  //On en a pas de besoin pour l'instant
            this.reward = reward;
            this.firstWin = firstWin;
        }

        public void PinataHasBeenDestroyed(Vector2 explosionPosition, Camera currentCamera, Action canUnloadCallback)
        {
            
            backgroundFreezer.FreezeBackground(currentCamera, delegate()
            {
                pinataExplosion.Animate(explosionPosition, reward.lootboxColor);
                canUnloadCallback();
            });

            DelayManager.LocalCallTo(delegate ()
            {
                animations.Init(reward, firstWin);
            }, 1, this);
        }

        void Continue()
        {
            if(DEMO) // A ENLEVER
                LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
            else
                LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}
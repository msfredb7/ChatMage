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
        public FadingButton continueButton;

        public bool DEMO = false;

        private string levelScriptAssetName;
        private GameReward reward;


        private Vector2 pinataCenter;

        void Awake()
        {
            continueButton.Interactable = false;
            continueButton.Hide();
            continueButton.onClick += Continue;
        }

        public void Init(GameReward reward, string levelScriptAssetName)
        {
            this.levelScriptAssetName = levelScriptAssetName;
            this.reward = reward;
        }

        public void PinataHasBeenDestroyed(Vector2 explosionPosition, Camera currentCamera, Action canUnloadCallback)
        {
            
            backgroundFreezer.FreezeBackground(currentCamera, delegate()
            {
                pinataExplosion.Animate(explosionPosition, PinataExplosion.BallColor.Blue);
                canUnloadCallback();
            });

            DelayManager.LocalCallTo(delegate ()
            {
                continueButton.Show(true);
                continueButton.Interactable = true;
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
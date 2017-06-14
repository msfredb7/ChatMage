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

        public void PinataHasBeenDestroyed(Vector2 viewportPosition, Camera currentCamera, Action canUnloadCallback)
        {
            
            backgroundFreezer.FreezeBackground(currentCamera, canUnloadCallback);

            // A faire
            //pinataExplosion.SetCenter(...);
            pinataExplosion.Animate();

            DelayManager.CallTo(delegate ()
            {
                continueButton.Show(true);
                continueButton.Interactable = true;
            }, 1);
        }

        void Continue()
        {
            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}
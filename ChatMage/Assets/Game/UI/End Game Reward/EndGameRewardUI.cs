
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace EndGameReward
{
    public class EndGameRewardUI : MonoBehaviour
    {
        public const string SCENENAME = "EndGameReward";

        [Header("Linking")]
        public PinataExplosion pinataExplosion;
        public BackgroundFreezer backgroundFreezer;
        //public RewardDisplay rewardDisplay;
        public WinAnimation winAnimation;
        public AudioMixerSnapshot normalAudioSnapshot;

        //private string levelScriptAssetName;
        private List<EquipablePreview> reward;

        private Vector2 pinataCenter;

        public void Init(List<EquipablePreview> reward, string levelScriptAssetName)
        {
            //this.levelScriptAssetName = levelScriptAssetName;
            this.reward = reward;
        }

        public void PinataHasBeenDestroyed(Vector2 explosionPosition, Camera currentCamera, Action canUnloadCallback)
        {
            normalAudioSnapshot.TransitionTo(0.75f);
            backgroundFreezer.FreezeBackground(currentCamera, delegate ()
            {
                //pinataExplosion.Animate(explosionPosition, PinataExplosion.BallColor.Blue);
                canUnloadCallback();
            });

            this.DelayedCall(delegate ()
            {
                winAnimation.Animate();
                //rewardDisplay.Init(reward);
            }, 1);
        }

        public void Continue()
        {
            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}
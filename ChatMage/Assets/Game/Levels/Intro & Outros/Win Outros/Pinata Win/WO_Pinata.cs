using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EndGameReward;

namespace GameIntroOutro
{
    public class WO_Pinata : BaseWinOutro
    {
        [Header("Linking")]
        public Unit pinata;
        [Header("Settings")]
        public float spawnDelay = 1;

        private EndGameRewardUI rewardUI;
        private float spawnDelayRemaining = -1;
        private bool canSpawnBall = false;

        void Update()
        {
            if (spawnDelayRemaining > 0)
                spawnDelayRemaining -= Time.deltaTime * Game.instance.worldTimeScale;
            else if (canSpawnBall)
                SpawnPinata();

        }

        void SpawnPinata()
        {
            canSpawnBall = false;
            Game.instance.SpawnUnit(pinata, Game.instance.gameCamera.Center).OnDeath += OnPinataKilled; ;
        }

        private void OnPinataKilled(Unit unit)
        {
            Camera cam = Game.instance.gameCamera.cam;
            rewardUI.PinataHasBeenDestroyed(
                unit.Position,
                cam,
                UnloadGameScenes);
        }

        private void UnloadGameScenes()
        {
            //On unload TOUS les scene sauf celle du endGameResult
            //   Peut etre qu'on devrais seulement unload les scene 'Framework' 'Map' et 'UI'
            //   Si on decide de faire ca, LE FAIRE DANS FRAMEWORK

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name != EndGameRewardUI.SCENENAME)
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                    //i--;
                }
            }
        }

        public override void Play()
        {
            spawnDelayRemaining = spawnDelay;

            Game.instance.gameCamera.followPlayer = false;

            Scenes.LoadAsync(EndGameRewardUI.SCENENAME, LoadSceneMode.Additive, OnWinScreenLoaded);
        }

        void OnWinScreenLoaded(Scene scene)
        {
            rewardUI = Scenes.FindRootObject<EndGameRewardUI>(scene);

            rewardUI.Init(Game.instance.levelScript.rewards, Game.instance.levelScript.name);

            canSpawnBall = true;
        }
    }
}

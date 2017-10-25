using CCC.Manager;
using EndGameReward;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameIntroOutro
{
    public abstract class StdWinOutro : BaseWinOutro
    {
        private EndGameRewardUI rewardUI;
        private bool hasEnded = false;

        protected virtual void LoadWinScene()
        {
            Scenes.LoadAsync(EndGameRewardUI.SCENENAME, LoadSceneMode.Additive, OnWinScreenLoaded);
        }

        protected virtual void OnWinScreenLoaded(Scene scene)
        {
            rewardUI = Scenes.FindRootObject<EndGameRewardUI>(scene);

            rewardUI.Init(Game.instance.levelScript.rewards, Game.instance.levelScript.name);

            CanEnd();
        }

        protected virtual void CheckEnd()
        {
            if (rewardUI != null && CanEnd())
                End();
        }

        protected abstract bool CanEnd();

        protected virtual void End()
        {
            if (hasEnded)
                return;
            hasEnded = true;

            Camera cam = Game.instance.gameCamera.cam;
            rewardUI.PinataHasBeenDestroyed(
                Game.instance.gameCamera.Center,
                cam,
                UnloadGameScenes);
        }

        protected virtual void UnloadGameScenes()
        {
            Game.instance.music.TransitionTo(MusicManager.SongName.Win);
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
    }
}
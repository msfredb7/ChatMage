using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameIntroOutro
{
    public class YouDiedOutro : BaseLoseOutro
    {
        public GameObject portalVFXPrefab;
        public SceneInfo endlessMenu;
        public WinAnimation textAnimation;
        public Image bgImage;
        public Color bgImageTargetColor;

        public override void Play()
        {
            bgImage.DOColor(bgImageTargetColor, 1);

            var portalInstance = portalVFXPrefab.Duplicate();
            var vfx = portalInstance.GetComponentInChildren<PortalVFX>();
            textAnimation.portal = vfx;
            textAnimation.Animate();
        }

        public void Restart()
        {
            DefaultAudioSources.StopMusicFaded();
            Game.Instance.framework.RestartLevel();
        }

        public void GoBackToMenu()
        {
            DefaultAudioSources.StopMusicFaded();
            if (Game.Instance.levelScript is LS_EndlessLevel)
            {
                LoadingScreen.TransitionTo(endlessMenu.SceneName, null);
            }
            else
            {
                LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameIntroOutro
{
    public class YouDiedOutro : BaseLoseOutro
    {
        public override void Play()
        {
            //hehe
        }

        public void Restart()
        {
            Game.instance.framework.RestartLevel();
        }

        public void GoBackToMenu()
        {
            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}

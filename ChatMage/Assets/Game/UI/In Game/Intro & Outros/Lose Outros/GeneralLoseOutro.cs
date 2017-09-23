using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameIntroOutro
{
    public class GeneralLoseOutro : BaseLoseOutro
    {
        public bool freezeTimescale = false;

        public override void Play()
        {
            if (freezeTimescale)
                Game.instance.gameRunning.Lock("ending");
        }

        public void Restart()
        {
            Game.instance.framework.RestartLevel();
        }

        public void ToLevelSelect()
        {
            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}

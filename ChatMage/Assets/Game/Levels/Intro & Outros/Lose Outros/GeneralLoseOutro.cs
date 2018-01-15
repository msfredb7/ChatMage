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
                Game.Instance.gameRunning.Lock("ending");
        }

        public void Restart()
        {
            Game.Instance.framework.RestartLevel();
        }

        public void ToLevelSelect()
        {
            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}

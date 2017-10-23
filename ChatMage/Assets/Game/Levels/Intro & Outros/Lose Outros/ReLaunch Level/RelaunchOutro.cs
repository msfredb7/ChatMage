using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

namespace GameIntroOutro
{
    public class RelaunchOutro : BaseLoseOutro
    {
        public AudioClip gameOverSound;
        public float soundDelay = 0.25f;

        public override void Play()
        {
            SoundManager.PlaySFX(gameOverSound, soundDelay);
        }

        public void Restart()
        {
            Game.instance.framework.RestartLevel();
        }
    }
}

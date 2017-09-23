using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameIntroOutro
{
    public class RelaunchOutro : BaseLoseOutro
    {
        public override void Play()
        {

        }

        public void Restart()
        {
            Game.instance.framework.RestartLevel();
        }
    }
}

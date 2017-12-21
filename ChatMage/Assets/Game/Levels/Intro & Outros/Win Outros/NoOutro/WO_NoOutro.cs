using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameIntroOutro
{
    public class WO_NoOutro : StdWinOutro
    {
        public override void Play()
        {
            LoadWinScene();
        }

        protected override bool CanEnd()
        {
            return true;
        }
    }
}

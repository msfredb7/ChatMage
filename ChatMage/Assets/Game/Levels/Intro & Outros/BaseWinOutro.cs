using System.Collections.Generic;
using UnityEngine;

namespace GameIntroOutro
{
    public abstract class BaseWinOutro : MonoBehaviour
    {
        public ParentType parentType = ParentType.UnderCanvas;
        public enum ParentType { UnderCanvas = 0, UnderCanvasWithinGameView = 2, UnderGame = 1 }
        public abstract void Play();
    }
}

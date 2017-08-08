using UnityEngine;

namespace GameIntroOutro
{
    public abstract class BaseLoseOutro : MonoBehaviour
    {
        public ParentType parentType = ParentType.UnderCanvas;
        public enum ParentType { UnderCanvas = 0, UnderCanvasWithinGameView = 2, UnderGame = 1 }
        public abstract void Play();
    }
}

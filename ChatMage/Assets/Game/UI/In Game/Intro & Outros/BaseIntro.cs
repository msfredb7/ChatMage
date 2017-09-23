using System;
using UnityEngine;

namespace GameIntroOutro
{
    public abstract class BaseIntro : MonoBehaviour
    {
        public ParentType parentType = ParentType.UnderCanvas;
        public enum ParentType { UnderCanvas = 0, UnderGame = 1 }
        public abstract void Play(Action onComplete);
    }

}
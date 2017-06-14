using System;
using UnityEngine;

namespace GameIntroOutro
{
    public abstract class BaseIntro : MonoBehaviour
    {
        public abstract void Play(Action onComplete);
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialoguing.Effect
{
    [System.Serializable]
    public class Shaker_Camera : Effect
    {
        public float intensity = 1;
        public float duration = 0.25f;

        public override void Apply(DialogDisplay display, Reply reply)
        {
            Game.Instance.gameCamera.vectorShaker.Shake(intensity, duration);
        }
    }
}

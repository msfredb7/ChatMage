using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialoguing.Effect
{
    [System.Serializable]
    public class Shaker : Effect
    {
        public bool shakeCamera = false;
        public bool shakeLeftCharacter = true;
        public bool shakeRightCharacter = false;
        public float intensity = 1;
        public float duration = 0.25f;

        public override void Apply(DialogDisplay display, Reply reply)
        {
            if(shakeCamera)
                Game.instance.gameCamera.vectorShaker.Shake(intensity, duration);
            if (shakeLeftCharacter)
                display.characters.SetLeftCharacterShake(intensity, duration);
            if (shakeRightCharacter)
                display.characters.SetRightCharacterShaker(intensity, duration);
        }
    }
}

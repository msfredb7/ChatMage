using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialoguing.Effect
{
    [System.Serializable]
    public class Shaker_Character : Effect
    {
        public float intensity = 50;
        public float duration = 0.25f;

        public override void Apply(DialogDisplay display, Reply reply)
        {
            Characters charac = display.characters;
            charac.supposeToShakeCharacter = true;
            charac.currentCharacterShakeDuration = duration;
            charac.currentCharacterShakeIntensity = intensity;
            if (!charac.supposeToShakeCharacter)
            {
                if (charac.leftImage.enabled)
                    charac.SetLeftCharacterShake(intensity, duration);
                if (charac.rightImage.enabled)
                    charac.SetRightCharacterShaker(intensity, duration);
            }
        }
    }
}

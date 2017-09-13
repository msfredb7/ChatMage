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
            if (display.characters.leftImage.enabled)
                display.characters.SetLeftCharacterShake(intensity, duration);
            if (display.characters.rightImage.enabled)
                display.characters.SetRightCharacterShaker(intensity, duration);
        }
    }
}

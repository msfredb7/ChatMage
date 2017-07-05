using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialoguing.Effect
{
    [System.Serializable]
    public class ChangeSprite : Effect
    {
        public Sprite newSprite;
        public Vector2 offset = Vector2.zero;
        public bool leftSide = true;

        public override void Apply(DialogDisplay display, Reply reply)
        {
            Characters charac = display.characters;

            if (offset != Vector2.zero)
            {
                if (leftSide)
                    charac.SetLeftImage(newSprite, offset);
                else
                    charac.SetRightImage(newSprite, offset);
            }
            else
            {
                if (leftSide)
                    charac.SetLeftImage(newSprite);
                else
                    charac.SetRightImage(newSprite);
            }
        }
    }
}
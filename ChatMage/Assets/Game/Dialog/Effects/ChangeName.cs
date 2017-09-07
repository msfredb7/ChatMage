using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialoguing.Effect
{
    [System.Serializable]
    public class ChangeName : Effect
    {
        public string name;
        public bool leftSide = true;

        public override void Apply(DialogDisplay display, Reply reply)
        {
            Characters charac = display.characters;

            if (leftSide)
                charac.SetLeftText(name);
            else
                charac.SetRightText(name);
        }
    }
}

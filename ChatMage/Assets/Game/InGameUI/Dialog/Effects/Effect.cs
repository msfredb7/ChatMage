using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialoguing.Effect
{
    public abstract class Effect
    {
        public abstract void Apply(DialogDisplay display, Reply reply);
    }
}
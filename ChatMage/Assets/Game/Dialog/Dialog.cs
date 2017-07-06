using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

namespace Dialoguing
{
    public class Dialog : BaseScriptableObject
    {
        public bool pauseGame = true;
        public Reply[] replies;
    }
}
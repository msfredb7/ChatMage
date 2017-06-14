using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

namespace LevelScripting
{
    [System.Serializable]
    public class WaveWhen
    {
        public Type type;
        [InspectorShowIf("UsesTime")]
        public float time;
        [InspectorShowIf("UsesName")]
        public string name;

        public enum Type
        {
            At = 0,                             // uses: float time
            Join = 1,                           //
            Append = 2,                         //
            AppendPlus = 3,                     // uses: float time
            OnLevelEvent = 4,                    // uses: string name
            OnManualTrigger = 6,                // uses: string name
        }

        public bool UsesTime { get { return type == Type.At || type == Type.AppendPlus; } }
        public bool UsesName { get { return type == Type.OnLevelEvent || type == Type.OnManualTrigger; } }
    }
}

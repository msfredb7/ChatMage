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
        [InspectorShowIf("UsesName")]
        public bool onlyTriggerOnce = false;

        public enum Type
        {
            At = 0,                             // uses: float time
            Join = 1,                           //
            JoinPlus = 9,                       // uses: float time
            Append = 2,                         //
            AppendPlus = 3,                     // uses: float time
            AppendComplete = 7,
            AppendCompletePlus = 8,             // uses: float time
            OnLevelEvent = 4,                   // uses: string name
            OnManualTrigger = 6,                // uses: string name
        }

        public bool UsesTime { get { return type == Type.At || type == Type.AppendPlus || type == Type.AppendCompletePlus
                     || type == Type.JoinPlus; } }
        public bool UsesName { get { return type == Type.OnLevelEvent || type == Type.OnManualTrigger; } }
    }
}

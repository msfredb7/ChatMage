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
        [InspectorShowIf("UsesFinishedRatio")]
        public float finishedRatio = 1;

        public enum Type
        {
            At = 0,                             // uses: float time
            Join = 1,                           //
            Append = 2,                         //
            AppendPlus = 3,                     // uses: float time
            OnMilestone = 4,                    // uses: string name
            AfterCompletionOfPreviousWave = 5,  // uses: float finishedRatio
            OnManualTrigger = 6,                // uses: string name
        }

        public bool UsesTime { get { return type == Type.At || type == Type.AppendPlus; } }
        public bool UsesName { get { return type == Type.OnMilestone || type == Type.OnManualTrigger; } }
        public bool UsesFinishedRatio { get { return type == Type.AfterCompletionOfPreviousWave; } }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;

namespace LevelScripting
{
    [System.Serializable]
    public class UnitWave
    {
        [InspectorCategory("What")]
        public UnitPack[] packs;
        [InspectorCategory("Where")]
        public WaveWhere where;
        [InspectorCategory("When")]
        public WaveWhen when;


        public enum Where { RandomAroundScreen = 0, Waypoints = 1 }
    }
}
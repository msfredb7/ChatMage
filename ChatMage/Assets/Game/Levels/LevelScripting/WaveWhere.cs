using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

namespace LevelScripting
{
    [System.Serializable]
    public class WaveWhere
    {
        public Type type;

        [InspectorShowIf("IsTypeWaypoint")]
        public WaypointInfo waypointInfo;

        public enum Type { RandomAroundScreen = 0, Waypoints = 1 }

        public bool IsTypeWaypoint { get { return type == Type.Waypoints; } }

        [System.Serializable]
        public class WaypointInfo
        {
            [InspectorHeader("Id")]
            public IdType idType;
            [InspectorHideIf("IdTypeIsEnum")]
            public string waypointName;
            [InspectorShowIf("IdTypeIsEnum")]
            public Waypoint.WaypointType waypointType;

            public enum IdType { Enum = 0, String = 1 }

            public bool IdTypeIsEnum { get { return idType == IdType.Enum; } }



            [InspectorHeader("Selection")]
            public Selection selection;

            [System.Serializable]
            public class Selection
            {
                public ReferencePoint referencePoint;
                public enum ReferencePoint { Player = 0, ScreenCenter = 1 }

                public float minRelativeHeight = -4.5f;
                public float maxRelativeHeight = 4.5f;
            }



            [InspectorHeader("Spawn")]
            public SpawningType spawningType;
            public enum SpawningType { Ordered = 0, Random = 1, AllUnitsOnSame = 2 }

        }
    }
}
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
        [InspectorHideIf("IsTypeRegion")]
        public bool clamp = false;


        [InspectorShowIf("UseClamping"), InspectorHeader("Clamping")]
        public ReferencePoint referencePoint;
        public enum ReferencePoint { Player = 0, ScreenCenter = 1, WorldCenter = 2 }
        [InspectorShowIf("UseClamping")]
        public Vector2 clampMin = new Vector2(-8, -4.5f);
        [InspectorShowIf("UseClamping")]
        public Vector2 clampMax = new Vector2(8, 4.5f);

        [InspectorShowIf("IsTypeWaypoint")]
        public WaypointInfo waypointInfo;

        public enum Type { RandomAroundScreen = 0, Waypoints = 1, WithinRegion = 2 }

        public bool IsTypeWaypoint { get { return type == Type.Waypoints; } }
        public bool IsTypeRegion { get { return type == Type.WithinRegion; } }
        public bool UseClamping { get { return type == Type.WithinRegion || clamp; } }

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
            public ReferencePoint referencePoint;
            public enum ReferencePoint { Player = 0, ScreenCenter = 1}
            public float minHeight = -4.5f;
            public float maxHeight = 4.5f;


            [InspectorHeader("Spawn")]
            public SpawningType spawningType;
            public enum SpawningType { Ordered = 0, Random = 1, AllUnitsOnSame = 2 }

        }
    }
}
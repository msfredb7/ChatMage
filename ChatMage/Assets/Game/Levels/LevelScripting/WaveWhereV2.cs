using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using FullInspector;

namespace LevelScripting
{
    [System.Serializable]
    public class WaveWhereV2
    {
        public bool logErrorIfUnableToLaunch = true;

        //_________________TAG_________________//
        public string spawnTag;


        //_________________FILTER_________________//
        public FilterType filterType;
        [InspectorShowIf("UseFilterVectors")]
        public Vector2 min;
        [InspectorShowIf("UseFilterVectors")]
        public Vector2 max;

        public enum FilterType
        {
            None = 0,
            RelativeToPlayer = 1,
            RelativeToCamera = 2,
        }
        bool UseFilterVectors { get { return filterType != FilterType.None; } }

        public List<UnitSpawn> Filter(List<UnitSpawn> spawns)
        {
            if (spawns == null)
                throw new System.Exception("Spawn list is null");

            FilterType type = filterType;

            //Si le joueur est null, on utilise la camera a la place
            if (type == FilterType.RelativeToPlayer && Game.Instance.Player == null)
                type = FilterType.RelativeToCamera;

            switch (type)
            {
                default:
                case FilterType.None:
                    return spawns;
                case FilterType.RelativeToPlayer:
                    {
                        List<UnitSpawn> newList = new List<UnitSpawn>();
                        Vector2 anchor = Game.Instance.Player.vehicle.Position;
                        for (int i = 0; i < spawns.Count; i++)
                        {
                            if (IsWithinRegion(spawns[i].transform.position, anchor, min, max))
                                newList.Add(spawns[i]);
                        }
                        return newList;
                    }
                case FilterType.RelativeToCamera:
                    {
                        List<UnitSpawn> newList = new List<UnitSpawn>();
                        Vector2 anchor = Game.Instance.gameCamera.Center;
                        for (int i = 0; i < spawns.Count; i++)
                        {
                            if (IsWithinRegion(spawns[i].transform.position, anchor, min, max))
                                newList.Add(spawns[i]);
                        }
                        return newList;
                    }
            }
        }

        private bool IsWithinRegion(Vector2 position, Vector2 anchor, Vector2 min, Vector2 max)
        {
            if (position.x > max.x + anchor.x)
                return false;
            if (position.y > max.y + anchor.y)
                return false;
            if (position.x < min.x + anchor.x)
                return false;
            if (position.y < min.y + anchor.y)
                return false;
            return true;
        }

        public UnitSpawn FilterAndSelect(List<UnitSpawn> spawns)
        {
            return Select(Filter(spawns));
        }


        //_________________SELECT_________________//
        public SelectType selectType;
        [InspectorShowIf("UseIndex")]
        public int index;

        public enum SelectType
        {
            ByIndex = 0,
            Random = 1,
            ClosestToPlayer = 2,
            ClosestToCamera = 3
        }
        bool UseIndex { get { return selectType == SelectType.ByIndex; } }

        public UnitSpawn Select(List<UnitSpawn> spawns)
        {
            if (spawns.Count == 0)
                return null;
            if (spawns.Count == 1)
                return spawns[0];

            SelectType type = selectType;

            //Si le joueur est null, on utilise la camera a la place
            if (type == SelectType.ClosestToPlayer && Game.Instance.Player == null)
                type = SelectType.ClosestToCamera;

            switch (type)
            {
                case SelectType.ByIndex:
                    return spawns[Mathf.Clamp(index, 0, spawns.Count - 1)];
                default:
                case SelectType.Random:
                    return spawns[Random.Range(0, spawns.Count)];
                case SelectType.ClosestToPlayer:
                    return GetClosestTo(Game.Instance.Player.vehicle.Position, spawns);
                case SelectType.ClosestToCamera:
                    return GetClosestTo(Game.Instance.gameCamera.Center, spawns);
            }
        }

        private UnitSpawn GetClosestTo(Vector2 position, List<UnitSpawn> spawns)
        {
            float smallestDistance = float.PositiveInfinity;
            int recordHolder = -1;

            for (int i = 0; i < spawns.Count; i++)
            {
                float distance = ((Vector2)spawns[i].transform.position - position).sqrMagnitude;
                if (distance < smallestDistance)
                {
                    recordHolder = i;
                    smallestDistance = distance;
                }
            }
            return spawns[recordHolder];
        }
    }
}
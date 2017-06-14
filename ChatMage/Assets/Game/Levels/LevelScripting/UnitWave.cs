using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;
using System;

namespace LevelScripting
{
    [System.Serializable]
    public class UnitWave
    {
        [InspectorCategory("What")]
        public UnitPack[] packs;
        [InspectorCategory("What")]
        public LevelEventCallback[] onProgressCallbacks;

        [InspectorCategory("Where")]
        public WaveWhere where;
        [InspectorCategory("When")]
        public WaveWhen when;
        [InspectorCategory("Across")]
        public float duration;

        public event SimpleEvent onLaunched;
        
        [fsIgnore]
        private List<Callback> completionCallbacks = new List<Callback>();
        [fsIgnore]
        private int totalUnits;
        [fsIgnore]
        private int currentKillCount = 0;
        [fsIgnore]
        private InGameEvents events;
        [fsIgnore]
        private int spawnedUnits;

        private void ResetData()
        {
            spawnedUnits = 0;
            totalUnits = 0;
            currentKillCount = 0;
            completionCallbacks = new List<Callback>();
        }

        public int TotalUnits()
        {
            int total = 0;
            for (int i = 0; i < packs.Length; i++)
            {
                total += packs[i].quantity;
            }
            return total;
        }

        public void Launch(InGameEvents events)
        {
            //Reset data
            ResetData();

            this.events = events;

            //Build callbacks from 'LevelEventCallbacks'
            for (int i = 0; i < onProgressCallbacks.Length; i++)
            {
                int localI = i;
                AddCallbackAfterCompletion(onProgressCallbacks[localI].completionRate, delegate ()
                {
                    Game.instance.currentLevel.ReceiveEvent(onProgressCallbacks[localI].eventName);
                });
            }

            //Quantite total de units a spawn
            totalUnits = TotalUnits();
            if (totalUnits <= 0)
            {
                //if (completionCallback != null)
                //    completionCallback();

                EndLaunch();
                return;
            }

            //Delai entre chaque spawning d'enemie
            float spawnDelta = duration / totalUnits;

            switch (where.type)
            {

                case WaveWhere.Type.RandomAroundScreen:
                    {
                        int currentCount = 0;
                        //Pour chaque pack de unit
                        foreach (UnitPack pack in packs)
                        {
                            //Pour chaque unit
                            for (int i = 0; i < pack.quantity; i++)
                            {
                                //Get Random pos around screen
                                Vector2 pos = Vector2.zero;

                                //Accot� sur le planfond/plancher OU le cot� droit/gauche ?
                                if (UnityEngine.Random.Range(0, 2) == 1)
                                {
                                    //    Donne:        soit -1 ou 1            , random entre -1f et 1f
                                    pos = new Vector2(UnityEngine.Random.Range(0, 2) * 2 - 1, UnityEngine.Random.Range(-1f, 1f));
                                }
                                else
                                {
                                    //    Donne:     random entre -1f et 1f,        soit -1 ou 1 
                                    pos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0, 2) * 2 - 1);
                                }

                                //Scale la position au bordure de l'�cran
                                pos.Scale(Game.instance.gameCamera.ScreenSize * 0.45f);

                                SpawnUnitRelativeToTransform(pack.unit, pos, Game.instance.gameCamera.transform, currentCount * spawnDelta);

                                spawnedUnits++;
                                currentCount++;
                            }
                        }

                        break;

                    }




                case WaveWhere.Type.WithinRegion:
                    {
                        where.clamp = false;

                        int currentCount = 0;
                        //Pour chaque pack de unit
                        foreach (UnitPack pack in packs)
                        {
                            //Pour chaque unit
                            for (int i = 0; i < pack.quantity; i++)
                            {
                                //Get Random pos within region
                                Vector2 v = new Vector2(UnityEngine.Random.Range(where.clampMin.x, where.clampMax.x),
                                    UnityEngine.Random.Range(where.clampMin.y, where.clampMax.y));

                                switch (where.referencePoint)
                                {
                                    case WaveWhere.ReferencePoint.Player:
                                        SpawnUnitRelativeToTransform(pack.unit, v, Game.instance.Player.transform, currentCount * spawnDelta);
                                        break;
                                    case WaveWhere.ReferencePoint.ScreenCenter:
                                        SpawnUnitRelativeToTransform(pack.unit, v, Game.instance.gameCamera.transform, currentCount * spawnDelta);
                                        break;
                                    case WaveWhere.ReferencePoint.WorldCenter:
                                        SpawnUnit(pack.unit, v, currentCount * spawnDelta);
                                        break;
                                }

                                spawnedUnits++;
                                currentCount++;
                            }
                        }
                        break;
                    }


                case WaveWhere.Type.Waypoints:
                    {
                        WaveWhere.WaypointInfo info = where.waypointInfo;
                        Mapping mapping = Game.instance.map.mapping;

                        //Il y a de drastiques difference entre les 2 façon de faire
                        // C'est pourquoi on fait 2 gros if/else.
                        if (info.UseSelection)
                        {
                            //On va chercher LE waypoint s'il sagit du mode 'allUnitsOnSame'
                            Waypoint allOnSameWaypoint = null;
                            if (info.spawningType == WaveWhere.WaypointInfo.SpawningType.AllUnitsOnSame)
                            {
                                allOnSameWaypoint = GetRandomFilteredWaypoint();
                            }


                            //Spawning !

                            int currentCount = 0;
                            //Pour chaque pack de unit
                            foreach (UnitPack pack in packs)
                            {
                                //Pour chaque unit
                                for (int i = 0; i < pack.quantity; i++)
                                {
                                    switch (info.spawningType)
                                    {
                                        case WaveWhere.WaypointInfo.SpawningType.Ordered:
                                            throw new Exception("On essai de spawn des unit sur des waypoints ordered. On ne"
                                                + " devrais pas utiliser 'filterByHeight");
                                        case WaveWhere.WaypointInfo.SpawningType.Random:

                                            events.AddDelayedAction(delegate ()
                                            {
                                                Waypoint wp = GetRandomFilteredWaypoint();
                                                if (wp != null)
                                                    SpawnUnit(pack.unit, wp.AdjustedPosition, -1);
                                                else
                                                {
                                                    //Failed to spawn unit
                                                    spawnedUnits--;
                                                    UpdateTargetKillCount();
                                                }
                                            },
                                            currentCount * spawnDelta);
                                            spawnedUnits++;
                                            break;
                                        case WaveWhere.WaypointInfo.SpawningType.AllUnitsOnSame:

                                            //Spawn simple
                                            if (allOnSameWaypoint != null)
                                            {
                                                SpawnUnit(pack.unit, allOnSameWaypoint.AdjustedPosition, currentCount * spawnDelta);
                                                spawnedUnits++;
                                            }
                                            break;
                                    }

                                    currentCount++;
                                }
                            }
                        }
                        else
                        {
                            List<Waypoint> waypoints = null;

                            //Get waypoints
                            switch (info.spawningType)
                            {
                                // 'Random' et 'Ordered' utilise la meme liste simple
                                case WaveWhere.WaypointInfo.SpawningType.Random:
                                case WaveWhere.WaypointInfo.SpawningType.Ordered:
                                    if (info.idType == WaveWhere.WaypointInfo.IdType.Tag)
                                        waypoints = mapping.GetWaypoints(info.waypointTag);
                                    else
                                        waypoints = mapping.GetWaypoints(info.waypointType);
                                    break;


                                // 'AllUnitsOnSame' utilise qu'un seul waypoint
                                case WaveWhere.WaypointInfo.SpawningType.AllUnitsOnSame:
                                    waypoints = new List<Waypoint>(1);

                                    Waypoint theWp = null;

                                    if (info.idType == WaveWhere.WaypointInfo.IdType.Tag)
                                        theWp = mapping.GetRandomWaypoint(info.waypointTag);
                                    else
                                        theWp = mapping.GetRandomWaypoint(info.waypointType);

                                    if (theWp != null)
                                        waypoints.Add(theWp);

                                    break;
                            }


                            //Spawning !

                            int currentCount = 0;
                            //Pour chaque pack de unit
                            foreach (UnitPack pack in packs)
                            {
                                //Pour chaque unit
                                for (int i = 0; i < pack.quantity; i++)
                                {
                                    if (waypoints.Count == 0)
                                        break;

                                    //Prendre position en fonction des waypoints acquis
                                    Vector2 pos = Vector2.zero;

                                    switch (info.spawningType)
                                    {
                                        case WaveWhere.WaypointInfo.SpawningType.Ordered:
                                            pos = waypoints[currentCount % waypoints.Count].AdjustedPosition;
                                            break;
                                        case WaveWhere.WaypointInfo.SpawningType.Random:
                                            pos = waypoints[UnityEngine.Random.Range(0, waypoints.Count)].AdjustedPosition;
                                            break;
                                        case WaveWhere.WaypointInfo.SpawningType.AllUnitsOnSame:
                                            pos = waypoints[0].AdjustedPosition;
                                            break;
                                    }

                                    //Spawn !
                                    SpawnUnit(pack.unit, pos, currentCount * spawnDelta);

                                    spawnedUnits++;
                                    currentCount++;
                                }
                            }
                        }

                        break;
                    }
            }

            EndLaunch();
        }

        private void EndLaunch()
        {
            UpdateTargetKillCount();

            if (onLaunched != null)
                onLaunched();
        }

        public void AddCallbackAfterCompletion(float completionRate, Action callback)
        {
            completionCallbacks.Add(new Callback(callback, completionRate));
        }

        private void SpawnUnit(Unit unit, Waypoint waypoint, float delay)
        {
            if (waypoint != null)
                SpawnUnit(unit, waypoint.AdjustedPosition, delay);
            //else
        }

        private void SpawnUnit(Unit unit, Vector2 position, float delay)
        {
            //On spawn la unit, mais faut-il compter la quantit� tu� ?
            if (completionCallbacks.Count > 0)
            {
                //Spawn + on setup des listener pour compter la mort de la unit
                events.SpawnUnit(unit, ClampCheck(position), delay, delegate (Unit spawnedUnit)
                {
                    spawnedUnit.onDeath += delegate (Unit deadUnit)
                    {
                        currentKillCount++;
                        CheckCompletion();
                    };
                });
            }
            else
            {
                //Regular Spawning
                events.SpawnUnit(unit, ClampCheck(position), delay);
            }
        }

        private void SpawnUnitRelativeToTransform(Unit unit, Vector2 relativePosition, Transform transform, float delay)
        {
            //On met un d�lai ici, car on veut �valu� la 'relativePosition + (Vector2)transform.position' APRES le d�lai
            events.AddDelayedAction(delegate ()
            {
                SpawnUnit(unit, relativePosition + (Vector2)transform.position, -1);
            },
            delay);
        }

        private Vector2 ClampCheck(Vector2 position)
        {
            if (!where.clamp)
                return position;

            Vector2 refPos = Vector2.zero;

            switch (where.referencePoint)
            {
                case WaveWhere.ReferencePoint.Player:
                    refPos = Game.instance.Player.vehicle.Position;
                    break;
                case WaveWhere.ReferencePoint.ScreenCenter:
                    refPos = Game.instance.gameCamera.Center;
                    break;
            }

            return new Vector2(
                Mathf.Clamp(position.x, where.clampMin.x + refPos.x, where.clampMax.x + refPos.x),
                Mathf.Clamp(position.y, where.clampMin.y + refPos.y, where.clampMax.y + refPos.y));
        }

        private Waypoint GetRandomFilteredWaypoint()
        {
            float absMinHeight = where.waypointInfo.maxHeight;
            float absMaxHeight = where.waypointInfo.minHeight;

            //Si le joueur n'existe pas, on se met en mode 'screenCenter'
            if (Game.instance.Player == null && where.waypointInfo.referencePoint == WaveWhere.WaypointInfo.ReferencePoint.Player)
                where.waypointInfo.referencePoint = WaveWhere.WaypointInfo.ReferencePoint.ScreenCenter;

            //Get les 'absMinHeight' et 'absMaxHeight'
            switch (where.waypointInfo.referencePoint)
            {
                case WaveWhere.WaypointInfo.ReferencePoint.Player:
                    absMaxHeight = where.waypointInfo.maxHeight + Game.instance.Player.vehicle.Position.y;
                    absMinHeight = where.waypointInfo.minHeight + Game.instance.Player.vehicle.Position.y;
                    break;
                case WaveWhere.WaypointInfo.ReferencePoint.ScreenCenter:
                    absMaxHeight = where.waypointInfo.maxHeight + Game.instance.gameCamera.Height;
                    absMinHeight = where.waypointInfo.minHeight + Game.instance.gameCamera.Height;
                    break;
            }

            //Get le random  waypoint
            if (where.waypointInfo.idType == WaveWhere.WaypointInfo.IdType.Enum)
                return Game.instance.map.mapping.GetRandomWaypoint(where.waypointInfo.waypointType, absMinHeight, absMaxHeight);
            else
                return Game.instance.map.mapping.GetRandomWaypoint(where.waypointInfo.waypointTag, absMinHeight, absMaxHeight);
        }

        private void CheckCompletion()
        {
            for (int i = 0; i < completionCallbacks.Count; i++)
            {
                if (currentKillCount >= completionCallbacks[i].targetKillcount)
                {
                    completionCallbacks[i].action();
                    completionCallbacks.RemoveAt(i);
                    i--;
                }
            }
        }

        private void UpdateTargetKillCount()
        {
            for (int i = 0; i < completionCallbacks.Count; i++)
            {
                completionCallbacks[i].targetKillcount = Mathf.CeilToInt(spawnedUnits * completionCallbacks[i].completionRate);
            }
            CheckCompletion();
        }

        public enum Where { RandomAroundScreen = 0, Waypoints = 1 }

        public class Callback
        {
            public Action action;
            public int targetKillcount;
            public float completionRate;
            public Callback(Action action, float completionRate)
            {
                this.action = action;
                this.completionRate = completionRate;
            }
        }
        public class LevelEventCallback
        {
            public string eventName;
            [InspectorRange(0,1)]
            public float completionRate;
        }
    }
}
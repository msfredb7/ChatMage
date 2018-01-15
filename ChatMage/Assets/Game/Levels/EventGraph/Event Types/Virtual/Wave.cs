using FullInspector;
using FullSerializer;
using LevelScripting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Units/Wave"), DefaultNodeName("Unit Wave")]
    public class Wave : FIVirtualEvent, IEvent
    {
        public Moment onComplete = new Moment();


        [InspectorCategory("What")]
        public _WaveWhat what;
        [InspectorCategory("What")]
        public bool infiniteRepeat = false;
        [InspectorShowIf("infiniteRepeat")]
        public float pauseBetweenRepeat = 0;

        [InspectorCategory("Where")]
        public WaveWhereV2 where;

        [InspectorCategory("Across")]
        public float spawnInterval = 1;

        [NonSerialized, fsIgnore]
        private bool isInfiniteSpawning = false;
        [NonSerialized, fsIgnore]
        private bool stopInfiniteSpawning = false;
        [NonSerialized, fsIgnore]
        private UnitKillsProgress infiniteCallbacker = null;

        /// <summary>
        /// Launch la wave ! (une meme wave peut etre launch plus qu'une fois).
        /// Retourne 'false' si la wave n'a pas peu se launch.
        /// </summary>
        public void Trigger()
        {
            if (infiniteRepeat && isInfiniteSpawning)
            {
                Debug.LogError("Cannot start infinite wave because it is already infiniteSpawning.");
                return;
            }

            stopInfiniteSpawning = false;

            //Get units
            Unit[] units = what.GetSpawnSequence();

            //Get callbacks
            UnitKillsProgress callbacker = what.GetKillsProgress(infiniteRepeat);

            if (onComplete != null && onComplete.HasListeners())
            {
                //Add on complete callback
                if (callbacker == null)
                    callbacker = new UnitKillsProgress(what.TotalUnits);
                callbacker.AddCallback(delegate () { onComplete.Launch(); }, 1f);
            }

            //Get Spawn
            List<UnitSpawn> spawns = Game.Instance.map.mapping.GetSpawns_NewList(where.spawnTag);
            if (spawns == null || spawns.Count == 0)
            {
                if (where.logErrorIfUnableToLaunch)
                    Debug.LogError("Unable to launch wave. No spawns with tag " + where.spawnTag);
                return;
            }
            UnitSpawn spawn = where.FilterAndSelect(spawns);
            if (spawn == null)
            {
                if (where.logErrorIfUnableToLaunch)
                    Debug.LogError("Unable to launch wave. No suitable spawns after filtering. (spawn tag: " + where.spawnTag + ")");
                return;
            }

            //Spawn Action
            Action<Unit> spawnAction = what.GetSpawnAction();

            if (spawn.CanSpawn)
            {

                SimpleEvent cancelAction = null;
                cancelAction = delegate ()
                {
                    if (callbacker != null)
                        callbacker.NoMoreUnitsWillSpawn();
                    spawn.onCancelSpawning -= cancelAction;
                    if (isInfiniteSpawning)
                        StopInfiniteRepeat();
                };

                //On ecoute au cancel de wave
                spawn.onCancelSpawning += cancelAction;



                if (infiniteRepeat)
                {
                    isInfiniteSpawning = true;
                    infiniteCallbacker = callbacker;
                    InfiniteSpawning(false, spawn, units, spawnAction);
                }
                else
                {
                    //Spawn units !
                    if (callbacker != null || spawnAction != null)
                    {
                        spawn.SpawnUnits(units, spawnInterval, delegate (Unit unit)
                        {
                            if (callbacker != null)
                                callbacker.RegisterUnit(unit);
                            if (spawnAction != null)
                                spawnAction(unit);
                        });
                    }
                    else
                        spawn.SpawnUnits(units, spawnInterval);
                }
            }
            else
            {
                if (callbacker != null)
                {
                    callbacker.NoMoreUnitsWillSpawn();
                }
            }
        }

        //public float Duration { get { return (what.TotalUnits - 1) * spawnInterval; } }


        private void InfiniteSpawning(bool insertDelay, UnitSpawn spawn, Unit[] sequence, Action<Unit> spawnAction)
        {
            if (stopInfiniteSpawning)
            {
                OnInfiniteSpawningStopped();
                return;
            }

            int i = 0;
            float realInterval = spawnInterval.Raised(0.1f);
            spawn.SpawnUnits(sequence, realInterval, insertDelay ? (realInterval + pauseBetweenRepeat) : -1, delegate (Unit unit)
            {
                i++;
                if (unit != null)
                {
                    if (infiniteCallbacker != null)
                        infiniteCallbacker.RegisterUnit(unit);
                    if (spawnAction != null)
                        spawnAction(unit);
                }

                //Sommes nous au bout de la sequence ? Si oui, recommancer
                if (i == sequence.Length)
                    InfiniteSpawning(true, spawn, sequence, spawnAction);
            });
        }

        public void StopInfiniteRepeat()
        {
            stopInfiniteSpawning = true;
        }

        private void OnInfiniteSpawningStopped()
        {
            stopInfiniteSpawning = false;
            isInfiniteSpawning = false;
            infiniteCallbacker = null;
        }

        public override void GetAdditionalMoments(out BaseMoment[] moments, out string[] names)
        {
            int count = 0;
            if (what != null)
            {
                if (what.progressCallbacks != null)
                    count = what.progressCallbacks.Length;

                //pour le what.OnSpawn
                if (!what.useLevelScriptMethod)
                    count++;
                else
                {
                    what.onSpawn.ClearMoments();
                    what.onSpawn.unityEvent.RemoveAllListeners();
                }
            }

            moments = new BaseMoment[count];
            names = new string[count];

            int i = 0;

            if (what != null && !what.useLevelScriptMethod)
            {
                moments[i] = what.onSpawn;
                names[i] = "on Spawn";
                i++;
            }

            if (what != null)
                for (int u = 0; u < what.progressCallbacks.Length; u++)
                {
                    _WaveWhat.Callback callback = what.progressCallbacks[u];
                    moments[i] = callback.moment;
                    if (callback.useProgress)
                        names[i] = "at " + Mathf.RoundToInt(callback.atProgress * 100) + "%";
                    else
                        names[i] = "at " + callback.atKillCount + " kills";
                    i++;
                }
        }

        public override string NodeLabel()
        {
            return name;
        }

        public override Color GUIColor()
        {
            return Colors.WAVES;
        }
    }

    public class _WaveWhat
    {
        public UnitPack[] spawnSequence;
        public bool useLevelScriptMethod = false;
        [InspectorShowIf("useLevelScriptMethod")]
        public string onSpawnMethod = "";
        [InspectorHideIf("useLevelScriptMethod")]
        public MomentUnit onSpawn = new MomentUnit();
        public Callback[] progressCallbacks;

        [System.Serializable]
        public class Callback
        {
            public Moment moment;

            public bool useProgress = true;

            [InspectorShowIf("useProgress"), InspectorRange(0, 1)]
            public float atProgress = 1;

            [InspectorHideIf("useProgress")]
            public int atKillCount;
        }

        public Unit[] GetSpawnSequence()
        {
            Unit[] sequence = new Unit[TotalUnits];
            int cursor = 0;

            for (int i = 0; i < spawnSequence.Length; i++)
            {
                for (int j = 0; j < spawnSequence[i].quantity; j++)
                {
                    sequence[cursor] = spawnSequence[i].unit;
                    cursor++;
                }
            }

            return sequence;
        }

        public UnitKillsProgress GetKillsProgress(bool isInfinite)
        {
            if (progressCallbacks == null || progressCallbacks.Length == 0)
                return null;

            UnitKillsProgress callbacker = isInfinite ? new UnitKillsProgress() : new UnitKillsProgress(TotalUnits);

            for (int i = 0; i < progressCallbacks.Length; i++)
            {
                Action action = progressCallbacks[i].moment.Launch;

                if (progressCallbacks[i].useProgress)
                    callbacker.AddCallback(action, progressCallbacks[i].atProgress);
                else
                    callbacker.AddCallback(action, progressCallbacks[i].atKillCount);
            }

            return callbacker;
        }

        public Action<Unit> GetSpawnAction()
        {
            if (useLevelScriptMethod)
            {
                if (string.IsNullOrEmpty(onSpawnMethod))
                    return null;

                Type type = Game.Instance.levelScript.GetType();

                Type[] rq_parameters = new Type[] { typeof(Unit) };

                MethodInfo method = type.GetMethod(onSpawnMethod, rq_parameters);

                if (method == null)
                    return null;

                return delegate (Unit unit)
                {
                    object[] param = new object[] { unit };
                    method.Invoke(Game.Instance.levelScript, param);
                };
            }
            else
            {
                if (onSpawn.HasListeners())
                    return onSpawn.Launch;
                else
                    return null;
            }
        }

        public int TotalUnits
        {
            get
            {
                int count = 0;
                for (int i = 0; i < spawnSequence.Length; i++)
                {
                    count += spawnSequence[i].quantity;
                }
                return count;
            }
        }
    }

}
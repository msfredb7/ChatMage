using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;
using System;

namespace LevelScripting
{
    public class UnitWaveV2
    {
        public event SimpleEvent onLaunched;
        public event SimpleEvent onComplete;

        [InspectorCategory("What")]
        public WaveWhat what;
        [InspectorCategory("What")]
        public bool infiniteRepeat = false;
        [InspectorShowIf("infiniteRepeat")]
        public float pauseBetweenRepeat = 0;

        [InspectorCategory("Where")]
        public WaveWhereV2 where;

        [InspectorCategory("When")]
        public WaveWhen when;

        [InspectorCategory("Across")]
        public float spawnInterval = 1;

        [InspectorCategory("Other")]
        public Dialoguing.Dialog preLaunchDialog;

        [NonSerialized, fsIgnore]
        private bool isInfiniteSpawning = false;
        [NonSerialized, fsIgnore]
        private bool stopInfiniteSpawning = false;
        [NonSerialized, fsIgnore]
        private UnitKillsProgress infiniteCallbacker = null;


        public void ResetData()
        {
            onLaunched = null;
            onComplete = null;
            isInfiniteSpawning = false;
            stopInfiniteSpawning = false;
            infiniteCallbacker = null;
        }

        /// <summary>
        /// Launch la wave ! (une meme wave peut etre launch plus qu'une fois).
        /// Retourne 'false' si la wave n'a pas peu se launch.
        /// </summary>
        public bool LaunchNow(LevelScript levelScript)
        {
            if (infiniteRepeat && isInfiniteSpawning)
            {
                Debug.LogError("Cannot start infinite wave because it is already infiniteSpawning.");
                return false;
            }

            //Get units
            Unit[] units = what.GetSpawnSequence();

            //Get callbacks
            UnitKillsProgress callbacker = what.GetKillsProgress(levelScript, infiniteRepeat);

            if (onComplete != null)
            {
                //Add on complete callback
                if (callbacker == null)
                    callbacker = new UnitKillsProgress(what.TotalUnits);
                callbacker.AddCallback(delegate () { onComplete(); }, 1f);
            }

            //Get Spawn
            List<UnitSpawn> spawns = Game.Instance.map.mapping.GetSpawns_NewList(where.spawnTag);
            if (spawns == null || spawns.Count == 0)
            {
                if (where.logErrorIfUnableToLaunch)
                    Debug.LogError("Unable to launch wave. No spawns with tag " + where.spawnTag);
                return false;
            }
            UnitSpawn spawn = where.FilterAndSelect(spawns);
            if (spawn == null)
            {
                if (where.logErrorIfUnableToLaunch)
                    Debug.LogError("Unable to launch wave. No suitable spawns after filtering. (spawn tag: " + where.spawnTag + ")");
                return false;
            }

            //Spawn Action
            Action<Unit> spawnAction = what.GetSpawnAction(levelScript);

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


            //Launch event
            if (onLaunched != null)
                onLaunched();

            return true;
        }

        public float Duration { get { return (what.TotalUnits - 1) * spawnInterval; } }


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

    }
}

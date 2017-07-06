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

        [InspectorCategory("Where")]
        public WaveWhereV2 where;

        [InspectorCategory("When")]
        public WaveWhen when;

        [InspectorCategory("Across")]
        public float spawnInterval = 1;


        public void ResetData()
        {
            onLaunched = null;
            onComplete = null;
        }

        /// <summary>
        /// Launch la wave ! (une meme wave peut etre launch plus qu'une fois).
        /// Retourne 'false' si la wave n'a pas peu se launch.
        /// </summary>
        public bool LaunchNow(LevelScript levelScript)
        {
            //Get units
            Unit[] units = what.GetSpawnSequence();
            
            //Get callbacks
            UnitKillsProgress callbacker = what.GetKillsProgress(levelScript);

            if(onComplete != null)
            {
                //Add on complete callback
                if (callbacker == null)
                    callbacker = new UnitKillsProgress(what.TotalUnits);
                callbacker.AddCallback(delegate () { onComplete(); }, 1f);
            }

            //Get Spawn
            List<UnitSpawn> spawns = Game.instance.map.mapping.GetSpawns_NewList(where.spawnTag);
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

            //Launch event
            if (onLaunched != null)
                onLaunched();

            return true;
        }

        public float Duration { get { return (what.TotalUnits - 1) * spawnInterval; } }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using System.Reflection;

using UnityEngine.Events;
using CCC.Utility;
using UnityEngine.UI;

public class Fred_TestScript : BaseBehavior
{
    //public ExplosiveMageProjectile projectile;
    //public Transform[] destinations;
    public UnitSpawn spawn;
    public LevelScripting.UnitWaveV2 wave;
    //public List<Unit> unitsToSpawn;
    //public List<float> intervals;
    //public int i = 0;

    void Start()
    {
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    projectile.DuplicateGO(Vector2.zero, Quaternion.identity).GoTo(destinations[i].position);
        //    i++;
        //    if (i == destinations.Length)
        //        i = 0;
        //}


        if (Input.GetKeyDown(KeyCode.T))
        {
            var player = Game.Instance.Player;
            if (player != null)
            {
                var trailsController = player.GetComponent<PlayerDriftTrails>();
                if (trailsController != null)
                {
                    foreach (var trail in trailsController.standardTrailInstances)
                    {
                        if (trail != null)
                            trail.Clear();

                        Debug.Log("clear !");
                    }
                }
            }
            Debug.Log("i did it reddit !");
            //wave.LaunchNow(Game.Instance.levelScript);
            //spawn.SpawnUnits(unitsToSpawn, intervals, 1.5f, (x) => Debug.Log("hello: " + x));
        }
    }
}
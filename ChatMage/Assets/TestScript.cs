using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : BaseBehavior
{
    public RubanPlayer rubanPlayer;
    public MapFollower follower;

    void Start()
    {
       rubanPlayer.StartNewPlaylist("test 1");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Game.instance.Player.vehicle.movingPlatform = rubanPlayer;
            //follower.Follow(Game.instance.Player.transform, rubanPlayer);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            rubanPlayer.UnStop();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            rubanPlayer.Stop();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Game.instance.Player.vehicle.TimeScale = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Game.instance.Player.vehicle.TimeScale = 2;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Game.instance.Player.vehicle.TimeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Game.instance.Player.vehicle.Bump(Vector2.right*5,0.1f, BumpMode.VelocityAdd);
        }
    }
}

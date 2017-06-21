using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocations : PlayerComponent
{
    /// <summary>
    /// Arriere du vehicule
    /// </summary>
    public Transform boule;
    public Transform[] wheels;

    public Transform FrontLeftWheel { get { return wheels[0]; } }
    public Transform FrontRightWheel { get { return wheels[1]; } }
    public Transform BackLeftWheel { get { return wheels[2]; } }
    public Transform BackRightWheel { get { return wheels[3]; } }


    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }
}

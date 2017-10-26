using CCC.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotorSounds : PlayerComponent
{
    [Header("Motor")]
    public AudioSource motorSource;

    [Header("Pitch Curve")]
    public float minPitch = 1;
    public float maxPitch = 1.5f;
    public float curvature = 1;
    public float exponent = 2;

    [Header("Variation")]
    public float pitchAccelerationSpeed = 0.5f;
    public float pitchDecelerationSpeed = 0.5f;

    [Header("Turning")]
    public float turningPitchMultiplier = 0.9f;

    [Header("Volume")]
    public float minVolume = 0.1f;
    public float maxVolume = 0.3f;

    PlayerVehicle veh;
    PlayerDriver driver;
    NeverReachingCurve motorCurve;

    public override void OnGameReady()
    {
        veh = controller.vehicle;
        driver = controller.playerDriver;
        motorCurve = new NeverReachingCurve(minPitch, maxPitch, curvature);
    }

    public override void OnGameStarted()
    {

    }

    void Update()
    {
        if (veh == null)
            return;

        float moveSpeed = veh.RealMoveSpeed();
        float x = veh.Speed.sqrMagnitude / (moveSpeed * moveSpeed);
        x = x.Powed(exponent);
        
        float targetPitch = motorCurve.Evalutate(x);
        if (driver.IsTurning)
            targetPitch *= turningPitchMultiplier;
        float currentPitch = motorSource.pitch;
        float pitchSpeed = currentPitch < targetPitch ? pitchAccelerationSpeed : pitchDecelerationSpeed;

        motorSource.pitch = currentPitch.MovedTowards(targetPitch, pitchSpeed * veh.DeltaTime());
        motorSource.volume = (currentPitch - minPitch) / (maxPitch - minPitch) * (maxVolume - minVolume) + minVolume;

    }

    void OnValidate()
    {
        motorCurve = new NeverReachingCurve(minPitch, maxPitch, curvature);
    }
}

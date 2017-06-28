using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;

public class ITM_FlameDrift : Item
{
    [InspectorHeader("Trail Setting")]
    public FlameDrift trailPrefab;
    public float trailLength;
    public float segmentLength;

    [InspectorHeader("Drift Setting")]
    public float minSpeed = 2.5f;
    public float minDriftDuration = 0.4f;

    [NonSerialized, fsIgnore]
    private Trail trail;

    [NonSerialized, fsIgnore]
    private float lastTurnDir = 0;
    [NonSerialized, fsIgnore]
    private float currentDriftTime = 0;
    [NonSerialized, fsIgnore]
    private float sqrMinSpeed;
    [NonSerialized, fsIgnore]
    private bool flameIsOn = false;
    [NonSerialized, fsIgnore]
    private FlameDrift currentFlameDrift;

    public override void OnGameReady()
    {
        trail = new Trail(player.transform, trailLength, segmentLength);
        sqrMinSpeed = minSpeed * minSpeed;

        currentFlameDrift = Instantiate(trailPrefab.gameObject, player.transform).GetComponent<FlameDrift>();
        currentFlameDrift.transform.localPosition = Vector2.zero;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {
        float currentTurnDir = Mathf.Sign(player.playerDriver.LastHorizontalInput);

        if (player.playerDriver.LastHorizontalInput != 0 &&         //Input is not 0
            lastTurnDir == currentTurnDir &&                        //Input is same as last frame
            player.vehicle.rb.velocity.sqrMagnitude > sqrMinSpeed)  //Player velocity > min speed
        {
            if (currentDriftTime >= minDriftDuration)
            {
                //Flame drift enabled
                FlameOn();
            }
            else
            {
                //Flame drift charging
                currentDriftTime += player.vehicle.DeltaTime();
            }
        }
        else
        {
            FlameOff();
        }

        lastTurnDir = currentTurnDir;

        trail.Update();
    }

    void FlameOn()
    {
        if (!flameIsOn)
        {
            flameIsOn = true;
            currentFlameDrift.StartTrail();
        }
    }
    void FlameOff()
    {
        if (flameIsOn)
        {
            flameIsOn = false;
            currentFlameDrift.EndTrail();
        }
        currentDriftTime = 0;
    }
}

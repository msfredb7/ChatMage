using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrift : MonoBehaviour
{
    [Header("Drift Settings")]
    public float driftAfterXs = 0.5f;

    public event SimpleEvent onStartDrift;
    public event SimpleEvent onEndDrift;

    public bool IsDrifting() { return drifting; }
    public float GetDriftTime() { return driftTimer; }

    private bool drifting = false;
    private PlayerVehicle veh;
    private PlayerDriver driver;

    private float lastInput;
    private float driftTimer;

    void Start()
    {
        PlayerController controller = GetComponent<PlayerController>();
        driver = controller.playerDriver;
        veh = controller.vehicle;
    }

    void Update()
    {
        float input = driver.LastHorizontalInput;

        if (input != 0 && input == lastInput)
        {
            driftTimer += veh.DeltaTime();

            if (driftTimer > driftAfterXs && !drifting)
                StartDrift();
        }
        else
        {
            driftTimer = 0;
            if (drifting)
                StopDrift();
        }

        lastInput = input;
    }

    private void StartDrift()
    {
        if (drifting)
            return;

        drifting = true;
        if (onStartDrift != null)
            onStartDrift();
    }

    private void StopDrift()
    {
        if (!drifting)
            return;

        drifting = false;
        if (onEndDrift != null)
            onEndDrift();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class Car_JetPack : Car, IFixedUpdate
{
    [InspectorHeader("Double Jet")]
    public float doubleJetBoost = 5;

    [InspectorMargin(12), InspectorHeader("Single Jet")]
    public float singleJetBoost = 2;
    public float rotationalForce = 5;
    public float jetTilt;

    [NotSerialized, fsIgnore]
    private int lastTouchCount;
    [NotSerialized, fsIgnore]
    private float lastHorizontalInput;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        player.vehicle.wheelsOnTheGround.LockUnique("car");
    }

    public override void OnInputUpdate(float horizontalInput)
    {
        if (Application.isEditor)
        {
            lastTouchCount = 0;
            if (Input.GetKey(KeyCode.LeftArrow))
                lastTouchCount++;
            if (Input.GetKey(KeyCode.RightArrow))
                lastTouchCount++;
        }
        else
        {
            lastTouchCount = Input.touchCount;
        }
        lastHorizontalInput = horizontalInput;
    }

    public override void OnUpdate()
    {

    }

    public void RemoteFixedUpdate()
    {
        Vehicle veh = player.vehicle;

        if (!veh.gameObject.activeSelf)
            return;

        float boost = 0;
        float angularBoost = 0;
        float tilt = 0;

        if (lastHorizontalInput == 0)
        {
            if (lastTouchCount > 0)
                boost = doubleJetBoost;
        }
        else
        {
            boost = singleJetBoost;
            angularBoost = -lastHorizontalInput * rotationalForce;
            tilt = -lastHorizontalInput * jetTilt;
        }

        if (veh.canMove && boost > 0)
        {
            Vector2 dir = veh.WorldDirection2D();
            if (tilt != 0)
                dir = dir.Rotate(tilt);
            veh.rb.AddForce(dir * boost, ForceMode2D.Force);
        }

        if(veh.canTurn && angularBoost != 0)
            veh.rb.AddTorque(angularBoost, ForceMode2D.Force);
    }
}

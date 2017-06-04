using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using CCC.Utility;

public class Car_Robot : Car
{
    //NE PAS MODIFIER IN-GAME
    public float turnClutch = 0;
    public float turnAcceleration = 5;
    public float turnSpeed = 185;
    public float moveSpeed = 6;
    public float changeDirectionCooldown = 0.25f;

    [fsIgnore]
    float horizontal = 0;
    [fsIgnore]
    float lastHorizontal = 0;
    [fsIgnore]
    float cooldown = 0; 

    public override void OnInputUpdate(float horizontalInput)
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Get movement of the finger since last frame
                Vector2 touchDeltaPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - player.transform.position;
                RotateTo(touchDeltaPosition);
            }
            else if (Input.GetMouseButton(0))
            {
                // Get movement of the mouse since last frame
                Vector2 mouseDeltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
                RotateTo(mouseDeltaPosition);
            }
        }
    }

    void RotateTo(Vector2 direction)
    {
        player.vehicle.TeleportDirection(Vehicle.VectorToAngle(direction));
        cooldown = changeDirectionCooldown;
    }

    public override void Init(PlayerController player)
    {
        base.Init(player);
        //On met la boule à 0 pour ne pas fuck les truc qu'on tire
        player.playerLocations.boule.localPosition = Vector3.zero;
    }

    public override void OnGameReady()
    {
        player.vehicle.MoveSpeed = moveSpeed;
        player.vehicle.useWeight = false;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}

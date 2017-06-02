using CCC.Utility;
using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Rocket : Car
{
    //NE PAS MODIFIER IN-GAME
    public float turnAcceleration = 0;
    public float turnSpeed = 1000;
    public float maxSpeed = 10;
    public float minSpeed = 1;
    public float boostDuration = 0.5f;
    public float rocketWeight = 10;
    public float startRotatingRatio = 2;
    public float intervalFromMinimum = 0.5f;

    [fsIgnore]
    float horizontal = 0;
    [fsIgnore]
    float lastHorizontal = 0;

    private bool wasTurning = false;
    private bool wasAccelerating = false;
    private bool firstime = true;

    public override void OnInputUpdate(float horizontalInput)
    {
        // Si on ne veut pas tourner
        if (horizontalInput == 0)
        {
            // Si on était en train de tourner
            if (wasTurning)
            {
                // Si le vehicle ne pouvait pas bouger
                if(!player.vehicle.canMove)
                {
                    // On relâche le véhicule, il peut maintenant avancer
                    player.vehicle.canMove.Unlock("rocket");
                    player.vehicle.wheelsOnTheGround.Unlock("rocket");

                    // Vitesse initiale au max
                    player.vehicle.moveSpeed = maxSpeed;
                }
                wasTurning = false;
            }

            UpdateAcceleration();

            UpdateSpeed();
        }
        else // Si on veut tourner
        {
            // Si on était pas en train de tourner
            if (!wasTurning)
            {
                // Si on était en mouvement libre
                if (wasAccelerating)
                {
                    UpdateRotation(horizontalInput);
                    wasTurning = true;
                } else if (!player.vehicle.canMove)
                {
                    UpdateRotation(horizontalInput);
                    wasTurning = true;
                }
                UpdateAcceleration();
            } else
            {
                if (!player.vehicle.wheelsOnTheGround || !player.vehicle.canMove)
                    UpdateRotation(horizontalInput);
                if (firstime)
                    UpdateAcceleration();
            }

            UpdateSpeed();
        }
    }

    public override void OnGameReady()
    {
        player.vehicle.weight = rocketWeight;
        player.vehicle.moveSpeed = maxSpeed;
        wasAccelerating = false;
        wasTurning = false;
        firstime = true;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }

    void UpdateSpeed()
    {
        // Si le joueur peut bouger
        if (player.vehicle.canMove)
        {
            // On réduit ca vitesse graduellement
            float speedReduction = (player.vehicle.moveSpeed - minSpeed) / (FPSCounter.GetFPS() * boostDuration);
            if ((player.vehicle.moveSpeed - speedReduction) > (minSpeed + intervalFromMinimum))
                player.vehicle.moveSpeed -= speedReduction;
            else // Si le vehicule a atteint sa vitesse minimum
            {
                // On immobilise le véhicule
                player.vehicle.moveSpeed = minSpeed;
                player.vehicle.canMove.LockUnique("rocket");
                player.vehicle.wheelsOnTheGround.Unlock("rocket");
                wasAccelerating = false;
            }
        }
    }

    void UpdateRotation(float horizontalInput)
    {
        // Boute qui gère le tournage

        if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
            lastHorizontal = 0;

        horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
        player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();
    }

    void UpdateAcceleration()
    {
        // Si on est proche d'être à la vitesse minimum
        if (player.vehicle.moveSpeed < (maxSpeed - minSpeed) / startRotatingRatio)
        {
            if (!wasAccelerating)
            {
                // On peut tourner comme on veut en même temps d'avancer dans la même direction qu'avant
                player.vehicle.wheelsOnTheGround.LockUnique("rocket");
                wasAccelerating = true;

                if (firstime)
                    firstime = false;
            }
        }
    }
}

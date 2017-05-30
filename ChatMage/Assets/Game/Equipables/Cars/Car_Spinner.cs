using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using CCC.Utility;

public class Car_Spinner : Car
{
    //NE PAS MODIFIER IN-GAME
    public float turnClutch = 0;
    public float turnAcceleration = 5;
    public float turnSpeed = 185;
    public float moveSpeed = 6;
    public int comboAmountRequire = 6;
    public float spinAcceleration = 5;
    public float spinSpeed = 500;
    public float comboCountdown = 0.4f;
    public float spinDuration = 1f;

    [fsIgnore]
    float horizontal = 0;
    [fsIgnore]
    float lastHorizontal = 0;

    float startTurnSpeed;
    float startTurnAcceleration;

    bool listenningLeft;
    bool listenningRight;
    bool spinning;
    int comboAmount = 0;
    float countdown = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        // Si on a reussi a faire le combo
        if (comboAmount >= comboAmountRequire){
            // mais qu'on etait pas en train de spin
            if (!spinning)
            {
                // init le spin
                turnSpeed = spinSpeed;
                turnAcceleration = spinAcceleration;
                spinning = true;
                countdown = spinDuration;
            }
            // set le spin
            horizontal = Mathf.MoveTowards(0, 1, player.vehicle.DeltaTime() * turnAcceleration);
        }else if (horizontalInput == 0 || !player.playerStats.canTurn)
            horizontal = 0; // tout drette
        else
        {
            // Si on a appuyer sur la droite
            if(horizontalInput == 1)
            {
                if (listenningRight) // Sinon es ce qu'on ecoutait pour la droite
                {
                    Debug.Log("Combo ++");
                    // on augmente le combo et on arrete d'ecouter pour la droite
                    comboAmount++;
                    listenningRight = false;
                    // on ecoute a gauche et on start le countdown
                    listenningLeft = true;
                    countdown = comboCountdown;
                }
                
                if (!listenningLeft) // et qu'on etait pas deja en train d'ecouter pour la gauche
                {
                    // on ecoute a gauche et on start le countdown
                    Debug.Log("Listenning to Left");
                    listenningLeft = true;
                    countdown = comboCountdown;
                }

            } else if (horizontalInput == -1) // Si on a appuer sur la gauche
            {
                if (listenningLeft)// Sinon es ce qu'on ecoutait pour la gauche
                {
                    Debug.Log("Combo ++");
                    // on augmente le combo et on arrete d'ecouter pour la gauche
                    comboAmount++;
                    listenningLeft = false;
                    // on ecoute a droite et on start le countdown
                    listenningRight = true;
                    countdown = comboCountdown;
                }

                // et qu'on etait pas deja en train d'ecouter pour la droite
                if (!listenningRight)
                {
                    Debug.Log("Listenning to Right");
                    // on ecoute a droite et on start le countdown
                    listenningRight = true;
                    countdown = comboCountdown;
                }
            }

            // Boute du tournage

            if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                lastHorizontal = 0;

            if (Mathf.Abs(horizontalInput) > turnClutch && Mathf.Abs(lastHorizontal) < turnClutch)
                lastHorizontal = horizontalInput * turnClutch;

            horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
        }

        player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();

        lastHorizontal = horizontal;

        countdown -= Time.deltaTime; // on reduit le countdown

        // Le countdown est termine, on reset tout
        if(countdown <= 0)
        {
            Debug.Log("Countdown Over");
            listenningLeft = false;
            listenningRight = false;
            comboAmount = 0;
            // Si jamais on etait en train de spin
            if (spinning)
            {
                // le countdown du spin est fini on reset toute
                turnSpeed = startTurnSpeed;
                turnAcceleration = startTurnAcceleration;
                spinning = false;
            }
        }


    }

    public override void OnGameReady()
    {
        // On reset toute au cas ou
        player.vehicle.moveSpeed = moveSpeed;
        listenningLeft = false;
        listenningRight = false;
        spinning = false;
        comboAmount = 0;
        countdown = 0;
        startTurnSpeed = turnSpeed;
        startTurnAcceleration = turnAcceleration;
        turnSpeed = startTurnSpeed;
        turnAcceleration = startTurnAcceleration;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}


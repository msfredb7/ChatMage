using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;

public abstract class StdCar : Car
{
    [InspectorMargin(12), InspectorHeader("Base Stats")]
    public float turnAcceleration = 5;
    public float turnSpeed = 185;
    public float moveSpeed = 6;
    public float weight = 0.2f;

    [InspectorHeader("Spin Stats")]
    public float startTurnSpeed = 185;
    public float startTurnAcceleration = 5;
    public int comboAmountRequire = 6;
    public float spinAcceleration = 5;
    public float spinSpeed = 500;
    public float comboCountdown = 0.4f;
    public float spinDuration = 1f;

    [fsIgnore, System.NonSerialized]
    protected float horizontal = 0;
    [fsIgnore, System.NonSerialized]
    protected float lastHorizontal = 0;

    bool listenningLeft;
    bool listenningRight;
    bool spinning;
    int comboAmount = 0;
    float countdown = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        if (player.vehicle.CanSpin || spinning)
        {
            // Si on a reussi a faire le combo
            if (comboAmount >= comboAmountRequire)
            {
                // mais qu'on etait pas en train de spin
                if (!spinning)
                {

                    // init le spin
                    var item = player.playerItems.GetAReferenceToItemOfType<ITM_Spinner>();
                    if (item != null)
                        DefaultAudioSources.PlaySFX(((ITM_Spinner)item).spinnerSFX);

                    turnSpeed = spinSpeed;
                    turnAcceleration = spinAcceleration;
                    spinning = true;
                    countdown = spinDuration;
                }
                // set le spin
                horizontal = Mathf.MoveTowards(0, 1, player.vehicle.DeltaTime() * turnAcceleration);
            }
            else if (horizontalInput == 0 || !player.playerStats.receivesTurnInput)
                horizontal = 0; // tout drette
            else
            {
                // Si on a appuyer sur la droite
                if (horizontalInput == 1)
                {
                    if (listenningRight) // Sinon es ce qu'on ecoutait pour la droite
                    {
                        //Debug.Log("Combo ++");
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
                        //Debug.Log("Listenning to Left");
                        listenningLeft = true;
                        countdown = comboCountdown;
                    }

                }
                else if (horizontalInput == -1) // Si on a appuer sur la gauche
                {
                    if (listenningLeft)// Sinon es ce qu'on ecoutait pour la gauche
                    {
                        //Debug.Log("Combo ++");
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
                        //Debug.Log("Listenning to Right");
                        // on ecoute a droite et on start le countdown
                        listenningRight = true;
                        countdown = comboCountdown;
                    }
                }

                // Boute du tournage

                if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                    lastHorizontal = 0;

                horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
            }

            player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();

            lastHorizontal = horizontal;

            countdown -= Time.deltaTime; // on reduit le countdown

            // Le countdown est termine, on reset tout
            if (countdown <= 0)
            {
                //Debug.Log("Countdown Over");
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
        else
        {
            if (horizontalInput == 0 || !player.playerStats.receivesTurnInput)
                horizontal = 0;
            else
            {
                if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                    lastHorizontal = 0;

                horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
            }

            player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();

            lastHorizontal = horizontal;
        }
    }

    public override void OnGameReady()
    {
        Vehicle veh = player.vehicle;
        veh.MoveSpeed = moveSpeed;
        veh.weight = weight;

        // On reset toute au cas ou
        player.vehicle.MoveSpeed = moveSpeed;
        listenningLeft = false;
        listenningRight = false;
        spinning = false;
        comboAmount = 0;
        countdown = 0;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusBrain : EnemyBrain<JesusVehicle> {

    public JesusVehicle jesus;
    public JesusRock rockPrefab;

    public int rocksMaxAmount = 2;
    public float distanceRockIsConsiderClose = 5f;
    public float shoutingDuration = 1f;
    public float shoutingCooldown = 4f;
    public float throwingCooldown = 2f;

    private List<JesusRock> rocks = new List<JesusRock>();
    private JesusRock rockTarget = null;

    private bool hasRock = false;
    private bool shouting = false;
    private bool ignoreNextFrame = false;

    private float shoutingCountdown = 0;
    private float throwingCountdown = 0;

    protected override void UpdatePlayer()
    {
        if (!ignoreNextFrame)
        {
            // Si on a une roche en notre possession
            if (hasRock)
            {
                // Lancer la roche qu'on vient de prendre
                if(throwingCountdown < 0)
                {
                    LancerNouvelleRoche();
                    hasRock = false;
                }
            }
            else
            {   // S'il y a une roche proche
                if (IsAnyRockClose())
                {
                    // on se dirige vers la roche la plus proche
                    SearchForARock();
                }
                else
                {
                    // Si on peut lancer une nouvelle roche
                    if (rocks.Count < rocksMaxAmount)
                    {
                        // Lancer une nouvelle roche
                        if (throwingCountdown < 0)
                            LancerNouvelleRoche();
                    }
                    else
                    {
                        // Si on criait pas
                        if (!shouting)
                        {
                            // et que ca fait un boute qu'on a pas crier
                            if (shoutingCooldown < 0)
                            {
                                // Crie
                                Debug.Log("RAAAWWWR");
                                SetBehavior(BehaviorType.Idle);
                                // ANIMATION CRIER
                                shoutingCountdown = shoutingDuration;
                                shouting = true;
                            } else
                                SearchForARock();
                        }
                        else
                        {
                            // Apres avoir crier
                            if (shoutingCooldown < 0)
                            {
                                // on se dirige vers la roche la plus proche
                                SearchForARock();
                                shoutingCountdown = shoutingCooldown;
                                shouting = false;
                            }
                        }
                    }
                }
            }
            shoutingCountdown -= player.vehicle.DeltaTime();
            throwingCountdown -= player.vehicle.DeltaTime();
        }
        ignoreNextFrame = false;
    }

    private void SearchForARock()
    {
        Debug.Log("Searching for the rock");
        // Trouver la plus proche
        rockTarget = ClosestRock();

        // Aller la chercher
        SetBehavior(BehaviorType.Wander);

        // On essaie de prendre la roche
        if (rockTarget.TakeTheRock(transform.position))
            hasRock = true;
    }

    private void LancerNouvelleRoche()
    {
        Debug.Log("Launching the rock");
        // On regarde le joueur et on lance!
        SetBehavior(BehaviorType.LookPlayer);

        // Spawn Roche et 
        JesusRock currentRock = Game.instance.SpawnUnit(rockPrefab, transform.position);
        currentRock.onRockTaken += delegate () { rocks.Remove(currentRock); };
        rocks.Add(currentRock);

        // ANIMATION DE LANCER UNE ROCHE

        // LANCAGE DE ROCHE ICI
        Vector2 normalizeDirection = (player.vehicle.Position - vehicle.Position).normalized;
        currentRock.Speed = normalizeDirection * 5;
        ignoreNextFrame = true;

        throwingCountdown = throwingCooldown;
    }

    private JesusRock ClosestRock()
    {
        JesusRock result = null;
        for (int i = 0; i < rocks.Count; i++)
        {
            if (result == null)
            {
                result = rocks[i];
                break;
            }
            if (Vector2.Distance(rocks[i].Position, transform.position) < Vector2.Distance(result.Position,transform.position))
                result = rocks[i];
        }
        return result;
    }

    private bool IsAnyRockClose()
    {
        bool result = false;
        for (int i = 0; i < rocks.Count; i++)
        {
            if(Vector2.Distance(rocks[i].Position,transform.position) < distanceRockIsConsiderClose)
                result = true;
        }
        return result;
    }

    protected override void UpdateNoPlayer()
    {
        SetBehavior(BehaviorType.Idle);
    }

    protected override EnemyBehavior NewWanderBehavior()
    {
        return new SearchingRockBehavior(vehicle, rockTarget);
    }
}

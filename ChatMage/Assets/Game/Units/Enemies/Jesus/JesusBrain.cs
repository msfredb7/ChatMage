using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusBrain : EnemyBrain<JesusVehicle> {

    public JesusVehicle jesus;
    public JesusRock rockPrefab;

    public int rocksMaxAmount = 2;
    public float distanceRockIsConsiderClose = 5f;
    public float shoutingDuration = 3f;

    private List<JesusRock> rocks = new List<JesusRock>();
    private JesusRock rockTaken = null;
    private JesusRock rockTarget = null;

    private bool hasRock = false;
    private bool shouting = false;

    private float countdown = 0;

    protected override void UpdatePlayer()
    {
        // Si on a une roche en notre possession
        if (hasRock)
        {
            // Lancer la roche qu'on vient de prendre
            LancerNouvelleRoche();
            hasRock = false;
        } else
        {   // S'il y a une roche proche
            if (IsAnyRockClose())
            {
                // on se dirige vers la roche la plus proche
                SearchForARock();
                countdown = shoutingDuration;
            } else
            {
                // Si on peut lancer une nouvelle roche
                if(rocks.Count < rocksMaxAmount)
                {
                    // Lancer une nouvelle roche
                    LancerNouvelleRoche();
                } else
                {
                    // Si on criait pas
                    if (!shouting)
                    {
                        // et que ca fait un boute qu'on a pas crier
                        if(countdown < 0)
                        {
                            // Crie
                            SetBehavior(BehaviorType.Idle);
                            // ANIMATION CRIER
                            countdown = shoutingDuration;
                            shouting = true;
                        }
                    } else
                    {
                        // Apres avoir crier
                        if (countdown < 0)
                        {
                            // on se dirige vers la roche la plus proche
                            SearchForARock();
                            countdown = shoutingDuration;
                            shouting = false;
                        }
                    }
                }
            }
        }
        countdown -= player.vehicle.DeltaTime();
    }

    private void SearchForARock()
    {
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
        // On regarde le joueur et on lance!
        SetBehavior(BehaviorType.LookPlayer);

        // Spawn Roche et 
        JesusRock currentRock = Game.instance.SpawnUnit(rockPrefab, transform.position);
        rocks.Add(currentRock);

        // Calculer la future position du joueur
        // donner la force qu'il faut pour qu'elle aille a cette position

        // ANIMATION DE LANCER UNE ROCHE
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

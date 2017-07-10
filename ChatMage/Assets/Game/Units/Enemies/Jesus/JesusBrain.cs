using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class JesusBrain : EnemyBrain<JesusVehicle>
{

    public JesusRock rockPrefab;

    [InspectorHeader("Jesus Values")]
    public int rocksMaxAmount = 2;
    public float distanceRockIsClose = 5f;
    public float shoutingDuration = 1f;
    public float shoutingCooldown = 4f;
    public float throwingCooldown = 2f;
    public bool damagableWhilePickingRock = false;

    private List<JesusRock> rocks = new List<JesusRock>();

    private bool hasRock = false;
    private bool shouting = false;
    private bool ignoreNextFrame = false;
    [HideInInspector]
    public bool pickingRockAnimationOver = false;

    private float shoutingCountdown = 0;
    private float throwingCountdown = 0;

    protected override void UpdateWithTarget()
    {
        if (!ignoreNextFrame)
        {
            // Si on a une roche en notre possession
            if (hasRock)
            {
                if (pickingRockAnimationOver)
                {
                    // Lancer la roche qu'on vient de prendre
                    if (throwingCountdown < 0)
                    {
                        LancerNouvelleRoche();
                        hasRock = false;
                    }
                }
            }
            else
            {
                // Si une roche est vraiment proche
                if (IsAnyRockClose())
                {
                    // on va la chercher
                    SearchForARock();
                }
                else
                {
                    // Si on criait pas
                    if (!shouting)
                    {
                        // es ce que c le temps de crier ?
                        if (shoutingCountdown < 0)
                        {
                            // Crie
                            FlashAnimation.FlashColor(vehicle, vehicle.animator.render, shoutingDuration, Color.red, null);

                            if (CanGoTo<IdleBehavior>())
                                SetBehavior(new IdleBehavior(vehicle));
                            // ANIMATION CRIER
                            shoutingCountdown = shoutingDuration;
                            shouting = true;
                        }
                        else
                        {
                            TryToThrowNewRock();
                        }
                    }
                    else // Si on crie
                    {
                        // Apres avoir crier
                        if (shoutingCountdown < 0)
                        {
                            shouting = false;
                            shoutingCountdown = shoutingCooldown;

                            TryToThrowNewRock();
                        }
                    }
                }
            }
            shoutingCountdown -= myVehicle.DeltaTime();
            throwingCountdown -= myVehicle.DeltaTime();
        }
        ignoreNextFrame = false;
    }

    private void TryToThrowNewRock()
    {
        // Si on peut pas lancer une nouvelle roche
        if (rocks.Count >= rocksMaxAmount)
        {
            // on se dirige vers la roche la plus proche
            SearchForARock();
        }
        else
        {
            // Lancer une nouvelle roche
            if (throwingCountdown < 0)
                LancerNouvelleRoche();
        }
    }

    private void SearchForARock()
    {
        // Trouver la plus proche
        JesusRock rockTarget = ClosestRock();

        // Aller la chercher
        if (CanGoTo<SearchingRockBehavior>())
            SetBehavior(new SearchingRockBehavior(vehicle, rockTarget));

        // On essaie de prendre la roche
        if (rockTarget.TakeTheRock(transform.position))
        {
            pickingRockAnimationOver = false;
            // ANIMATION DE PRENDRE LA ROCHE
            pickingRockAnimationOver = true; // quand l'animation termine
            hasRock = true;
        }
    }

    private void LancerNouvelleRoche()
    {
        Debug.Log("Launching the rock");

        // On regarde le joueur et on lance!
        if (CanGoTo<LookTargetBehavior>())
            SetBehavior(new LookTargetBehavior(vehicle));

        // Spawn Roche et 
        JesusRock currentRock = Game.instance.SpawnUnit(rockPrefab, transform.position);
        currentRock.onRockTaken += delegate () { rocks.Remove(currentRock); };
        rocks.Add(currentRock);

        // ANIMATION DE LANCER UNE ROCHE

        // LANCAGE DE ROCHE ICI
        Vector2 normalizeDirection = (target.Position - vehicle.Position).normalized;
        currentRock.Speed = normalizeDirection;
        ignoreNextFrame = true;

        // Temps avant le prochain lancement de roche
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
            if (Vector2.Distance(rocks[i].Position, transform.position) < Vector2.Distance(result.Position, transform.position))
                result = rocks[i];
        }
        return result;
    }

    private bool IsAnyRockClose()
    {
        // Si on vient de lancer une roche
        if (throwingCountdown > 0) // A ENLEVER AU BESOIN
            return false; // On doit pas aller chercher une roche proche de suite

        bool result = false;
        for (int i = 0; i < rocks.Count; i++)
        {
            if (Vector2.Distance(rocks[i].Position, transform.position) < distanceRockIsClose)
                result = true;
        }
        return result;
    }

    protected override void UpdateWithoutTarget()
    {
        if (CanGoTo<IdleBehavior>())
            SetBehavior(new IdleBehavior(vehicle));
    }

    public void ResetCooldowns()
    {
        throwingCountdown = throwingCooldown;
        shoutingCountdown = shoutingCooldown;
    }
}

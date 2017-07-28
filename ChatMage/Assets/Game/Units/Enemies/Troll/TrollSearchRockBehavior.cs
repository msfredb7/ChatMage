using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrollSearchRockBehavior : EnemyBehavior<TrollVehicle>
{
    private const float ROCK_PICKUP_DIST = 2.25f; //CETTE DISTANCE EST ^2

    private JesusRockV2 rockTarget;
    private List<JesusRockV2> availableRocks;
    private Action<JesusRockV2> onComplete;

    public TrollSearchRockBehavior(TrollVehicle vehicle, JesusRockV2 myRock, Action<JesusRockV2> onComplete) : base(vehicle)
    {
        this.rockTarget = myRock;
        this.onComplete = onComplete;
    }

    public override void Enter(Unit target)
    {
        GetAvailableRocks();
    }

    public override void Exit(Unit target) { }

    public override void Update(Unit target, float deltaTime)
    {
        if (rockTarget != null)
        {
            //Vector entre moi et la roche
            Vector2 meToRock = rockTarget.Position - vehicle.Position;

            //Distance entre la roche et moi
            float sqrDist = meToRock.sqrMagnitude;


            //Some nous assez proche de la roche ?
            if (sqrDist < ROCK_PICKUP_DIST)
            {
                //On stop le vehicle
                vehicle.Stop();

                float meToRockAngle = CCC.Math.Vectors.VectorToAngle(meToRock);
                float deltaAngle = Mathf.Abs(vehicle.Rotation - meToRockAngle);

                //Est-ce qu'on regarde vers la roche ?
                if (deltaAngle < 5)
                {
                    //On a terminer de chercher une roche !
                    onComplete(rockTarget);
                }
                else
                {
                    //On fait simplement se tourner vers la roche
                    vehicle.TurnToDirection(meToRockAngle, deltaTime);
                }
            }
            else
            {
                //On avance vers la roche
                vehicle.GotoDirection(meToRock, deltaTime);
            }
        }
        else
        {
            //Trouvons une roche proche
            rockTarget = GetClosestRock();
        }
    }

    /// <summary>
    /// Mets tout les roches qui sont dans le jeu dans une liste
    /// </summary>
    private void GetAvailableRocks()
    {
        availableRocks = new List<JesusRockV2>();

        LinkedListNode<Unit> node = Game.instance.units.First;
        while (node != null)
        {
            Unit val = node.Value;

            if (val is JesusRockV2)
            {
                availableRocks.Add(val as JesusRockV2);
            }

            node = node.Next;
        }
    }

    /// <summary>
    /// Parmis les roche availables, laquel est la plus proche
    /// </summary>
    private JesusRockV2 GetClosestRock()
    {
        float minDist = float.PositiveInfinity;
        JesusRockV2 recordHolder = null;

        for (int i = 0; i < availableRocks.Count; i++)
        {
            if (availableRocks[i].IsFlying)
                continue;

            float dist = (availableRocks[i].Position - vehicle.Position).sqrMagnitude;

            if (dist < minDist)
            {
                recordHolder = availableRocks[i];
                minDist = dist;
            }
        }
        return recordHolder;
    }
}

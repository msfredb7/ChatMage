using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class JesusV2Brain : EnemyBrainV2<JesusV2Vehicle>
    {
        List<JesusRockV2> availableRocks;
        JesusRockV2 chosenRock;
        Unit target;

        void Start()
        {
            AddGoal(new Goal_Wander(veh));
        }

        protected override void Update()
        {
            base.Update();

            if(availableRocks == null)
                GetAvailableRocks();

            if (target == null)
            {
                //On cherche une cible
                target = veh.targets.TryToFindTarget(veh);
            }
            else
            {
                //A-t-on une roche ?
                if (chosenRock == null)
                {
                    //On cherche la roche
                    chosenRock = GetClosestRock();

                    //On en a trouver une ?
                    if (chosenRock != null)
                    {
                        //On lance l'attaque
                        Goal attackGoal = new JesusGoal_Attack(veh, chosenRock, target);
                        attackGoal.onRemoved = (Goal g) => { target = null; chosenRock = null; };
                        AddGoal(attackGoal);
                    }
                }
            }
        }

        private JesusRockV2 GetClosestRock()
        {
            float minDist = float.PositiveInfinity;
            JesusRockV2 recordHolder = null;

            for (int i = 0; i < availableRocks.Count; i++)
            {
                if (availableRocks[i].IsFlying)
                    continue;

                float dist = (availableRocks[i].Position - veh.Position).sqrMagnitude;

                if (dist < minDist)
                {
                    recordHolder = availableRocks[i];
                    minDist = dist;
                }
            }
            return recordHolder;
        }

        private void GetAvailableRocks()
        {
            if (Game.instance == null)
                return;

            availableRocks = new List<JesusRockV2>(3);

            LinkedListNode<Unit> node = Game.instance.units.First;
            while (node != null)
            {
                Unit unit = node.Value;

                if (unit is JesusRockV2)
                {
                    availableRocks.Add(unit as JesusRockV2);
                }

                node = node.Next;
            }
        }
    }
}

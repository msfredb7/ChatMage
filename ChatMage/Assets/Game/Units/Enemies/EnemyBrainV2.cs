using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class EnemyBrainV2<T> : EnemyBrainV2 where T : EnemyVehicle
    {
        protected T veh;
        protected virtual void Awake()
        {
            veh = GetComponent<T>();
            if (veh == null)
                throw new System.Exception("Could not find vehicle of type: " + typeof(T).ToString());
        }
    }
    public class EnemyBrainV2 : MonoBehaviour
    {
        protected Stack<Goal> goals = new Stack<Goal>();
        protected Stack<Goal> forcedGoals = new Stack<Goal>();

        /// <summary>
        /// Ajoute un but sur le top de la liste de but forcer (il aura la plus grande priorite)
        /// </summary>
        public void AddForcedGoal(Goal goal)
        {
            if (goal != null)
                forcedGoals.Push(goal);
        }
        public void RemoveAllForcedGoal()
        {
            forcedGoals.Clear();
        }

        /// <summary>
        /// Ajoute un but sur le top de la liste (il aura la plus grande priorite)
        /// </summary>
        public void AddGoal(Goal goal)
        {
            if (goal != null)
                goals.Push(goal);
        }

        public void RemoveAllGoals()
        {
            goals.Clear();
        }

        protected virtual void Update()
        {
            if (forcedGoals.Count != 0)
            {
                //On enleve les sub goals qui on fail ou qui sont fini
                Goal goal = forcedGoals.Peek();
                while (goal.IsComplete() || goal.HasFailed())
                {
                    forcedGoals.Pop();
                    goal.Exit();

                    if (forcedGoals.Count > 0)
                        goal = forcedGoals.Peek();
                    else
                        goal = null;
                }

                if (goal != null)
                    goal.Process();
            }
            else
            {
                if (goals.Count == 0)
                    return;

                //On enleve les sub goals qui on fail ou qui sont fini
                Goal goal = goals.Peek();
                while (goal.IsComplete() || goal.HasFailed())
                {
                    goals.Pop();
                    goal.Exit();

                    if (goals.Count > 0)
                        goal = goals.Peek();
                    else
                        goal = null;
                }

                if (goal != null)
                    goal.Process();
            }
        }
    }
}

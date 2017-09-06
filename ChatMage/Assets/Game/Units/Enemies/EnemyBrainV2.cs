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
            veh.Stop();
        }
    }
    public class EnemyBrainV2 : MonoBehaviour
    {
        public bool canGetForcedGoals = true;

        protected Stack<Goal> goals = new Stack<Goal>();
        protected struct ForcedGoal
        {
            public Goal goal;
            public int priority;
        }
        protected List<ForcedGoal> forcedGoals = new List<ForcedGoal>();

        protected Goal goalInFocus;

        /// <summary>
        /// Ajoute un but sur le top de la liste de but forcer (il aura la plus grande priorite)
        /// </summary>
        public void AddForcedGoal(Goal goal, int priority)
        {
            if (!canGetForcedGoals)
                return;

            if (goal != null)
            {
                AddForcedGoal(new ForcedGoal { goal = goal, priority = priority });
            }
        }

        private void AddForcedGoal(ForcedGoal goal)
        {
            for (int i = 0; i < forcedGoals.Count; i++)
            {
                if (forcedGoals[i].priority > goal.priority)
                {
                    forcedGoals.Insert(i, goal);
                    return;
                }
            }
            forcedGoals.Add(goal);
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
            Goal newGoalInFocus = null;

            if (forcedGoals.Count != 0)
            {
                //On enleve les sub goals qui on fail ou qui sont fini
                Goal goal = forcedGoals[forcedGoals.Count - 1].goal;
                while (goal.IsComplete() || goal.HasFailed())
                {
                    forcedGoals.RemoveAt(forcedGoals.Count - 1);
                    goal.Removed();

                    if (forcedGoals.Count > 0)
                    {
                        goal = forcedGoals[forcedGoals.Count - 1].goal;
                    }
                    else
                    {
                        goal = null;
                        break;
                    }
                }

                newGoalInFocus = goal;
            }
            else if (goals.Count > 0)
            {
                //On enleve les sub goals qui on fail ou qui sont fini
                Goal goal = goals.Peek();
                while (goal.IsComplete() || goal.HasFailed())
                {
                    goals.Pop();
                    goal.Removed();

                    if (goals.Count > 0)
                    {
                        goal = goals.Peek();
                    }
                    else
                    {
                        goal = null;
                        break;
                    }
                }

                newGoalInFocus = goal;
            }
            else
            {
                newGoalInFocus = null;
            }

            if(goalInFocus == null)
            {
                goalInFocus = newGoalInFocus;
            }
            else if(goalInFocus != newGoalInFocus)
            {
                //Changement de goal
                if (!goalInFocus.IsActive())
                {
                    //Assign new goal
                    goalInFocus = newGoalInFocus;
                }
                else if(goalInFocus.CanBeInterrupted)
                {
                    //Interruption 
                    goalInFocus.Interrupted();
                    //Assign new goal
                    goalInFocus = newGoalInFocus;
                }
            }

            if (goalInFocus != null)
                goalInFocus.Process();
        }
    }
}

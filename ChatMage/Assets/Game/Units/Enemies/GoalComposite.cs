using System.Collections.Generic;

namespace AI
{
    public abstract class GoalComposite<T> : GoalComposite where T : EnemyVehicle
    {
        protected EnemyVehicle veh;
        public GoalComposite(EnemyVehicle vehicle)
        {
            veh = vehicle;
        }
    }
    public abstract class GoalComposite : Goal
    {
        public Queue<Goal> subGoals = new Queue<Goal>();

        public Status ProcessSubGoals()
        {
            if (subGoals.Count == 0)
                return Status.completed;

            //On enleve les sub goals qui on fail ou qui sont fini
            Goal subGoal = subGoals.Peek();
            while (subGoal.IsComplete() || subGoal.HasFailed())
            {
                subGoals.Dequeue();
                subGoal.Exit();

                if (subGoals.Count > 0)
                    subGoal = subGoals.Peek();
                else
                    subGoal = null;
            }


            if (subGoal != null)
            {
                Status statusOfSubGoal = subGoal.Process();

                if(statusOfSubGoal == Status.completed && subGoals.Count > 1)
                {
                    return Status.active;
                }

                return statusOfSubGoal;
            }
            else
            {
                return Status.completed;
            }
        }

        public void RemoveAllSubGoals()
        {
            subGoals.Clear();
        }

        /// <summary>
        /// Ajoute un but a la fin de la liste
        /// </summary>
        public void AddSubGoal(Goal goal)
        {
            subGoals.Enqueue(goal);
        }
    }
}

using System.Collections.Generic;

namespace AI
{
    public abstract class GoalComposite<T> : GoalComposite where T : EnemyVehicle
    {
        protected T veh;
        public GoalComposite(T vehicle)
        {
            veh = vehicle;
        }
    }
    public abstract class GoalComposite : Goal
    {
        public Queue<Goal> subGoals = new Queue<Goal>(4);

        public Status ProcessSubGoals()
        {
            if (subGoals.Count == 0)
                return Status.completed;

            bool failure = false;

            //On enleve les sub goals qui on fail ou qui sont fini
            Goal subGoal = subGoals.Peek();
            while (subGoal.IsComplete() || subGoal.HasFailed())
            {
                if (subGoal.HasFailed())
                    failure = true;

                subGoals.Dequeue();
                subGoal.Removed();

                if (subGoals.Count > 0)
                {
                    subGoal = subGoals.Peek();
                }
                else
                {
                    subGoal = null;
                    break;
                }
            }

            if (failure)
                return Status.failed;

            if (subGoal != null)
            {
                Status statusOfSubGoal = subGoal.Process();

                if (statusOfSubGoal == Status.completed && subGoals.Count > 1)
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

        public override void ForceFailure()
        {
            foreach (Goal goal in subGoals)
            {
                if (goal.IsActive())
                {
                    goal.ForceFailure();
                    goal.Removed();
                }
            }

            base.ForceFailure();
        }

        public override void Interrupted()
        {
            if (subGoals.Count > 0)
            {
                Goal goal = subGoals.Peek();
                if (goal.IsActive())
                    goal.Interrupted();
            }

            base.Interrupted();
        }

        public override bool CanBeInterrupted
        {
            get
            {
                if(subGoals.Count > 0)
                {
                    //s'il y a des sous-but, checkez le but enfant ?
                    if (base.CanBeInterrupted)
                        return subGoals.Peek().CanBeInterrupted;
                    else
                        return false;
                }
                else
                {
                    //Si il n'y a aucun sous-but. on PEUT etre interrompu
                    return true;
                }
            }

            set
            {
                base.CanBeInterrupted = value;
            }
        }
    }
}

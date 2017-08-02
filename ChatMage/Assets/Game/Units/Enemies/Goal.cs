using System;

namespace AI
{
    public abstract class Goal<T> : Goal where T : EnemyVehicle
    {
        protected T veh;
        public Goal(T vehicle)
        {
            veh = vehicle;
        }
    }

    public abstract class Goal
    {
        public delegate void GoalEvent(Goal goal);
        public enum Status { active = 0, inactive = 1, completed = 2, failed = 3 }

        public GoalEvent onExit;

        protected Status status = Status.inactive;

        public virtual void Activate()
        {
            status = Status.active;
        }

        public abstract Status Process();

        public virtual void ForceCompletion()
        {
            status = Status.completed;
        }

        public virtual void LoseFocus()
        {

        }
        public virtual void GainFocus()
        {

        }

        public virtual void ForceFailure()
        {
            status = Status.failed;
        }

        public virtual void Exit()
        {
            if (onExit != null)
                onExit(this);
        }

        protected void ReactivateIfFailed()
        {
            if (status == Status.failed)
                Activate();
        }

        protected void ActivateIfInactive()
        {
            if (status == Status.inactive)
                Activate();
        }

        public bool IsComplete() { return status == Status.completed; }
        public bool IsActive() { return status == Status.active; }
        public bool IsInactive() { return status == Status.active; }
        public bool HasFailed() { return status == Status.failed; }

    }
}

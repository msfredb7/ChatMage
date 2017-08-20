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

        public GoalEvent onRemoved;
        public GoalEvent onActivated;
        private bool canBeInterrupted = true;

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

        public virtual void ForceFailure()
        {
            status = Status.failed;
        }

        public virtual void Interrupted()
        {

        }

        public virtual void Removed()
        {
            if (onRemoved != null)
                onRemoved(this);
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

        protected void ReactivateIfCompleted()
        {
            if (status == Status.completed)
                Activate();
        }

        public virtual bool CanBeInterrupted
        {
            get { return canBeInterrupted; }
            set { canBeInterrupted = value; }
        }

        public bool IsComplete() { return status == Status.completed; }
        public bool IsActive() { return status == Status.active; }
        public bool IsInactive() { return status == Status.active; }
        public bool HasFailed() { return status == Status.failed; }

    }
}

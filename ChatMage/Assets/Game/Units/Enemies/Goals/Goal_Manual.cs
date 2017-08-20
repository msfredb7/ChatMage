using System;

namespace AI
{
    public class Goal_Manual : Goal
    {
        Action onActivate;
        public Goal_Manual(Action onActivate = null)
        {
            this.onActivate = onActivate;
        }

        public override void Activate()
        {
            base.Activate();

            if (onActivate != null)
                onActivate();
        }

        public override Status Process()
        {
            ActivateIfInactive();
            return status;
        }
    }
}

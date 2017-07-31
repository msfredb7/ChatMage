using UnityEngine;

public abstract class BaseBuff
{
    public float remainingDuration;
    public bool worldTime;

    public BaseBuff(float duration, bool worldTime = true)
    {
        remainingDuration = duration;
        this.worldTime = worldTime;
    }

    public virtual float DecreaseDuration(float worldDeltaTime, float localDeltaTime)
    {
        return remainingDuration -= worldTime ? worldDeltaTime : localDeltaTime;
    }
    public abstract void ApplyEffect(Unit unit);
    public abstract void RemoveEffect(Unit unit);
}

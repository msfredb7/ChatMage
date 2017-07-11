using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleBuff : BaseBuff
{
    private float multiplier;
     
    public TimeScaleBuff(float multiplier, float duration, bool worldTime) : base(duration, worldTime)
    {
        this.multiplier = multiplier;
    }
    public override void ApplyEffect(Unit unit)
    {
        unit.TimeScale *= multiplier;
    }

    public override void RemoveEffect(Unit unit)
    {
        unit.TimeScale /= multiplier;
    }
}

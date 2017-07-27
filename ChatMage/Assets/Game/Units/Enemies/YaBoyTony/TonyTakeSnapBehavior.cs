using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonyTakeSnapBehavior : BaseTweenBehavior<TonyVehicle>
{
    private float postZoneRemains;
    private Action onComplete;

    public TonyTakeSnapBehavior(TonyVehicle veh, Action onComplete) : base(veh)
    {
        this.onComplete = onComplete;
        tween = veh.animator.TakeSnap(OnZoneComplete);
    }
    
    public override void Update(Unit target, float deltaTime)
    {
        if(postZoneRemains > 0)
        {
            postZoneRemains -= deltaTime;
            if(postZoneRemains <= 0)
            {
                if (onComplete != null)
                    onComplete();
                onComplete = null;
            }
        }
    }

    void OnZoneComplete()
    {
        postZoneRemains = 0.8f;
    }
}

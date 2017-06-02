using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GourdinierAttackAnim
{
    const float chargeDuration = 0.75f;

    GourdinierVehicle vehicle;
    Color baseBodyColor;
    Tween tween;

    void SetTimeScale()
    {
        tween.timeScale = vehicle.TimeScale;
    }

    private void Vehicle_onTimeScaleChange(Unit unit)
    {
        SetTimeScale();
    }

    public void Cancel()
    {

    }

    public GourdinierAttackAnim(GourdinierVehicle vehicle)
    {
        this.vehicle = vehicle;
        baseBodyColor = vehicle.bodySprite.color;
        vehicle.onTimeScaleChange += Vehicle_onTimeScaleChange;
    }

    public void Charge(TweenCallback onComplete)
    {
        //Launch anim
        tween = vehicle.bodySprite.DOColor(Color.white, chargeDuration).SetUpdate(false).OnComplete(onComplete);

        SetTimeScale();
    }
}

using CCC.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : Vehicle, IAttackable, IVisible
{
    [Header("Motor"), ReadOnly]
    public float driftStrain;
    [ReadOnly]
    public float motorStrain;

    [NonSerialized]
    public PlayerController controller;

    public ISpeedOverrider speedOverrider = null;
    public List<ISpeedBuff> speedBuffs = new List<ISpeedBuff>();

    public float RealMoveSpeed()
    {
        return ActualMoveSpeed();
    }
    protected override float ActualMoveSpeed()
    {
        if (speedOverrider != null)
            return speedOverrider.GetSpeed();

        float value = base.ActualMoveSpeed();
        for (int i = 0; i < speedBuffs.Count; i++)
        {
            value += speedBuffs[i].GetAdditionalSpeed();
        }

        return value;
    }

    public void Init(PlayerController controller)
    {
        this.controller = controller;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        driftStrain = Mathf.Abs(Mathf.DeltaAngle(targetDirection, Rotation));
        motorStrain = (Speed - targetSpeed).magnitude / ActualMoveSpeed();
    }

    public void Kill()
    {
        if (IsDead)
            return;

        Die();
    }

    protected override void Die()
    {
        base.Die();

        //Death animation
        Destroy();
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, otherUnit, source);

        return controller.playerStats.Attacked(on, amount, otherUnit, source);
    }

    public float GetSmashJuiceReward()
    {
        return 0;
    }

    public bool IsVisible()
    {
        return controller.playerStats.isVisible;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBuff : BaseBuff, IAttackable
{
    private const float BUMP_STR = 3.5f;
    private const int BUBBLE_SMASH_JUICE = 1;

    private bool removeShield = false;
    private GameObject vfxPrefab;
    private GameObject vfx;
    float vfxScaleToUnitWidth;

    public BubbleBuff(float duration, GameObject visualPrefab, float vfxScaleToUnitWidth) : base(duration, true)
    {
        this.vfxScaleToUnitWidth = vfxScaleToUnitWidth;
        vfxPrefab = visualPrefab;
    }
    public override void ApplyEffect(Unit unit)
    {
        Transform tr = unit.transform;

        if (unit is EnemyVehicle && (unit as EnemyVehicle).bodyCenter != null)
            tr = (unit as EnemyVehicle).bodyCenter;

        vfx = UnityEngine.Object.Instantiate(vfxPrefab, tr);

        float unitWidth = 1;
        if(unit is EnemyVehicle)
        {
            unitWidth = (unit as EnemyVehicle).unitWidth;
        }
        else if (unit is PlayerVehicle)
        {
            unitWidth = 1.25f;
        }

        vfx.transform.localScale = Vector3.one * vfxScaleToUnitWidth * unitWidth / (tr.lossyScale.x);
    }

    public override void RemoveEffect(Unit unit)
    {
    }

    public override float DecreaseDuration(float worldDeltaTime, float localDeltaTime)
    {
        if (removeShield)
        {
            return base.DecreaseDuration(worldDeltaTime, localDeltaTime);
        }
        else
        {
            return 1;
        }
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if(source != null)
        {
            Unit unit = source.parentUnit;
            if (unit != null && unit is PlayerVehicle)
                (unit as Vehicle).Bump(
                    (source.transform.position - on.transform.position).normalized * BUMP_STR,
                    0,
                    BumpMode.VelocityChange);
        }

        if(!removeShield)
            UnityEngine.Object.Destroy(vfx);

        removeShield = true;

        //Block le dégat
        return 0;
    }

    public int SmashJuice()
    {
        return BUBBLE_SMASH_JUICE;
    }
}

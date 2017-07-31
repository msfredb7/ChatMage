using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardVehicle : EnemyVehicle
{
    [Header("Bard")]
    public BardAnimator animator;
    [Forward]
    public Targets boostTargets;

    [Header("Sing")]
    public float singRadius;
    public float duration;
    public float timeScaleMultiplier;
    public float capOnOtherBards = 2;
    public float defaultCap = 5;

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0 && !IsDead)
            return 1;

        Die();
        return 0;
    }

    protected override void Die()
    {
        base.Die();

        //Death animation
        Destroy();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, singRadius);
    }
}

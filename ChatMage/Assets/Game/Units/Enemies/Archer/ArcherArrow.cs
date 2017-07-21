using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : MovingUnit
{
    [Header("Linking")]
    public SimpleColliderListener listener;

    [Header("Settings")]
    public float shootSpeed = 8;
    public float flyDistance = 18.5f;

    [NonSerialized]
    private float distanceTravelled = 0;
    [NonSerialized]
    private Unit origin;
    [NonSerialized]
    private Targets targets;

    public void Init(Unit origin, Vector2 dir, Targets targetsToCopy)
    {
        listener.onTriggerEnter += Listener_onTriggerEnter;
        Speed = dir.normalized * shootSpeed * timeScale;
        transform.rotation = Quaternion.Euler(Vector3.forward * Vehicle.VectorToAngle(dir));

        targets.CopyTargetsFrom(targetsToCopy);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isDead)
        {
            distanceTravelled += shootSpeed * FixedDeltaTime();

            if (distanceTravelled > flyDistance)
                Die();
        }
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (isDead)
            return;

        Unit unit = other.parentUnit;
        if (unit != origin && targets.IsValidTarget(unit))
        {
            IAttackable attackable = unit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, this, listener.info);
                Die();
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}

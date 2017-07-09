using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : Projectiles
{
    [Header("Linking")]
    public SimpleColliderListener listener;

    [Header("Settings")]
    public float shootSpeed = 8;
    public float flyDistance = 18.5f;

    private float distanceTravelled = 0;

    public void Init(Vector2 dir)
    {
        listener.onTriggerEnter += Listener_onTriggerEnter;
        Speed = dir.normalized * shootSpeed * timeScale;
        transform.rotation = Quaternion.Euler(Vector3.forward * Vehicle.VectorToAngle(dir));
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

        if (other.parentUnit != origin && IsValidTarget(other.parentUnit.allegiance))
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
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

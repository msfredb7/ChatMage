using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirRocheProjectile : MovingUnit
{
    [Header("Linking")]
    public SimpleColliderListener listener;

    [Header("Settings")]
    public float shootSpeed = 8;

    private float distanceToReach;
    private bool hitSomething = false;
    private float distanceTravelled = 0;
    private bool aboutToDie = false;

    public void Init(Vector2 dir, float distance)
    {
        distanceToReach = distance;
        listener.onTriggerEnter += Listener_onTriggerEnter;
        Speed = dir.normalized * shootSpeed * timeScale;
        Rotation = Vehicle.VectorToAngle(dir);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        distanceTravelled += Speed.magnitude * FixedDeltaTime();

        if (!aboutToDie && distanceTravelled > distanceToReach)
        {
            hitSomething = false;
            Die();
        }
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (aboutToDie)
            return;

        if (other.parentUnit.allegiance == Allegiance.Ally)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, this, listener.info);
                hitSomething = true;
                Die();
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        aboutToDie = true;

        if (hitSomething)
        {
            //hit something animation (ex: sparks)
            Destroy(gameObject);
        }
        else
        {
            //hit nothing animation (ex: dirt splash)
            Destroy(gameObject);
        }
    }
}

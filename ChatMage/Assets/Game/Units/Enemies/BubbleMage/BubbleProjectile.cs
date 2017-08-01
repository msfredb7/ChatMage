using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : MovingUnit
{
    public float speed;
    public float maxTimeAlive = 2;
    public SimpleColliderListener listener;
    public float turnSpeed;
    public GameObject bubbleVFX;
    public float vfxScaleToUnitWidth;

    private Unit doNoHit;
    private Transform seek;

    void Start()
    {
        listener.onTriggerEnter += Listener_onTriggerEnter;
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (IsDead)
            return;

        Unit unit = other.parentUnit;
        if (unit != null && unit != doNoHit && !unit.HasBuffOfType(typeof(BubbleBuff)))
        {
            unit.AddBuff(new BubbleBuff(1, bubbleVFX, vfxScaleToUnitWidth));
        }
        Die();
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }

    public void Init(float dir, Unit doNoHit, Transform seek = null)
    {
        Rotation = dir;
        UpdateSpeed();

        this.doNoHit = doNoHit;
        this.seek = seek;
    }

    protected override void Update()
    {
        base.Update();

        float deltaTime = DeltaTime();

        if(seek != null)
        {
            Vector2 v = (Vector2)seek.position - Position;
            Rotation = Rotation.MovedTowards(v.ToAngle(), deltaTime * turnSpeed);

            UpdateSpeed();
        }

        maxTimeAlive -= deltaTime;

        if (maxTimeAlive < 0 && !isDead)
            Die();
    }

    private void UpdateSpeed()
    {
        Speed = Rotation.ToVector() * speed;
    }
}

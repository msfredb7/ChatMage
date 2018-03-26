using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BubbleProjectile : MovingUnit
{
    public float speed;
    public float maxTimeAlive = 2;
    public SimpleColliderListener listener;
    public float turnSpeed;
    public new ParticleSystem particleSystem;

    [Header("My VFX")]
    public GameObject core;

    [Header("Bubble Prefab")]
    public GameObject bubbleVFX;
    public float vfxScaleToUnitWidth;

    private Unit doNoHit;
    private Unit seek;

    public AudioPlayable bubble;
    public AudioSource bubbleTravelSource;

    void Start()
    {
        listener.onTriggerEnter += Listener_onTriggerEnter;
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (IsDead)
            return;

        Unit unit = other.parentUnit;

        if (unit != null && unit != doNoHit && unit is IAttackable)
        {
            if (!unit.HasBuffOfType(typeof(BubbleBuff)))
            {
                DefaultAudioSources.PlaySFX(bubble);
                unit.AddBuff(new BubbleBuff(0.2f, bubbleVFX, vfxScaleToUnitWidth));
            }
            Die();
        }
    }

    protected override void Die()
    {
        bubbleTravelSource.volume = 0;
        if (!isDead)
            Game.Instance.events.AddDelayedAction(Destroy, 1.5f);
        base.Die();

        Speed = Vector2.zero;
        core.SetActive(false);
        enabled = false;
    }

    public void Init(float dir, Unit doNoHit, Unit seek = null)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * dir);
        Rotation = dir;
        UpdateSpeed();

        this.doNoHit = doNoHit;
        this.seek = seek;
    }

    protected override void Update()
    {
        base.Update();

        float deltaTime = DeltaTime();

        if(HasPresence(seek))
        {
            Vector2 v = seek.Position - Position;
            Rotation = Rotation.MovedTowards(v.ToAngle(), deltaTime * turnSpeed);

            UpdateSpeed();
        }

        maxTimeAlive -= deltaTime;

        if (maxTimeAlive < 0 && !isDead)
        {
            Die();
        }
            
    }

    private void UpdateSpeed()
    {
        Speed = Rotation.ToVector() * speed * TimeScale;
    }

    public override float TimeScale
    {
        get
        {
            return base.TimeScale;
        }

        set
        {
            float before = timeScale;
            base.TimeScale = value;

            float mult = timeScale / before;
            Speed *= mult;

            var main = particleSystem.main;
            main.simulationSpeed = timeScale;
            var rate = particleSystem.emission;
            rate.rateOverDistanceMultiplier /= mult;
            var velocity = particleSystem.inheritVelocity;
            velocity.curveMultiplier /= mult;
        }
    }
}

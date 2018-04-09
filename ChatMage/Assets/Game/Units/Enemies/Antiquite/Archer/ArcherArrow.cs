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
    public bool bounce = false;
    [ShowIf("bounce")]
    public int bounceCount = 0;
    [ShowIf("bounce")]
    public AudioPlayable bounceSFX;
    public int[] bounceOn;

    [Header("VFX")]
    public Transform arrowTip;

    [NonSerialized]
    private float distanceTravelled = 0;
    [NonSerialized]
    private Unit origin;
    [NonSerialized]
    private Targets targets;
    private Vector2 wasVelocity;
    private bool weJustHitAPlayer;
    private int stuckInCarCounter = 0;
    private Vector2 flyVector;

    public void Init(Unit origin, Vector2 dir, Targets targetsToCopy)
    {
        listener.onTriggerEnter += Listener_onTriggerEnter;
        listener.onCollisionEnter += Listener_onCollisionEnter;
        flyVector = dir.normalized * shootSpeed;
        ApplyFlyVector();
        transform.rotation = Quaternion.Euler(Vector3.forward * Vehicle.VectorToAngle(dir));

        targets = new Targets(targetsToCopy);
    }

    private void ApplyFlyVector()
    {
        Speed = flyVector * timeScale;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        wasVelocity = Speed;

        ApplyFlyVector();

        if (!isDead && !bounce)
        {
            distanceTravelled += shootSpeed * FixedDeltaTime();

            if (distanceTravelled > flyDistance)
                Die();
        }
        weJustHitAPlayer = false;

        if(stuckInCarCounter >= 8)
        {
            Die();
        }
    }

    private void Listener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        Listener_onTriggerEnter(other, listener);
        weJustHitAPlayer = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.rigidbody.gameObject == Game.Instance.Player.gameObject)
        {
            stuckInCarCounter++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (bounceOn.Contains(collision.gameObject.layer) && bounce)
        {
            if (collision.contacts.Length > 0)
                ImpactEffect(collision.contacts[0].point);

            if (bounceCount > 0 && collision.contacts.Length > 0)
            {
                Vector2 averageNormals = Vector2.zero;
                for (int i = 0; i < collision.contacts.Length; i++)
                {
                    averageNormals += collision.contacts[i].normal;
                    Debug.DrawLine(collision.contacts[i].point, collision.contacts[i].point + collision.contacts[i].normal, Color.red);
                }
                averageNormals.Normalize();

                var newDir = Vector2.Reflect(wasVelocity, averageNormals);
                flyVector = newDir.normalized * shootSpeed;
                Rotation = newDir.ToAngle();
                ApplyFlyVector();
                bounceCount--;

                //if (!weJustHitAPlayer)
                //{
                //    // Impact SFX + VFX
                //}
            }
            else
            {
                Die();
            }
        }
    }

    private void ImpactEffect(Vector2 position)
    {
        Game.Instance.commonVfx.HitWhite(position);
        DefaultAudioSources.PlaySFX(bounceSFX);
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
                Game.Instance.commonVfx.HitWhite(arrowTip.position);

                attackable.Attacked(other, 1, this, listener.info);

                if (!bounce || bounceCount <= 0)
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : Unit
{
    [Header("Linking")]
    public SimpleColliderListener core;
    public SimpleColliderListener orbit;
    public SunAnimator animator;

    [Header("Growth")]
    public float startSize = 1;
    public float growthPerKill = 0.25f;

    [Header("Orbit")]
    public float orbitBumpStrength = 4;
    public float orbitBumpDuration = 1;


    private float duration;
    private float size;

    void Start()
    {
        core.onTriggerEnter += Core_onTriggerEnter;
        orbit.onTriggerEnter += Orbit_onTriggerEnter;

        //On commence a scale (0,0)
        transform.localScale = Vector3.zero;

        //On grossi !
        size = startSize;
        ApplySize();
    }

    private void Orbit_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        //On bump la unit vers l'intérieur
        if (other.parentUnit is Vehicle)
        {
            (other.parentUnit as Vehicle).Bump(
                (Position - other.parentUnit.Position).normalized * orbitBumpStrength,
                orbitBumpDuration,
                BumpMode.VelocityChange);
        }
    }

    private void Core_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {

        //On tue la unit
        IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
        if (attackable != null)
        {
            bool unitKilled = attackable.Attacked(other, 100, this, listener.info) == 0;
            
            if (unitKilled)
            {
                if (!isDead)
                    Grow();
            }
            else
            {
                //Debug.LogWarning("Une unit à été envoyé dans le soleil et elle est pas morte... pls");
            }
        }
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    void Update()
    {
        duration -= DeltaTime();
        if (duration <= 0)
            Die();
    }

    private void ApplySize(DG.Tweening.TweenCallback onComplete = null)
    {
        animator.SetSize(size, onComplete);
    }

    private void Grow()
    {
        size += growthPerKill;
        ApplySize();
    }

    protected override void Die()
    {
        if (isDead)
            return;

        base.Die();

        size = 0;
        ApplySize(delegate()
        {
            Destroy();
        });
    }

    public void ForceDeath()
    {
        Die();
    }

    public void SetVFX(ScreenAdd vfx)
    {
        animator.SetVFX(vfx);
    }
}

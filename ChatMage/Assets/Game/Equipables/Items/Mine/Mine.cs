using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Unit
{
    [Header("Trigger")]
    public SimpleColliderListener colliderListener;
    public Collider2D col;

    [Header("Apparence")]
    public SpriteRenderer spriteRenderer;

    [Header("Explosion")]
    public LayerMask explosionLayerMask;
    public float stdExplosionRadius = 1;
    public float boostedExplosionRadius = 2;
    public ExplosionAnimator explosion;

    [Header("Timing")]
    public float explosionDelay = 5;
    public float flickerDuration = 2.5f;
    public float flickerSpeed = 0.4f;

    float timer;
    bool flickering = false;
    //bool exploding = false;

    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;

        timer = explosionDelay;
    }

    void Update()
    {
        if (timer < flickerDuration && !flickering)
        {
            Flicker();
        }

        if (timer < 0 && !isDead)
            Die();

        timer -= DeltaTime();
    }

    void Flicker()
    {
        if (flickering)
            return;
        flickering = true;

        if (flickerSpeed <= 0)
            Debug.LogWarning("Flicker speed is set to < 0 !");
        spriteRenderer.DOFade(0, 1 / flickerSpeed).SetLoops(-1, LoopType.Yoyo);
    }


    protected override void Die()
    {
        base.Die();

        //desactive la trigger
        col.enabled = false;
        spriteRenderer.enabled = false;
        spriteRenderer.DOKill();

        float radius = stdExplosionRadius;
        if (Game.instance.Player != null && Game.instance.Player.playerStats.boostedAOE)
            radius = boostedExplosionRadius;

        explosion.Explode(radius, delegate () { Destroy(gameObject); }, this);

        //Explosion !
        Collider2D[] cols = Physics2D.OverlapCircleAll(Position, boostedExplosionRadius, explosionLayerMask);
        for (int i = 0; i < cols.Length; i++)
        {
            ColliderInfo otherInfo = cols[i].GetComponent<ColliderInfo>();
            if (otherInfo != null)
                UnitHit(otherInfo);
        }
    }

    private void UnitHit(ColliderInfo other)
    {
        if (other.parentUnit.allegiance != Allegiance.Ally)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, this);
            }
        }
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (!isDead)
            Die();
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : Vehicle
{
    // Collider
    public SimpleColliderListener colliderListener;

    // Apparence
    public SpriteRenderer image;

    // Position
    Vector2 myPos;

    // Explosion
    public LayerMask explosionLayerMask;
    float explosionForce = 1;
    public ExplosionAnimator explosion;

    // Timing
    public float explosionMaxDelay = 5;
    public float preparingExplosionDelay = 2.5f;
    public float fadeSpeed = 1;
    float cooldown;
    public float animationdelay = 0.5f;

    // Condition
    bool previouslyInvisible = false;
    bool animationStarted = false;

    List<Sequence> currentAnimations = new List<Sequence>();

    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;

        cooldown = explosionMaxDelay;
        animationStarted = false;

        explosion.ResetValues();

        image.color = new Color(image.color.r, image.color.g, image.color.b, 255);
    }

    void Update()
    {
        if (cooldown < 0)
            Explode();
        if(cooldown < preparingExplosionDelay && !animationStarted)
        {
            DoAnimation();
            animationStarted = true;
        }
        cooldown -= Time.deltaTime;
        TeleportPosition(myPos);
    }

    void DoAnimation()
    {
        if (previouslyInvisible)
        {
            previouslyInvisible = false;
            image.DOFade(1, fadeSpeed).OnComplete(delegate () {
                Sequence sq = DOTween.Sequence().SetUpdate(true);
                sq.InsertCallback(animationdelay, DoAnimation);
                currentAnimations.Add(sq);
                sq.OnComplete(delegate () { currentAnimations.Remove(sq); });
            });
        } else
        {
            previouslyInvisible = true;
            image.DOFade(0, fadeSpeed).OnComplete(delegate () {
                Sequence sq = DOTween.Sequence().SetUpdate(true);
                sq.InsertCallback(animationdelay, DoAnimation);
                currentAnimations.Add(sq);
                sq.OnComplete(delegate () { currentAnimations.Remove(sq); });
            });
        }
    }

    public void SetMinePosition(Vector2 position)
    {
        TeleportPosition(position);
        myPos = position;
    }

    public void Explode()
    {
        explosion.Explode(delegate () { Destroy(gameObject); }, this);

        // Stop les Animations en cours
        for (int i = 0; i < currentAnimations.Count; i++)
        {
            currentAnimations[i].Complete();
        }

        //Explosion !
        Collider2D[] cols = Physics2D.OverlapCircleAll(Position, explosionForce, explosionLayerMask);
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

    public void SetExplosionForce(float force)
    {
        explosionForce = force;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Explode();
    }
}

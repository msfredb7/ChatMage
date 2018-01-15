using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rasengan : Unit
{
    [Header("Rasengan")]
    public SpriteRenderer spr;
    public SimpleColliderListener trigger;
    [Forward]
    public Targets targets;
    public float boostedAOESizeMultiplier = 1.4f;

    [Header("Growth")]
    public float finalSize = 1;
    public float growthDuration;
    public Ease growthEase;

    [Header("In between")]
    public float duration;

    [Header("Decay")]
    public float decayFinalSize = 0.8f;
    public float decayDuration;
    public Ease decayEase;

    private new Sequence animation = null;
    private float sizeMul = 1;

    void Start()
    {
        sizeMul = Game.Instance.Player.playerStats.boostedAOE ? boostedAOESizeMultiplier : 1;
        trigger.onTriggerEnter += Trigger_onTriggerEnter;

        //---------ANIMATION---------//

        animation = DOTween.Sequence();

        //Grow
        GrowAnim(animation);

        //Stay
        animation.AppendInterval(duration);

        //Shrink back
        DecayAnim(animation);

        animation.OnComplete(delegate ()
        {
            Die();
        });
    }

    public override float TimeScale
    {
        get
        {
            return base.TimeScale;
        }

        set
        {
            base.TimeScale = value;

            if (animation != null)
                animation.timeScale = timeScale;
        }
    }

    private void Trigger_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Unit unit = other.parentUnit;
        if(unit != null && unit is IAttackable && targets.IsValidTarget(unit))
        {
            IAttackable attackable = unit as IAttackable;

            bool wasDead = unit.IsDead;
            attackable.Attacked(other, 1, this, listener.info);

            if (unit.IsDead && !wasDead && Game.Instance.Player != null)
                Game.Instance.Player.playerStats.RegisterKilledUnit(unit);
        }
    }

    void GrowAnim(Sequence sq)
    {
        transform.localScale = Vector2.zero;
        sq.Append(spr.DOFade(1, growthDuration));
        sq.Join(transform.DOScale(finalSize * sizeMul, growthDuration).SetEase(growthEase));
    }

    void DecayAnim(Sequence sq)
    {
        sq.Append(spr.DOFade(0, decayDuration));
        sq.Join(transform.DOScale(decayFinalSize * sizeMul, decayDuration).SetEase(decayEase));
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(animation != null)
        {
            animation.Kill();
            animation = null;
        }
    }

}

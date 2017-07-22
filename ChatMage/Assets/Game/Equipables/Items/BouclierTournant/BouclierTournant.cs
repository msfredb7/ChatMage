using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BouclierTournant : Unit, IAttackable
{
    [Header("Linking"), Forward]
    public Targets targets;
    public SimpleColliderListener shieldCollider;
    public GameObject hitVfx;

    [Header("Turn Animation")]
    public float animationDuration = 3;
    public Ease turnEase = Ease.OutBack;
    public float overshoot;

    private Tween rotateTween = null;

    void Start()
    {
        shieldCollider.onCollisionEnter += ShieldCollider_onCollisionEnter;
    }

    //Si le shield frappe un ennemi, il l'attaque, puis tourne
    private void ShieldCollider_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        Unit unit = other.parentUnit;
        if (!targets.IsValidTarget(unit))
            return;

        IAttackable attackable = unit.GetComponent<IAttackable>();
        if (attackable != null)
        {
            bool wasDead = unit.IsDead;
            attackable.Attacked(other, 1, null);

            if (unit.IsDead && !wasDead)
                Game.instance.Player.playerStats.RegisterKilledUnit(unit);

            OnShieldHit();
        }
    }

    //Si le shield se fait frapper, il resiste et tourne
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        OnShieldHit();
        return 1;
    }

    void OnShieldHit()
    {
        if (!IsTurning())
            TurnShield();

        Instantiate(hitVfx, shieldCollider.transform.position, Quaternion.identity);
    }

    private bool IsTurning()
    {
        return rotateTween != null && rotateTween.IsActive() && !rotateTween.IsComplete();
    }

    private void TurnShield()
    {
        float mult = Game.instance.Player.playerStats.cooldownMultiplier;
        rotateTween = transform.DOLocalRotate(new Vector3(0, 0, -90), animationDuration * mult, RotateMode.LocalAxisAdd)
            .SetEase(turnEase, overshoot);
    }
}

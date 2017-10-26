using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DarkMoleSword : Unit, IAttackable
{
    [Header("Linking"), Forward]
    public Targets targets;
    public SimpleColliderListener swordCollider;

    [Header("Size")]
    public float boostedSizeMultiplier = 1.4f;

    void Start()
    {
        //On grossie le shield si on a 'boostedAOE'
        if (Game.instance.Player.playerStats.boostedAOE)
            swordCollider.transform.localScale *= boostedSizeMultiplier;

        swordCollider.onCollisionEnter += ShieldCollider_onCollisionEnter;
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

            OnSwordHit();
        }
    }

    //Si le shield se fait frapper, il resiste et tourne
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        OnSwordHit();
        return 1;
    }

    void OnSwordHit()
    {
        Game.instance.commonVfx.SmallHit(swordCollider.transform.position, Color.red);
    }

    public float GetSmashJuiceReward()
    {
        return 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingAxe : Unit
{
    [Header("Linking"), Forward]
    public Targets targets;
    public SimpleColliderListener shieldCollider;

    void Start()
    {
        shieldCollider.onTriggerEnter += ShieldCollider_onTiggerEnter;
    }

    //Si la hache frappe un ennemi, il l'attaque
    private void ShieldCollider_onTiggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Unit unit = other.parentUnit;
        if (!targets.IsValidTarget(unit))
            return;

        IAttackable attackable = unit.GetComponent<IAttackable>();
        if (attackable != null)
        {
            bool wasDead = unit.IsDead;
            attackable.Attacked(other, 1, this);

            if (unit.IsDead && !wasDead)
                Game.instance.Player.playerStats.RegisterKilledUnit(unit);
        }
    }
}

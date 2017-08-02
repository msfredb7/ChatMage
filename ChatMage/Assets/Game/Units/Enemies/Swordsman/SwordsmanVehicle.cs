using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanVehicle : EnemyVehicle
{
    [Header("Spearman")]
    public SwordsmanAnimator animator;
    public SpriteRenderer bodySprite;

    [Header("Attack")]
    public SimpleColliderListener attackListener;
    public float attackBumpForce = 5;

    [Header("Armor")]
    public float invulnerableDuration = 0.35f;
    public float armorBumpForce = 5;

    public event SimpleEvent onArmorLoss;
    
    [System.NonSerialized]
    public bool spearAttackConsumed = false;

    private bool armorUp = true;
    private float invulnerableRemains = -1;

    protected override void Awake()
    {
        base.Awake();
        attackListener.onTriggerEnter += AttackListener_onTriggerEnter;
    }

    protected override void Update()
    {
        if (invulnerableRemains > 0)
            invulnerableRemains -= DeltaTime();
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0 && !isDead)
            return armorUp ? 2 : 1;

        if (invulnerableRemains > 0 && !isDead)
            return armorUp ? 2 : 1;
        
        if (armorUp)
        {
            if (unit is Vehicle)
                (unit as Vehicle).Bump((unit.Position - Position).normalized * armorBumpForce, 0.1f, BumpMode.VelocityChange);
            LoseArmor();
            return 1;
        }
        else
        {

            Die();
            return 0;
        }
    }

    private void LoseArmor()
    {
        invulnerableRemains = invulnerableDuration;
        armorUp = false;
        gameObject.layer = Layers.ENEMIES;
        if (onArmorLoss != null)
            onArmorLoss();
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }

    private void AttackListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (spearAttackConsumed)
            return;

        Unit unit = other.parentUnit;

        if (targets.IsValidTarget(unit))
        {
            IAttackable attackable = unit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                spearAttackConsumed = true;
                attackable.Attacked(other, 1, this, listener.info);

                //Bump
                if (unit is Vehicle)
                {
                    Vector2 v = unit.Position - Position;
                    (unit as Vehicle).Bump(v.normalized * attackBumpForce, -1, BumpMode.VelocityAdd);
                }
            }
        }
    }
}

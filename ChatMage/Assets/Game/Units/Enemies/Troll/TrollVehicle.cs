using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollVehicle : EnemyVehicle
{
    [Header("Troll")]
    public int hp = 10;
    public float finalTimescale = 2;

    [Header("On Hit")]
    public float invulnurableDuration = 1.5f;
    public float bumpForce = 5;

    [Header("Linking")]
    public new Collider2D collider;
    public TrollAnimator animator;
    public Transform rockTransporter;

    [Header("Throw")]
    public float throwSpeed;

    [Header("Visuals")]
    public SpriteRenderer[] spriteRenderers;

    private bool damagable = true;
    private float timescaleIncrease;

    void Start()
    {
        timescaleIncrease = 1 + ((finalTimescale - 1) / (hp - 1));
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0)
            return hp;

        if (!damagable)
            return hp;

        //Decrease hp
        hp -= amount;

        TimeScale *= timescaleIncrease;

        //Flashs animation
        damagable = false;
        FlashAnimation.FlashColor(this, spriteRenderers, invulnurableDuration, Color.red, () => damagable = true);

        //Bump unit
        if (unit is Vehicle)
            (unit as Vehicle).Bump((unit.Position - Position).normalized * bumpForce, -1, BumpMode.VelocityAdd);


        if (hp <= 0)
        {
            //Dead !
            if (!isDead)
                Die();
        }

        return hp;
    }

    protected override void Die()
    {
        base.Die();

        collider.enabled = false;

        //Death anim
        Destroy();
    }
}

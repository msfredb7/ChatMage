using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusV2Vehicle : EnemyVehicle
{
    [Header("Jesus")]
    public string displayName = "Jesus";
    public int hp = 10;
    public float finalTimescale = 2;

    [Header("On Hit")]
    public float invulnurableDuration = 1.5f;
    public float bumpForce = 5;

    [Header("Linking")]
    public new Collider2D collider;
    public JesusV2Animator animator;
    public Transform rockTransporter;

    [Header("Throw")]
    public float throwSpeed;

    [Header("Visuals")]
    public SpriteRenderer[] spriteRenderers;

    private int maxHp;
    private BossHealthBarDisplay hpDisplay;

    private bool damagable = true;

    void Start()
    {
        maxHp = hp;
        hpDisplay = Game.instance.ui.bossHealthBar;
        hpDisplay.DisplayBoss(displayName);
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (!damagable)
            return hp;

        //Decrease hp
        hp -= amount;

        TimeScale *= 1 + ((finalTimescale - 1) / maxHp);

        //Flashs animation
        damagable = false;
        FlashAnimation.FlashColor(this, spriteRenderers, invulnurableDuration, Color.red, () => damagable = true);

        //Bump unit
        if (unit is Vehicle)
            (unit as Vehicle).Bump((unit.Position - Position).normalized * bumpForce, -1, BumpMode.VelocityAdd);

        if (hp > 0)
        {
            //Set boss slider
            hpDisplay.SetSliderValue01(hp / (float)maxHp);
        }
        else
        {
            //Dead !
            if (!isDead)
                Die();

            //Cache la barre d'hp du boss
            if (hpDisplay.IsVisible)
                hpDisplay.Hide();
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

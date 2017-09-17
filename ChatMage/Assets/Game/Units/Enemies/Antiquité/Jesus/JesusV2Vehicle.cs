using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public JesusV2AnimatorV2 animator;
    public Transform rockTransporter;

    [Header("Throw")]
    public float throwSpeed;

    [Header("Visuals")]
    public Color flashColor = Color.red;
    public Color lowHPColor = Color.red;
    public SpriteRenderer[] spriteRenderers;

    private int maxHp;
    private BossHealthBarDisplay hpDisplay;
    private float timescaleIncrease;
    private bool damagable = true;

    protected override void Awake()
    {
        base.Awake();
        maxHp = hp;
        timescaleIncrease = 1 + ((finalTimescale - 1) / (maxHp - 1));
    }

    public void ShowHP()
    {
        hpDisplay = Game.instance.ui.bossHealthBar;
        hpDisplay.Show();
        hpDisplay.SetBossName(displayName);
        hpDisplay.SetSliderValue01(hp / (float)maxHp);
    }

    public void LightenUp()
    {
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.DOColor(Color.white, 1);
        }
    }

    private bool IsHPShown()
    {
        return Game.instance.ui.bossHealthBar.IsVisible;
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
        UpdateVisuals();
        FlashAnimation.FlashColor(this, spriteRenderers, invulnurableDuration, flashColor, () => damagable = true);

        //Bump unit
        if (unit != null && unit is Vehicle)
            (unit as Vehicle).Bump((unit.Position - Position).normalized * bumpForce, -1, BumpMode.VelocityAdd);


        if (hp > 0)
        {
            if (!IsHPShown())
                ShowHP();

            //Set boss slider
            hpDisplay.SetSliderValue01(hp / (float)maxHp);
        }
        else
        {
            //Dead !
            if (!isDead)
                Die();

            //Cache la barre d'hp du boss
            if (IsHPShown())
                hpDisplay.Hide();
        }

        return hp;
    }

    private void UpdateVisuals()
    {
        Color c = Color.Lerp(lowHPColor, Color.white, ((hp - 1f).Raised(0)) / (maxHp - 1f));
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = c;
        }
    }

    protected override void Die()
    {
        base.Die();

        collider.enabled = false;

        //Death anim
        Destroy();
    }
}

using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using DG.Tweening;

public class PlayerStats : PlayerComponent, IAttackable
{
    [Header("Camera Shake")]
    private const float onHitShakeStrength = 0.7f;

    [Header("Camera Shake")]
    public Color loseHpHitColor = Color.red;

    [NonSerialized]
    public Locker receivesTurnInput = new Locker();
    [NonSerialized]
    public StatInt health = new StatInt(3, 0, 3, BoundMode.Cap);
    [NonSerialized]
    public StatInt armor = new StatInt(0, 0, 3, BoundMode.Cap);
    [NonSerialized]
    public StatInt damageMultiplier = new StatInt(1, 0, int.MaxValue, BoundMode.Cap);
    [NonSerialized]
    public StatInt rightDamage = new StatInt(1, 0, int.MaxValue, BoundMode.Cap);
    [NonSerialized]
    public StatInt leftDamage = new StatInt(1, 0, int.MaxValue, BoundMode.Cap);
    [NonSerialized]
    public StatInt frontDamage = new StatInt(1, 0, int.MaxValue, BoundMode.Cap);
    [NonSerialized]
    public StatInt backDamage = new StatInt(1, 0, int.MaxValue, BoundMode.Cap);

    //� quel vitesse le smash s'unlock-t-il
    [NonSerialized]
    public StatInt smashRefreshRate = new StatInt(1, 0, int.MaxValue, BoundMode.Cap);

    //� combien de % est-ce que le cooldown se reset
    [NonSerialized]
    public StatFloat smashCooldownRate = new StatFloat(1, 0, float.MaxValue, BoundMode.Cap);

    // Cooldown reduction factor
    [NonSerialized]
    public StatFloat cooldownMultiplier = new StatFloat(1, 0, 1, BoundMode.Cap);

    [Header("OnHit Flash Animation")]
    public SpriteRenderer sprite;
    public float unhitableDuration;

    [Header("Low HP Animation")]
    public Color lowHPFlash_Color = Color.white;
    public float lowHPFlash_Speed = 1;
    public Ease lowHPFlash_Ease = Ease.InSine;

    [Header("Variables")]
    public bool damagable = true;
    public bool isVisible = true; // TODO
    public bool boostedAOE = false;

    public event SimpleEvent OnReceiveDamage;
    public event SimpleEvent OnRegen;
    public event Unit.Unit_Event OnUnitKilled;

    private bool lowHPAnimating = false;
    private Tween lowHPTween = null;
    public bool LowHPAnimating
    {
        get { return lowHPAnimating; }
        private set
        {
            if (lowHPAnimating != value)
            {
                lowHPAnimating = value;
                if (value)
                {
                    sprite.color = Color.white;
                    lowHPTween = sprite.DOColor(lowHPFlash_Color, 1 / lowHPFlash_Speed).SetEase(lowHPFlash_Ease);
                    lowHPTween.SetLoops(-1, LoopType.Yoyo);
                }
                else
                {
                    lowHPTween.Kill();
                    lowHPTween = null;
                    sprite.color = Color.white;
                }
            }
        }
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        //----------------------Damagable ----------------------//
        if (!damagable)
            return controller.playerItems.ItemCount;

        //----------------------Calcul le damage finale----------------------//
        //Passe a travers tous les equipables
        if (controller.playerDriver.Car is IAttackable)
            amount = (controller.playerDriver.Car as IAttackable).Attacked(on, amount, otherUnit, source);
        if (controller.playerSmash.Smash is IAttackable)
            amount = (controller.playerSmash.Smash as IAttackable).Attacked(on, amount, otherUnit, source);

        for (int i = 0; i < controller.playerItems.items.Count; i++)
        {
            Item item = controller.playerItems.items[i];
            if (item is IAttackable)
                amount = (item as IAttackable).Attacked(on, amount, otherUnit, source);
        }

        //----------------------Exit if dmg = 0----------------------//
        if (amount <= 0)
            return controller.playerItems.ItemCount;


        //----------------------Camera Shake----------------------//
        Vector2 camShakeDir;
        if (source != null)
            camShakeDir = source.transform.position - transform.position;
        else
            camShakeDir = -controller.vehicle.WorldDirection2D();
        Game.Instance.gameCamera.vectorShaker.Hit(camShakeDir.normalized * onHitShakeStrength);


        //----------------------VFX + SFX----------------------//
        Game.Instance.commonVfx.HitRed3(on.transform.position, MultiSize.Size.Medium);
        Game.Instance.commonSfx.Hit();


        if (controller.playerItems.ItemCount > 0)
        {
            //----------------------Flash Animation----------------------//
            damagable = false;
            UnitFlashAnimation.Flash(Game.Instance.Player.vehicle, sprite, unhitableDuration, () => damagable = true);

            //----------------------Remove items----------------------//
            for (int i = 0; i < amount; i++)
            {
                controller.playerItems.UnequipFirst();
            }
        }
        else
        {
            controller.vehicle.Kill();
        }

        if (OnReceiveDamage != null)
            OnReceiveDamage();

        UpdateLowHpAnimation();

        return controller.playerItems.ItemCount;
    }

    void OnDestroy()
    {
        LowHPAnimating = false;
    }

    public override void OnGameReady()
    {
        controller.playerItems.OnItemListChange += UpdateLowHpAnimation;
        UpdateLowHpAnimation();
    }

    void UpdateLowHpAnimation()
    {
        LowHPAnimating = !controller.vehicle.IsDead && controller.playerItems.ItemCount == 0;
    }

    public override void OnGameStarted()
    {
    }

    public void RegisterKilledUnit(Unit unit)
    {
        if (OnUnitKilled != null)
            OnUnitKilled(unit);
    }

    public void GiveHealth(int amount)
    {
        health++;
        if (OnRegen != null)
            OnRegen();
    }

    public void GiveArmor()
    {
        GiveArmor(1);
    }
    public void GiveArmor(int amount)
    {
        armor.Set(armor + amount);
    }

    public void EnableSprite()
    {
        sprite.enabled = true;
    }

    public float GetSmashJuiceReward()
    {
        return 0;
    }
}

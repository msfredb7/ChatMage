using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

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

    [Header("Flash Animation")]
    public SpriteRenderer sprite;
    public float unhitableDuration;

    [Header("Variables")]
    public bool damagable = true;
    public bool isVisible = true; // TODO
    public bool boostedAOE = false;

    public event SimpleEvent onReceiveDamage;
    public event SimpleEvent onRegen;
    public event Unit.Unit_Event onUnitKilled;

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (!damagable)
            return health + armor;

        //Calculate damage taken. Passe a travers tous les equipables.
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

        //Do nothing if damage <= 0
        if (amount <= 0)
            return health + armor;


        //Shake camera !
        Vector2 camShakeDir;
        if (source != null)
            camShakeDir = source.transform.position - transform.position;
        else
            camShakeDir = -controller.vehicle.WorldDirection2D();
        Game.instance.gameCamera.vectorShaker.Hit(camShakeDir.normalized * onHitShakeStrength);


        //Hit Animation
        damagable = false;
        Game.instance.commonVfx.MediumHit(on.transform.position, loseHpHitColor, SortingLayers.PLAYER);
        UnitFlashAnimation.Flash(Game.instance.Player.vehicle, sprite, unhitableDuration, () => damagable = true);


        //Reduce health / armor
        int damageToArmor = amount;
        amount -= armor;
        armor.Set(armor - damageToArmor);

        if (amount > 0)
            health.Set(health - amount);

        if (onReceiveDamage != null)
            onReceiveDamage();

        if (health <= 0)
            controller.vehicle.Kill();


        return health + armor;
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }

    public void Regen()
    {
        Regen(1);
    }

    public void Regen(int amount)
    {
        health++;
        if (onRegen != null)
            onRegen();
    }

    public void RegisterKilledUnit(Unit unit)
    {
        if (onUnitKilled != null)
            onUnitKilled(unit);
    }

    public void GiveArmor()
    {
        armor++;
    }

    public void EnableSprite()
    {
        sprite.enabled = true;
    }

    public float SmashJuice()
    {
        return 0;
    }
}

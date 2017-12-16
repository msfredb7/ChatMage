using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_SmashShield : Item, IAttackable
{

    public GameObject shieldPrefab;
    public float maxShieldDuration = 10;

    [NonSerialized, FullSerializer.fsIgnore]
    private GameObject shieldInstance;
    private float remainingShieldDuration;

    public override void Init(PlayerController player)
    {
        base.Init(player);

        //Spawn shield
        shieldInstance = Instantiate(shieldPrefab, player.body);
    }

    public override void OnGameReady()
    {
        //Shield off on start
        ShieldOff();
    }

    public override void OnGameStarted()
    {
        //Set listeners
        player.playerSmash.onSmashStarted += ShieldOn;
        player.playerSmash.onSmashCompleted += ShieldOff;
    }

    public override void OnUpdate()
    {
        bool wasOn = IsShieldOn();

        remainingShieldDuration -= player.vehicle.DeltaTime();

        if (!IsShieldOn() && wasOn)
        {
            ShieldOff();
        }
    }

    private void ShieldOn()
    {
        shieldInstance.SetActive(true);
        remainingShieldDuration = maxShieldDuration;
    }
    private void ShieldOff()
    {
        shieldInstance.SetActive(false);
        remainingShieldDuration = -1;
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();

        shieldInstance = null;
    }

    private bool IsShieldOn()
    {
        return remainingShieldDuration > 0;
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (IsShieldOn())
            return 0;
        else
            return amount;
    }

    public float GetSmashJuiceReward()
    {
        return 0;
    }

    public override void Equip(int duplicateIndex)
    {
        throw new NotImplementedException();
    }

    public override void Unequip()
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerVehicle : EnemyVehicle
{
    bool aboutToDie = false;
    public void Init()
    {
    }

    public void Hit(ColliderInfo info, ColliderListener listener)
    {
        if (aboutToDie)
            return;
        if (info.parentUnit.gameObject == Game.instance.Player.gameObject)
            Game.instance.Player.playerStats.Attacked(info, 1, this, listener.info);
    }

    public void ChargePlayer()
    {
        if (Game.instance.Player != null && Game.instance.Player.GetComponent<PlayerStats>().isVisible)
            GotoPosition(Game.instance.Player.transform.position);
    }

    IEnumerator DieNextFrame()
    {
        yield return null;
        Die();
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (aboutToDie)
            return 0;

        IAttackable attackable = unit.GetComponent<IAttackable>();
        if (attackable != null)
        {
            attackable.Attacked(source, 1, this, on);
        }

        aboutToDie = true;
        StartCoroutine(DieNextFrame());
        return 0;
    }
}

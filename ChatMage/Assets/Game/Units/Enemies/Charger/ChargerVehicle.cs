using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerVehicle : EnemyVehicle
{
    bool aboutToDie = false;
    public void Init()
    {
        SetBounds(Game.instance.ScreenBounds, 1);
        //GetComponentInChildren<SimpleColliderListener>().onTriggerEnter += Hit;
    }

    public void Hit(ColliderInfo info, ColliderListener listener)
    {
        if (aboutToDie)
            return;
        if (info.parentUnit.gameObject == Game.instance.Player.gameObject)
            Game.instance.Player.playerStats.Attacked(info, 1, this);
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

    public override int Attacked(ColliderInfo on, int amount, MonoBehaviour source)
    {
        if (aboutToDie)
            return 0;

        if (Game.instance.Player != null)
        {
            ColliderInfo playerCol = null;
            if (source is ColliderInfo)
                playerCol = source as ColliderInfo;
            else
            {
                playerCol = Game.instance.Player.playerCarTriggers.front.info;
                Debug.LogWarning("Ça serait meilleur si la source était le colliderlistener du char." +
                    " Je suis obligé de mettre 'front' arbitrairement.");
            }

            Game.instance.Player.playerStats.Attacked(playerCol, 1, this);
        }

        aboutToDie = true;
        StartCoroutine(DieNextFrame());
        return 0;
    }
}

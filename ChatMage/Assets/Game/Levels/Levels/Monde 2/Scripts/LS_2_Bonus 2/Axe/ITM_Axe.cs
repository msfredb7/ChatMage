using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Axe : Item
{
    public GameObject axePrefab;

    private bool alreadyDone = false;

    public override void Equip(int duplicateIndex)
    {
        throw new System.NotImplementedException();
    }

    public override void OnGameReady()
    {
        alreadyDone = false;
    }

    public override void OnGameStarted()
    {
        
    }

    public override void OnUpdate()
    {
        if(Game.Instance.Player != null)
        {
            if (!alreadyDone)
            {
                alreadyDone = true;
                Instantiate(axePrefab, Game.Instance.Player.vehicle.transform);
            }
        }
    }

    public override void Unequip()
    {
        throw new System.NotImplementedException();
    }
}

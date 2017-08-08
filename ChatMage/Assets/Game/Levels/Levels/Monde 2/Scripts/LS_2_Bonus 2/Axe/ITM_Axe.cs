using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Axe : Item
{
    public GameObject axePrefab;

    private bool alreadyDone = false;

    public override void OnGameReady()
    {
        alreadyDone = false;
    }

    public override void OnGameStarted()
    {
        
    }

    public override void OnUpdate()
    {
        if(Game.instance.Player != null)
        {
            if (!alreadyDone)
            {
                alreadyDone = true;
                Instantiate(axePrefab, Game.instance.Player.vehicle.transform);
            }
        }
    }
}

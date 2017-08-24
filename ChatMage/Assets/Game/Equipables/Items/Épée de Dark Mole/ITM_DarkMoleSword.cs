using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DarkMoleSword : Item
{
    public GameObject swordPrefab;

    public override void OnGameReady()
    {
        Instantiate(swordPrefab, player.transform);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }
}

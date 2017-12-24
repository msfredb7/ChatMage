using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DarkMoleSword : Item
{
    public DarkMoleSword swordPrefab;

    [FullSerializer.fsIgnore]
    private DarkMoleSword sword;

    public override void Equip(int duplicateIndex)
    {
        sword = swordPrefab.DuplicateGO(player.transform);
        sword.gameObject.SetActive(false);
        sword.SetController(player);

        //J'ai mis se delai pour comprenser avec le manque d'animation lorsqu'on gagne un item. 
        //C'est temporaire et ca devrais etre enlever dans le futur
        Game.instance.events.AddDelayedAction(() =>
        {
            sword.gameObject.SetActive(true);
            sword.OpenSwordSet(duplicateIndex);
        }, 0.5f);
    }

    public override void OnGameReady()
    {
        Instantiate(swordPrefab, player.transform);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.U))
            player.playerItems.Unequip(this);
    }

    public override void Unequip()
    {
        sword.BreakOff(sword.DestroyGO);
        sword = null;
    }
}

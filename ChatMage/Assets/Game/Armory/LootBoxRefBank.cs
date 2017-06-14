using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LootBoxRefBank : BaseBankManager<LootBoxRef>
{
    public override void Init()
    {
        CompleteInit();
    }

    protected override string Convert(LootBoxRef obj)
    {
        return obj.identifiant;
    }
}

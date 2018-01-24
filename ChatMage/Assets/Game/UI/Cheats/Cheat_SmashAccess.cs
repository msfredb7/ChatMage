using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_SmashAccess : CheatButton
{
    public override void Execute()
    {
        var dataSaver = DataSaverBank.Instance.GetDataSaver(DataSaverBank.Type.Armory);
        Armory.UnlockAccessToSmash(dataSaver);
    }
}

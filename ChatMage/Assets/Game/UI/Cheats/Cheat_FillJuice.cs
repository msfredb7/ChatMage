using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_FillJuice : CheatButton
{
    public override void Execute()
    {
        if (Game.Instance == null)
            return;
        SmashManager sm = Game.Instance.smashManager;
        sm.IncreaseSmashJuice(sm.MaxJuice);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_Lose : CheatButton
{
    public override void Execute()
    {
        Game.Instance.levelScript.Lose();
    }
}

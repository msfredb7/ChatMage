using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_Win : CheatButton
{
    public override void Execute()
    {
        Game.Instance.levelScript.Win();
    }
}

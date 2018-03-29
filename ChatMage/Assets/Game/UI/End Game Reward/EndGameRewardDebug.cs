using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class EndGameRewardDebug : BaseBehavior
{
    public WinAnimation winAnimation;

    [InspectorButton]
    public void Animate()
    {
        winAnimation.Animate();
    }
}

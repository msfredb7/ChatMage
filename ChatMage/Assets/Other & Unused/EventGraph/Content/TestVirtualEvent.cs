using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVirtualEvent : BaseVirtualEvent
{
    public string bullshitOne;
    public string bullshitTwo;
    public string bullshitThree;
    public string bullshitFour;
    public string bullshitFive;

    public override Color DefaultColor()
    {
        return new Color(1, 0.8f, 1, 1);
    }

    public override string DefaultLabel()
    {
        return "Test Virtual";
    }
}

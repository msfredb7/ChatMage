using System;
using UnityEngine;

public interface IEvent
{
    void Trigger();
}
public interface INodeDisplay
{
    Rect WindowRect { get; set; }
    void ResetWindowRectPos();
    void ResetWindowRectSize();
    Color DefaultColor();
    string name { get; set; }
    UnityEngine.Object AsObject();
    bool CanBeManuallyDestroyed();
    string DefaultLabel();
}

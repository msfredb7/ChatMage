using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Border
{
    public bool enabled;
    public float padding;
    public Border(bool enabled, float padding)
    {
        this.enabled = enabled;
        this.padding = padding;
    }
}

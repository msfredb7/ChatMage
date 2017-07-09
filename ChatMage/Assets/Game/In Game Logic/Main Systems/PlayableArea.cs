using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableArea
{
    private Box2D box;
    //private Vector2 min;
    //private Vector2 max;

    public PlayableArea()
    {
        Reset();
    }

    public void Reset()
    {
        Vector2 min = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
        Vector2 max = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        SetArea(min, max);
    }

    public Vector2 ClampToArea(Vector2 position)
    {
        return position.Clamped(box.min, box.max);
    }

    public Vector2 ClampToArea(Vector2 position, float borderReduction)
    {
        return position.Clamped(box.min + Vector2.one * borderReduction, box.max - Vector2.one * borderReduction);
    }

    public void SetArea(Box2D box)
    {
        this.box = box;
    }
    public void SetArea(Vector2 min, Vector2 max)
    {
        box = new Box2D(min, max);
    }
}

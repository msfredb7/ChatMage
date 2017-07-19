using UnityEngine;

[System.Serializable]
public struct Box2D
{
    public Vector2 min;
    public Vector2 max;
    public Box2D(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }

    public Vector2 Size
    {
        get { return new Vector2(LengthX, LengthY); }
    }

    public float LengthX
    {
        get { return max.x - min.x; }
    }
    public float LengthY
    {
        get { return max.y - min.y; }
    }

    public Vector2 Center
    {
        get
        {
            return new Vector2(
                (min.x + max.x) / 2,
                (min.y + max.y) / 2);
        }
    }

    public Vector2 GetRandomPositionWithin()
    {
        return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBounds : MonoBehaviour
{

    [Header("Borders")]
    public BoxCollider2D top;
    public BoxCollider2D bottom;
    public BoxCollider2D right;
    public BoxCollider2D left;

    public void Resize(Vector2 bounds)
    {
        float midY = bounds.y / 2;
        float midX = bounds.x / 2;
        right.transform.position = new Vector3(bounds.x + (right.size.x / 2), midY, 0);
        left.transform.position = new Vector3(-left.size.x / 2, midY, 0);
        top.transform.position = new Vector3(midX, top.size.y / 2 + bounds.y, 0);
        bottom.transform.position = new Vector3(midX, -top.size.y / 2, 0);
    }
}

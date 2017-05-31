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

    public void Resize(float width, float z)
    {
        right.transform.localPosition = new Vector3(width / 2 + 0.5f, 0, z);
        left.transform.position = new Vector3(-width / 2 - 0.5f, 0, z);
    }
}

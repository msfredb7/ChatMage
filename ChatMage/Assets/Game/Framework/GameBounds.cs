using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBounds : MonoBehaviour
{
    [Header("Borders")]
    public bool enabledByDefault = false;
    public BoxCollider2D top;
    public BoxCollider2D bottom;
    public BoxCollider2D right;
    public BoxCollider2D left;

    void Awake()
    {
        if (enabledByDefault)
            EnableAll();
        else
            DisableAll();
    }

    public void Resize(float width, float z)
    {
        right.transform.localPosition = new Vector3(width / 2 + 0.5f, 0, z);
        left.transform.position = new Vector3(-width / 2 - 0.5f, 0, z);
    }

    public void EnableAll()
    {
        top.enabled = true;
        bottom.enabled = true;
        right.enabled = true;
        left.enabled = true;
    }

    public void DisableAll()
    {
        top.enabled = false;
        bottom.enabled = false;
        right.enabled = false;
        left.enabled = false;
    }

}

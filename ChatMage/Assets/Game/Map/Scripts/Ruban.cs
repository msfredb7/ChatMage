using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruban : MonoBehaviour
{
    public const float STARTHEIGHT = 9.5f;
    public float length = 10;
    public Milestone[] milestones;

    private float progress;
    private Transform tr;

    void Awake()
    {
        tr = transform;
    }

    public void PutInScreen()
    {
        PutAt(0);
    }

    public void PutAt(float height)
    {
        tr.position = new Vector3(8, height + (length / 2));
        progress = 1 - ((GetTopHeight() - STARTHEIGHT) / length);
    }

    public float GetProgress()
    {
        return progress;
    }

    public float GetTopHeight()
    {
        return tr.position.y + (length / 2);
    }

    public float GetBottomHeight()
    {
        return tr.position.y - (length / 2);
    }

    public bool HasExitedScreen()
    {
        return GetTopHeight() < 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(16, length, 1));
    }
}
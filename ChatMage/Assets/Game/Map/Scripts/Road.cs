using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

    [Header("Must be < 1000 x 2")]
    public float length;

    private float teleportingMargin = 1;
    private float topHeight;
    private float bottomHeight;

    public float TopHeight { get { return topHeight; } }
    public float BottomHeight { get { return bottomHeight; } }

    void Awake()
    {
        topHeight = transform.position.y + length / 2;
        bottomHeight = transform.position.y - length / 2;
    }

    public bool IsTargetAboveRoad(float targetHeight)
    {
        return topHeight < targetHeight;
    }

    public bool IsTargetUnderRoad(float targetHeight)
    {
        return bottomHeight > targetHeight;
    }

    public float GetTeleportingBottom()
    {
        return bottomHeight + teleportingMargin;
    }

    public float GetTeleportingTop()
    {
        return topHeight - teleportingMargin;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(16, length, 1));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapping : MonoBehaviour {

    private float mapHeight;
    private float mapWidth;

    private float limitTop;
    private float limitBottom;
    private float limitRight;
    private float limitLeft;

    [SerializeField]
    private List<Waypoint> waypoints;

    public void Init(float height, float width)
    {
        mapHeight = height;
        mapWidth = width;
    }

    public void SetOffsets(float top, float bottom, float right, float left)
    {
        if (top >= mapHeight)
            limitTop = top;
        else
            limitTop = mapHeight;

        if (bottom >= 0)
            limitBottom = bottom;
        else
            limitBottom = 0;

        if (right >= mapWidth)
            limitRight = right;
        else
            limitRight = mapWidth;

        if (left >= 0)
            limitLeft = left;
        else
            limitLeft = 0;
    }
}

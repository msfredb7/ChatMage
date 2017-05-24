using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour {

    [SerializeField]
    private GameObject offsetWaypointTop;
    [SerializeField]
    private GameObject offsetWaypointBottom;
    [SerializeField]
    private GameObject offsetWaypointRight;
    [SerializeField]
    private GameObject offsetWaypointLeft;

    public float GetTopLimit()
    {
        return offsetWaypointTop.transform.position.y;
    }

    public float GetBottomLimit()
    {
        return offsetWaypointBottom.transform.position.y;
    }

    public float GetRightLimit()
    {
        return offsetWaypointRight.transform.position.x;
    }

    public float GetLeftLimit()
    {
        return offsetWaypointLeft.transform.position.x;
    }
}

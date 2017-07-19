using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : MonoBehaviour
{
    public enum CastMode { Linecast, Raycast, InifiniteRaycast }

    public Text display;

    public LayerMask layerMask;
    public int raycastCount;
    public Vector2 start;
    public Vector2 end;
    public CastMode mode;

    private Vector2 dir;
    private float length;
    private bool elo;

    void Start()
    {
        dir = (end - start).normalized;
        length = (end - start).magnitude;
    }

    public void Update()
    {
        Stopwatch sw = new Stopwatch(Stopwatch.PrintType.Milliseconds);
        switch (mode)
        {
            case CastMode.Linecast:
                for (int i = 0; i < raycastCount; i++)
                {
                    Physics2D.Linecast(start, end, layerMask);
                }
                break;
            case CastMode.Raycast:
                for (int i = 0; i < raycastCount; i++)
                {
                    Physics2D.Raycast(start, dir, length, layerMask);
                }
                break;
            case CastMode.InifiniteRaycast:
                for (int i = 0; i < raycastCount; i++)
                {
                    RaycastHit2D hit = Physics2D.Raycast(start, dir, float.PositiveInfinity , layerMask);
                    if ((hit.point - start).sqrMagnitude > 10)
                    {
                        elo = !elo;
                    }
                }
                break;
            default:
                break;
        }
        float timeML = sw.GetTime();

        if (timeML < 10)
            raycastCount += 10;
        else
            raycastCount -= 10;

        display.text = "" + raycastCount;

        Debug.DrawLine(start, end);
    }
}
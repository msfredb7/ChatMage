using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Milestone : BaseBehavior
{
    public enum StopType { At, Before, WhenMet}
    [InspectorHeader("Map Stop")]
    public bool stopMap;
    [InspectorShowIf("stopMap")]
    public StopType type;
    [InspectorHeader("Event")]
    public bool fireEventToLevelScript;
    [InspectorShowIf("fireEvent")]
    public string eventMessage;


    public void Execute()
    {
        if (stopMap)
        {
            switch (type)
            {
                case StopType.At:
                    break;
                case StopType.Before:
                    break;
                case StopType.WhenMet:
                    break;
                default:
                    break;
            }
        }

        if (fireEventToLevelScript)
        {

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(16, 0.25f, 1));
    }
}

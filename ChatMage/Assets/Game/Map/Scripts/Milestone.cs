using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Milestone : BaseBehavior
{
    public enum TriggerType { BottomOfScreen, TopOfScreen }
    [InspectorHeader("Trigger")]
    public TriggerType triggerOn;
    public bool disapearAfterTrigger = true;


    [InspectorHeader("Map Stop")]
    public bool completlyStopMap;
    public bool modifyCanScrollUp = false;
    [InspectorMargin(5), InspectorShowIf("modifyCanScrollUp")]
    public bool canScrollUpEffect = true;
    [InspectorMargin(5)]
    public bool modifyCanScrollDown = false;
    [InspectorShowIf("modifyCanScrollDown")]
    public bool canScrollDownEffect = true;


    [InspectorHeader("Event")]
    public bool fireEventToLevelScript;
    [InspectorShowIf("fireEventToLevelScript")]
    public string eventMessage;


    public void Execute()
    {
        if (modifyCanScrollDown)
        {
            //Game.instance.map.rubanPlayer.CanScrollDown = canScrollDownEffect;
        }
        if (modifyCanScrollUp)
        {
            //Game.instance.map.rubanPlayer.CanScrollUp = canScrollUpEffect;
        }
        if (completlyStopMap)
        {
            //Game.instance.map.rubanPlayer.Stopped = true;
        }

        if (fireEventToLevelScript)
        {
            Game.instance.currentLevel.ReceiveEvent(eventMessage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(triggerOn == TriggerType.BottomOfScreen ? 1 : 0, triggerOn == TriggerType.TopOfScreen ? 1 : 0, 0, 1);
        Gizmos.DrawCube(transform.position, new Vector3(16, disapearAfterTrigger ? 0.25f : 0.5f, 1));
    }
}

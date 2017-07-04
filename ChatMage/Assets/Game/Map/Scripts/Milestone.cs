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
    public bool modifyFollowPlayer;
    [InspectorShowIf("modifyFollowPlayer")]
    public bool followPlayerEffect = false;
    [InspectorMargin(5)]
    public bool modifyCanScrollUp = false;
    [InspectorShowIf("modifyCanScrollUp")]
    public bool canScrollUpEffect = true;
    [InspectorMargin(5)]
    public bool modifyCanScrollDown = false;
    [InspectorShowIf("modifyCanScrollDown")]
    public bool canScrollDownEffect = true;


    [InspectorHeader("Event")]
    public bool fireEventToLevelScript;
    [InspectorShowIf("fireEventToLevelScript")]
    public string eventMessage;


    public bool Execute()
    {
        if (!gameObject.activeSelf)
            return false;
        if (modifyCanScrollDown)
        {
            Game.instance.gameCamera.canScrollDown = canScrollDownEffect;
        }
        if (modifyCanScrollUp)
        {
            Game.instance.gameCamera.canScrollUp = canScrollUpEffect;
        }
        if (modifyFollowPlayer)
        {
            Game.instance.gameCamera.followPlayer = canScrollDownEffect;
        }

        if (fireEventToLevelScript)
        {
            Game.instance.currentLevel.ReceiveEvent(eventMessage);
        }
        return true;
    }

    public float GetHeight()
    {
        return transform.position.y + (triggerOn == TriggerType.BottomOfScreen ? 9 : 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(triggerOn == TriggerType.BottomOfScreen ? 1 : 0, triggerOn == TriggerType.TopOfScreen ? 1 : 0, 0, 1);
        Gizmos.DrawCube(transform.position, new Vector3(16, disapearAfterTrigger ? 0.25f : 0.5f, 1));
    }
}

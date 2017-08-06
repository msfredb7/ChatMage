using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;

public class Milestone : BaseBehavior
{
    public enum TriggerType { BottomOfScreen, TopOfScreen }
    [InspectorHeader("Trigger")]
    public TriggerType triggerOn = TriggerType.TopOfScreen;
    public bool disapearAfterTrigger = true;
    public bool gizmosAlwaysVisible = true;

    [InspectorMargin(12), InspectorHeader("AI Area")]
    public bool setAiArea;
    [InspectorShowIf("setAiArea")]
    public bool aiAreaAdjustToSceenRatio = true;
    [InspectorShowIf("setAiArea")]
    public Box2D aiAreaRelativeToMilestone = new Box2D(new Vector2(-8, -9f), new Vector2(8f, 0f));

    [InspectorMargin(12), InspectorHeader("Map Stop")]
    public bool modifyFollowPlayer;
    [InspectorShowIf("modifyFollowPlayer")]
    public bool followPlayerEffect = false;
    [InspectorMargin(5)]
    public bool setCameraMin = false;
    [InspectorShowIf("setCameraMin")]
    public float cameraMinRelativeToMilestone = 0;
    [InspectorMargin(5)]
    public bool setCameraMax = false;
    [InspectorShowIf("setCameraMax")]
    public float cameraMaxRelativeToMilestone = 0;


    [InspectorMargin(12), InspectorHeader("Event")]
    public bool fireEventToLevelScript;
    [InspectorShowIf("fireEventToLevelScript")]
    public string eventMessage;

    public UnityEvent onExecute = new UnityEvent();


    [InspectorMargin(12), InspectorHeader("Dialog")]
    public Dialoguing.Dialog dialog;



    public bool Execute()
    {
        if (!gameObject.activeInHierarchy)
            return false;

        onExecute.Invoke();

        if (setAiArea)
        {
            Vector2 min = aiAreaRelativeToMilestone.min + (Vector2)transform.position;
            Vector2 max = aiAreaRelativeToMilestone.max + (Vector2)transform.position;
            if (aiAreaAdjustToSceenRatio)
            {
                GameCamera cam = Game.instance.gameCamera;
                min = cam.AdjustVector(min);
                max = cam.AdjustVector(max);
            }
            Game.instance.aiArea.SetArea(min, max);
        }

        if (setCameraMax)
        {
            float delta = (triggerOn == TriggerType.BottomOfScreen) ? 4.5f : -4.5f;
            float yPos = transform.position.y + cameraMaxRelativeToMilestone + delta;
            Game.instance.gameCamera.maxHeight = yPos;
        }
        if (setCameraMin)
        {
            float delta = (triggerOn == TriggerType.BottomOfScreen) ? 4.5f : -4.5f;
            float yPos = transform.position.y + cameraMinRelativeToMilestone + delta;
            Game.instance.gameCamera.minHeight = yPos;
        }

        if (dialog != null)
            Game.instance.ui.dialogDisplay.StartDialog(dialog);

        if (modifyFollowPlayer)
        {
            Game.instance.gameCamera.followPlayer = followPlayerEffect;
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
        if (!gizmosAlwaysVisible)
            DrawGizmos();
    }

    void OnDrawGizmos()
    {
        if (gizmosAlwaysVisible)
            DrawGizmos();
    }

    void DrawGizmos()
    {
        Gizmos.color = new Color(triggerOn == TriggerType.BottomOfScreen ? 1 : 0, triggerOn == TriggerType.TopOfScreen ? 1 : 0, 0, 1);
        Gizmos.DrawCube(transform.position, new Vector3(16, disapearAfterTrigger ? 0.25f : 0.5f, 1));


        if (modifyFollowPlayer)
        {
            Vector3 pos = transform.position + Vector3.left * 8;
            if (followPlayerEffect)
            {
                Gizmos.DrawIcon(pos, "Gizmos CameraFollow");
            }
            else
            {
                Gizmos.DrawIcon(pos, "Gizmos CameraStopFollow");
            }
        }

        if (setAiArea)
        {
            Gizmos.color = new Color(0.2f, 1, 0, 0.4f);
            Gizmos.DrawCube((Vector2)transform.position + aiAreaRelativeToMilestone.Center, aiAreaRelativeToMilestone.Size);
        }

        if (setCameraMax)
        {
            float delta = (triggerOn == TriggerType.BottomOfScreen) ? 9 : 0;
            Vector3 pos = transform.position + Vector3.up * (cameraMaxRelativeToMilestone + delta);
            Gizmos.DrawIcon(pos, "Gizmos CameraTop");
        }
        if (setCameraMin)
        {
            float delta = (triggerOn == TriggerType.BottomOfScreen) ? 0 : -9;
            Vector3 pos = transform.position + Vector3.up * (cameraMinRelativeToMilestone + delta);
            Gizmos.DrawIcon(pos, "Gizmos CameraBottom");
        }
    }
}

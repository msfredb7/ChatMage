using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using GameEvents;

public class Milestone : FIPhysicalEvent, IEvent
{
    public Moment onTigger = new Moment();

    public bool gizmosAlwaysVisible = true;

    public enum TriggerType { BottomOfScreen, TopOfScreen }
    [InspectorHeader("Trigger")]
    public TriggerType triggerOn = TriggerType.TopOfScreen;
    public bool disapearAfterTrigger = true;

    [InspectorMargin(12), InspectorHeader("AI Area")]
    public bool setAiArea;
    [InspectorShowIf("setAiArea")]
    public bool aiAreaAdjustToSceenRatio = true;
    [InspectorShowIf("setAiArea")]
    public Box2D aiAreaRelativeToMilestone = new Box2D(
        new Vector2(-GameCamera.DEFAULT_SCREEN_WIDTH / 2, -GameCamera.DEFAULT_SCREEN_HEIGHT),
        new Vector2(GameCamera.DEFAULT_SCREEN_WIDTH / 2, 0f));

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

    [InspectorMargin(12), InspectorHeader("Cadre")]
    public bool useCadre = false;
    [InspectorShowIf("useCadre")]
    public Border _leftSide = new Border(true, 0);
    [InspectorShowIf("useCadre")]
    public Border _rightSide = new Border(true, 0);
    [InspectorShowIf("useCadre")]
    public Border _bottomSide = new Border(true, 0);
    [InspectorShowIf("useCadre")]
    public Border _topSide = new Border(true, 0);

    [InspectorMargin(12), InspectorHeader("Event")]
    public bool fireEventToLevelScript;
    [InspectorShowIf("fireEventToLevelScript")]
    public string eventMessage;

    public UnityEvent onExecute = new UnityEvent();


    [InspectorMargin(12), InspectorHeader("Dialog")]
    public Dialoguing.Dialog dialog;

    public bool Execute()
    {
        if (!enabled)
            return false;
        Trigger();
        return true;
    }

    public void Trigger()
    {
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
            Game.instance.gameCamera.maxHeight = cameraMaxRelativeToMilestone + GetCamerasCenter();
        }
        if (setCameraMin)
        {
            Game.instance.gameCamera.minHeight = cameraMinRelativeToMilestone + GetCamerasCenter();
        }

        if (dialog != null)
            Game.instance.ui.dialogDisplay.StartDialog(dialog);

        if (modifyFollowPlayer)
        {
            Game.instance.gameCamera.followPlayer = followPlayerEffect;
        }

        if (fireEventToLevelScript)
        {
            Game.instance.levelScript.ReceiveEvent(eventMessage);
        }

        if (useCadre)
        {
            AjusteCadre cadre = Game.instance.cadre;
            cadre.CenterTo(GetCamerasCenter());
            cadre.EnableSides(_leftSide.enabled, _rightSide.enabled, _bottomSide.enabled, _topSide.enabled);
            cadre.SetPaddings(_leftSide.padding, _rightSide.padding, _bottomSide.padding, _topSide.padding);
            cadre.Appear();
        }

        onTigger.Launch();
        onExecute.Invoke();
    }

    public float GetVirtualHeight()
    {
        return transform.position.y + (triggerOn == TriggerType.BottomOfScreen ? GameCamera.DEFAULT_SCREEN_HEIGHT : 0);
    }
    public float GetCamerasCenter()
    {
        float halfH = GameCamera.DEFAULT_SCREEN_HEIGHT / 2;
        return transform.position.y + (triggerOn == TriggerType.BottomOfScreen ? halfH : -halfH);
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
        Gizmos.DrawCube(transform.position, new Vector3(GameCamera.DEFAULT_SCREEN_WIDTH, disapearAfterTrigger ? 0.25f : 0.5f, 1));


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

        if (dialog != null)
        {
            Vector3 pos = transform.position + Vector3.right * 3;
            Gizmos.DrawIcon(pos, "Gizmos Dialog");
        }

        if (setAiArea)
        {
            Gizmos.color = new Color(0.2f, 0.2f, 1, 0.4f);
            Gizmos.DrawCube((Vector2)transform.position + aiAreaRelativeToMilestone.Center, aiAreaRelativeToMilestone.Size);
        }

        if (setCameraMax)
        {
            float delta = (triggerOn == TriggerType.BottomOfScreen) ? GameCamera.DEFAULT_SCREEN_HEIGHT : 0;
            Vector3 pos = transform.position + Vector3.up * (cameraMaxRelativeToMilestone + delta);
            Gizmos.DrawIcon(pos, "Gizmos CameraTop");
        }
        if (setCameraMin)
        {
            float delta = (triggerOn == TriggerType.BottomOfScreen) ? 0 : -GameCamera.DEFAULT_SCREEN_HEIGHT;
            Vector3 pos = transform.position + Vector3.up * (cameraMinRelativeToMilestone + delta);
            Gizmos.DrawIcon(pos, "Gizmos CameraBottom");
        }

        if (useCadre)
        {

        }
    }

    public override Color GUIColor()
    {
        return Colors.MILESTONE;
    }

    public override string NodeLabel()
    {
        return "MS: " + gameObject.name;
    }
}

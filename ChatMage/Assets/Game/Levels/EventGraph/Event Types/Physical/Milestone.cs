using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using GameEvents;

public interface IMilestone
{
    bool Execute(bool isGoingUp);
    void Disable();
    float GetVirtualHeight();
    MSTriggerType TriggerOn { get; }
    GameObject GameObj { get; }
}
public enum MSTriggerType { BottomOfScreen, TopOfScreen }

public class Milestone : FIPhysicalEvent, IEvent, IMilestone
{
    public Moment onTigger = new Moment();

    public bool gizmosAlwaysVisible = true;

    [InspectorHeader("Trigger")]
    public MSTriggerType triggerOn = MSTriggerType.TopOfScreen;
    public bool disapearAfterTrigger = true;
    public bool triggerOnGoingUp = true;
    public bool triggerOnGoingDown = true;

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

    public MSTriggerType TriggerOn { get { return triggerOn; } }
    public GameObject GameObj { get { return gameObject; } }

    public bool Execute(bool isGoingUp)
    {
        if (!enabled)
            return false;

        if (isGoingUp)
        {
            if (triggerOnGoingUp)
                Trigger();
            else
                return false;
        }
        else
        {
            if (triggerOnGoingDown)
                Trigger();
            else
                return false;
        }
        return disapearAfterTrigger;
    }

    public void Trigger()
    {
        if (setAiArea)
        {
            Vector2 min = aiAreaRelativeToMilestone.min + (Vector2)transform.position;
            Vector2 max = aiAreaRelativeToMilestone.max + (Vector2)transform.position;
            if (aiAreaAdjustToSceenRatio)
            {
                GameCamera cam = Game.Instance.gameCamera;
                min = cam.AdjustVector(min);
                max = cam.AdjustVector(max);
            }
            Game.Instance.aiArea.SetArea(min, max);
        }

        if (setCameraMax)
        {
            Game.Instance.gameCamera.maxHeight = cameraMaxRelativeToMilestone + GetCamerasCenter();
        }
        if (setCameraMin)
        {
            Game.Instance.gameCamera.minHeight = cameraMinRelativeToMilestone + GetCamerasCenter();
        }

        if (dialog != null)
            Game.Instance.ui.dialogDisplay.StartDialog(dialog);

        if (modifyFollowPlayer)
        {
            Game.Instance.gameCamera.followPlayer = followPlayerEffect;
        }

        if (fireEventToLevelScript)
        {
            Game.Instance.levelScript.ReceiveEvent(eventMessage);
        }

        if (useCadre)
        {
            AjusteCadre cadre = Game.Instance.cadre;
            cadre.CenterTo(GetCamerasCenter());
            cadre.EnableSides(_leftSide.enabled, _rightSide.enabled, _bottomSide.enabled, _topSide.enabled);
            cadre.SetPaddings(_leftSide.padding, _rightSide.padding, _bottomSide.padding, _topSide.padding);
            cadre.Appear();
        }

        onTigger.Launch();
        onExecute.Invoke();
    }

    public void Disable()
    {
        enabled = false;
    }

    public float GetVirtualHeight()
    {
        return transform.position.y + (triggerOn == MSTriggerType.BottomOfScreen ? GameCamera.DEFAULT_SCREEN_HEIGHT : 0);
    }
    public float GetCamerasCenter()
    {
        float halfH = GameCamera.DEFAULT_SCREEN_HEIGHT / 2;
        return transform.position.y + (triggerOn == MSTriggerType.BottomOfScreen ? halfH : -halfH);
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
        Gizmos.color = new Color(triggerOn == MSTriggerType.BottomOfScreen ? 1 : 0, triggerOn == MSTriggerType.TopOfScreen ? 1 : 0, 0, 1);
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
            float delta = (triggerOn == MSTriggerType.BottomOfScreen) ? GameCamera.DEFAULT_SCREEN_HEIGHT : 0;
            Vector3 pos = transform.position + Vector3.up * (cameraMaxRelativeToMilestone + delta);
            Gizmos.DrawIcon(pos, "Gizmos CameraTop");
        }
        if (setCameraMin)
        {
            float delta = (triggerOn == MSTriggerType.BottomOfScreen) ? 0 : -GameCamera.DEFAULT_SCREEN_HEIGHT;
            Vector3 pos = transform.position + Vector3.up * (cameraMinRelativeToMilestone + delta);
            Gizmos.DrawIcon(pos, "Gizmos CameraBottom");
        }

        if (useCadre)
        {
            float hh = GameCamera.DEFAULT_SCREEN_HEIGHT / 2;
            float hw = GameCamera.DEFAULT_SCREEN_WIDTH / 2;
            float yCenter = transform.position.y + (triggerOn == MSTriggerType.BottomOfScreen ? hh : -hh);
            Gizmos.color = new Color(0, 0.5f, 1, 1);
            if (_topSide.enabled)
            {
                float xL = -hw + _leftSide.padding;
                float xR = hw - _rightSide.padding;
                float y = hh - _topSide.padding + yCenter;
                Gizmos.DrawLine(new Vector3(xL, y, 0), new Vector3(xR, y, 0));
            }
            if (_bottomSide.enabled)
            {
                float xL = -hw + _leftSide.padding;
                float xR = hw - _rightSide.padding;
                float y = -hh + _bottomSide.padding + yCenter;
                Gizmos.DrawLine(new Vector3(xL, y, 0), new Vector3(xR, y, 0));
            }
            if (_rightSide.enabled)
            {
                float x = hw - _rightSide.padding;
                float yB = -hh + _bottomSide.padding + yCenter;
                float yT = hh - _topSide.padding + yCenter;
                Gizmos.DrawLine(new Vector3(x, yT, 0), new Vector3(x, yB, 0));
            }
            if (_leftSide.enabled)
            {
                float x = -hw + _leftSide.padding;
                float yB = -hh + _bottomSide.padding + yCenter;
                float yT = hh - _topSide.padding + yCenter;
                Gizmos.DrawLine(new Vector3(x, yT, 0), new Vector3(x, yB, 0));
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class Road : BaseBehavior
{
    [InspectorHeader("Doit etre < 1000 x 2")]
    public float topHeight = GameCamera.DEFAULT_SCREEN_HEIGHT / 2;
    public float bottomHeight = -GameCamera.DEFAULT_SCREEN_HEIGHT / 2;
    public bool setCameraMinMax = true;
    public bool showCameraTrails = true;

    public float TopHeight { get { return topHeight; } }
    public float BottomHeight { get { return bottomHeight; } }

    [InspectorHeader("NE PAS MODIFIER INGAME")]
    public List<Milestone> milestones;
    [fsProperty, InspectorDisabled()]
    private List<float> milestoneRelativeHeights;


    private float teleportingMargin = 1;
    private float lastRealPos;
    private bool lastRealPosSet = false;
    private bool isTeleporting = false;

    GameCamera gameCamera;

    public void Init(GameCamera gameCamera, RoadPlayer roadplayer)
    {
        this.gameCamera = gameCamera;
        if (setCameraMinMax)
        {
            ApplyMinMaxToCamera();
        }
    }

    public void ApplyMinMaxToCamera()
    {
        ApplyMinToCamera();
        ApplyMaxToCamera();
    }
    public void ApplyMinToCamera()
    {
        gameCamera.minHeight = bottomHeight + gameCamera.ScreenSize.y / 2;
    }
    public void ApplyMaxToCamera()
    {
        gameCamera.maxHeight = topHeight - gameCamera.ScreenSize.y / 2;
    }

    public void OnCompleteTeleport()
    {
        isTeleporting = false;
        lastRealPos += gameCamera.Top - lastRealPos;
    }

    public void OnStartTeleport()
    {
        isTeleporting = true;
    }

    void OnGameStated()
    {
        lastRealPos = gameCamera.Top;
    }

    void FixedUpdate()
    {
        if (milestones.Count <= 0)
        {
            enabled = false;
            return;
        }
        if (gameCamera == null || !Game.instance.gameStarted)
            return;

        //Ne va se faire qu'une seul fois
        if (!lastRealPosSet)
        {
            lastRealPos = gameCamera.Top;
            lastRealPosSet = true;
        }

        if (!isTeleporting)
            lastRealPos = CheckForMilestones(lastRealPos, gameCamera.Top);
    }


    /// <summary>
    /// Retourne la 'newRealPos'
    /// </summary>
    private float CheckForMilestones(float lastRealPos, float newRealPos)
    {
        float bottom = Mathf.Min(lastRealPos, newRealPos);
        float top = Mathf.Max(lastRealPos, newRealPos);

        for (int i = 0; i < milestones.Count; i++)
        {
            if (milestoneRelativeHeights[i] > top)
                break;

            if (milestoneRelativeHeights[i] >= bottom)
            {
                if (milestones[i].Execute() && milestones[i].disapearAfterTrigger)
                {
                    milestones[i].gameObject.SetActive(false);
                    milestones.RemoveAt(i);
                    milestoneRelativeHeights.RemoveAt(i);
                    i--;
                }
            }
        }

        return newRealPos;
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

    void OnDrawGizmos()
    {
        if (showCameraTrails)
        {
            float halfW = GameCamera.DEFAULT_SCREEN_WIDTH / 2;

            Bounds interiorBounds = new Bounds();
            interiorBounds.max = new Vector3(halfW, topHeight, 0);
            interiorBounds.min = new Vector3(-halfW, bottomHeight, 0);

            DebugExtension.DrawBounds(interiorBounds, new Color(1, 0, 1, 1));
            

            Bounds exteriorBounds = new Bounds();
            exteriorBounds.max = new Vector3(halfW + GameCamera.MAX_CAMERA_SHAKE, topHeight + GameCamera.MAX_CAMERA_SHAKE, 0);
            exteriorBounds.min = new Vector3(-halfW - GameCamera.MAX_CAMERA_SHAKE, bottomHeight - GameCamera.MAX_CAMERA_SHAKE, 0);

            DebugExtension.DrawBounds(exteriorBounds, new Color(1, 0, 1, 0.3f));
        }

        //Gizmos.color = new Color(0, 0, 1, 0.5F);
        //Gizmos.DrawCube(new Vector3(0, (topHeight + bottomHeight) / 2, 0),
        //    new Vector3(GameCamera.DEFAULT_SCREEN_WIDTH, topHeight - bottomHeight, 1));
    }

    [InspectorButton(), InspectorName("Gather All Milestones")]
    public void GatherAllMilestones()
    {
        milestones.Clear();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Milestone");

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetComponent<Milestone>() != null)
                milestones.Add(objs[i].GetComponent<Milestone>());
        }
    }

    [InspectorButton(), InspectorName("Apply Milestones")]
    public void ApplyMilestones()
    {
        int count = milestones.Count;

        Milestone[] newList = new Milestone[count];
        float[] newHeightList = new float[count];

        for (int i = 0; i < count; i++)
        {
            int plusBas = 0;
            float record = Mathf.Infinity;
            for (int u = 0; u < milestones.Count; u++)
            {
                if (milestones[u].GetVirtualHeight() < record)
                {
                    plusBas = u;
                    switch (milestones[u].triggerOn)
                    {
                        case Milestone.TriggerType.BottomOfScreen:
                            record = milestones[u].GetVirtualHeight();
                            break;
                        case Milestone.TriggerType.TopOfScreen:
                            record = milestones[u].GetVirtualHeight();
                            break;
                    }
                }
            }
            newHeightList[i] = record;
            newList[i] = milestones[plusBas];
            milestones.RemoveAt(plusBas);
        }
        milestoneRelativeHeights = new List<float>(count);
        milestoneRelativeHeights.AddRange(newHeightList);
        milestones.AddRange(newList);
    }
}

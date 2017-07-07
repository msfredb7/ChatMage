using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class Road : BaseBehavior
{
    [InspectorHeader("Doit etre < 1000 x 2")]
    public float topHeight = 4.5f;
    public float bottomHeight = -4.5f;
    public bool setCameraMinMax = true;

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
            gameCamera.minHeight = bottomHeight + gameCamera.ScreenSize.y / 2;
            gameCamera.maxHeight = topHeight - gameCamera.ScreenSize.y / 2;
        }
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
                if (milestones[i].disapearAfterTrigger && milestones[i].Execute())
                {
                    Destroy(milestones[i].gameObject);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawCube(new Vector3(0, (topHeight + bottomHeight)/2, 0), new Vector3(16, topHeight - bottomHeight, 1));
    }

    [InspectorButton(), InspectorName("Apply Milestones")]
    void ApplyMilestones()
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
                if (milestones[u].GetHeight() < record)
                {
                    plusBas = u;
                    switch (milestones[u].triggerOn)
                    {
                        case Milestone.TriggerType.BottomOfScreen:
                            record = milestones[u].GetHeight();
                            break;
                        case Milestone.TriggerType.TopOfScreen:
                            record = milestones[u].GetHeight();
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

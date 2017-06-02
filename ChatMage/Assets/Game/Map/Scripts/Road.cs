using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class Road : BaseBehavior
{

    [Header("Must be < 1000 x 2")]
    public float length;

    public float TopHeight { get { return topHeight; } }
    public float BottomHeight { get { return bottomHeight; } }

    [Header("NE PAS MODIFIER INGAME")]
    public List<Milestone> milestones;
    [fsProperty, InspectorDisabled()]
    private List<float> milestoneRelativeHeights;


    private float teleportingMargin = 1;
    private float topHeight;
    private float bottomHeight;
    private float lastRealPos;

    GameCamera gameCamera;


    protected override void Awake()
    {
        base.Awake();

        topHeight = transform.position.y + length / 2;
        bottomHeight = transform.position.y - length / 2;
    }

    public void Init(GameCamera gameCamera)
    {
        this.gameCamera = gameCamera;
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
            if (milestoneRelativeHeights[i] >= top)
                break;

            if (milestoneRelativeHeights[i] >= bottom)
            {
                milestones[i].Execute();
                if (milestones[i].disapearAfterTrigger)
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
        Gizmos.DrawCube(transform.position, new Vector3(16, length, 1));
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
                if (milestones[u].transform.position.y < record)
                {
                    plusBas = u;
                    switch (milestones[u].triggerOn)
                    {
                        case Milestone.TriggerType.BottomOfScreen:
                            record = milestones[u].transform.position.y + 9;
                            break;
                        case Milestone.TriggerType.TopOfScreen:
                            record = milestones[u].transform.position.y;
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

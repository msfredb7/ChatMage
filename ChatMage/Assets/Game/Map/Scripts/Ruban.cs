using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class Ruban : BaseBehavior
{
    public const float TOPLIMIT = 9.5f;
    public const float BOTTOMLIMIT = -0.5f;
    [Header("DOIT ETRE > 10")]
    public float length = 10;
    [Header("NE PAS MODIFIER INGAME")]
    public List<Milestone> milestones;
    [fsProperty, InspectorDisabled()]
    private List<float> milestoneRelativeHeights;

    private float progress;
    private float lastRealPos;
    private Transform tr;

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
                            record = milestones[u].transform.position.y - transform.position.y;
                            break;
                        case Milestone.TriggerType.TopOfScreen:
                            record = milestones[u].transform.position.y - 9 - transform.position.y;
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

    protected override void Awake()
    {
        base.Awake();
        tr = transform;
        lastRealPos = GetBottomHeight();
    }

    public void PutInScreen()
    {
        PutAt(0, true);
    }

    public void PutAt(float height, bool ignoreMilestones = false)
    {
        float newY = height + (length / 2);
        tr.position = new Vector3(8, newY);

        if (GetTopHeight() - TOPLIMIT < 0)
            progress = 1;
        else if (GetBottomHeight() - BOTTOMLIMIT > 0)
            progress = -1;
        else
            progress = 0;

        if (!ignoreMilestones)
            CheckForMilestones(lastRealPos, newY);

        lastRealPos = newY;
    }

    private void CheckForMilestones(float lastRealPos, float newRealPos)
    {
        float bottom = Mathf.Min(-lastRealPos, -newRealPos);
        float top = Mathf.Max(-lastRealPos, -newRealPos);

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
    }

    public float GetProgress()
    {
        return progress;
    }

    public float GetTopHeight()
    {
        return tr.position.y + (length / 2);
    }

    public float GetBottomHeight()
    {
        return tr.position.y - (length / 2);
    }

    public bool HasExitedScreen()
    {
        return GetTopHeight() < BOTTOMLIMIT || GetBottomHeight() > TOPLIMIT;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(16, length, 1));
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeColliderListener))]
public class TrailOfColliders : MonoBehaviour
{
    private class Segment
    {
        public Transform tr;
        public float diesAt;
    }

    public float width;
    public float minVertexLength = 0.5f;
    public float trailDuration = 2f;
    public Transform followTarget;
    public bool debugDraw = true;

    public TriggerEvent OnTriggerEnter { get { return comp.OnTriggerEnter; } set { comp.OnTriggerEnter = value; } }
    public TriggerEvent OnTriggerExit { get { return comp.OnTriggerExit; } set { comp.OnTriggerExit = value; } }

    private CompositeColliderListener comp;
    private InGameEvents ingameEvents;
    private Vector2 tipPos;
    private List<Segment> segments;

    private int head = -1;
    private int tail = 0;
    private int length = 0;

    void Awake()
    {
        comp = GetComponent<CompositeColliderListener>();
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
        tipPos = target.position;
    }

    void Start()
    {
        ingameEvents = Game.instance.currentLevel.inGameEvents;
        segments = new List<Segment>(5);
    }

    void Update()
    {

        if (length > 0)
        {
            //On detruit la queue
            float time = GameTime;
            if (time >= segments[tail].diesAt)
                RemoveSegment();
        }
        
        
        Vector2 targetPos= followTarget.position;
        Vector2 targetToHead = targetPos - tipPos;

        if (targetToHead.sqrMagnitude >= minVertexLength * minVertexLength)
        {
            AddSegment((tipPos + targetPos) / 2,
                new Vector2(targetToHead.magnitude*1.2f, width),
                targetToHead.ToAngle());
        }

        if (debugDraw)
        {
            Debug.DrawLine(tipPos, targetPos, Color.red);

            bool canDraw = false;
            Vector2 previousPos = Vector2.zero;

            for (int i = 0; i < length; i++)
            {
                int index = (tail + i).Mod(segments.Count);

                Vector2 segPos = segments[index].tr.position;

                if (canDraw)
                    Debug.DrawLine(segPos, previousPos, Color.green);

                canDraw = true;
                previousPos = segPos;
            }
        }
    }

    private float GameTime { get { return Time.time; } }

    Transform AddSegment(Vector2 position, Vector2 size, float rotation)
    {
        int nextHead = (head + 1).Mod(segments.Count);

        //On se mange la queue ?
        if (nextHead == tail || segments.Count == 0)
        {
            //Oui !
            Segment newSegment = NewSegment(position, size, rotation);

            if (tail > head)
                tail++;

            segments.Insert(head + 1, newSegment);
            head++;

            if (length == 0)
                tail = head;
        }
        else
        {
            //Non !
            AdjustSegment(segments[nextHead], position, size, rotation);
            head = nextHead;
        }

        length++;

        segments[head].diesAt = GameTime + trailDuration;

        tipPos = (Vector2)segments[head].tr.position + (Vector2.right * size.x / 2).Rotate(rotation);

        return segments[head].tr;
    }

    void RemoveSegment()
    {
        if (length == 0)
            return;

        segments[tail].tr.gameObject.SetActive(false);
        tail++;
        tail = tail.Mod(segments.Count);

        length--;

        if (length == 0)
            tipPos = followTarget.position;
    }

    void AdjustSegment(Segment segment, Vector2 position, Vector2 size, float rot)
    {
        Transform newTr = segment.tr;
        newTr.position = position;
        newTr.rotation = Quaternion.Euler(0, 0, rot);

        segment.tr.gameObject.SetActive(true);
    }

    Segment NewSegment(Vector2 position, Vector2 size, float rot)
    {
        GameObject newObj = new GameObject("col");
        newObj.layer = gameObject.layer;


        Transform newTr = newObj.transform;
        newTr.SetParent(transform);
        newTr.position = position;
        newTr.rotation = Quaternion.Euler(0,0, rot);


        BoxCollider2D box = newObj.AddComponent<BoxCollider2D>();
        box.usedByComposite = true;
        box.size = size;


        Segment newSegment = new Segment();
        newSegment.tr = newTr;

        return newSegment;
    }
}

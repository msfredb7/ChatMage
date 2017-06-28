using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Une liste de segment qui se créer derniere le 'transform' target.
/// </summary>
public class Trail
{
    public float maxTrailLength;
    public bool debugDraw = false;
    public Color debugDrawColor = Color.white;
    public event SimpleEvent onSegmentAdded;
    public event SimpleEvent onSegmentRemoved;
    public event SimpleEvent onTrailCleared;

    protected LinkedList<Segment> segments;
    protected Transform target;
    protected float trailLength = 0;
    protected float minSegmentLength;

    public LinkedList<Segment> Segments
    {
        get
        {
            return segments;
        }
    }

    public class Segment
    {
        public Vector2 position;
        public float distanceToNext;
        public Segment(Vector2 position, float lengthToNext) { this.position = position; this.distanceToNext = lengthToNext; }
    }

    public Trail(Transform followTarget,
        float maxLength,
        float minSegmentLength)
    {
        this.maxTrailLength = maxLength;
        this.minSegmentLength = minSegmentLength;
        this.target = followTarget;

        ResetTrail();
    }

    public void ResetTrail()
    {
        if (segments == null)
            segments = new LinkedList<Segment>();
        segments.Clear();

        if (onTrailCleared != null)
            onTrailCleared();


        segments.AddFirst(new Segment(target.position, 0));


        trailLength = 0;
    }

    public void Update()
    {
        Vector2 deltaPos = (Vector2)target.position - segments.First.Value.position;
        float deltaPosLength = deltaPos.magnitude;


        //Nouveau segment ?
        if (deltaPosLength > minSegmentLength)
        {
            //Add new segment
            trailLength += deltaPosLength;
            segments.AddFirst(new Segment(target.position, deltaPosLength));

            if (onSegmentAdded != null)
                onSegmentAdded();

            //Si le dernier segment est trop loin, on le coupe
            while (trailLength > maxTrailLength)
            {
                segments.RemoveLast();
                trailLength -= segments.Last.Value.distanceToNext;
                segments.Last.Value.distanceToNext = 0;

                if (onSegmentRemoved != null)
                    onSegmentRemoved();
            }

            OnNewNode();
        }

        if (debugDraw)
            DrawTrail(segments.Last);
    }

    protected virtual void OnNewNode()
    {
    }

    protected void DrawTrail(LinkedListNode<Segment> until)
    {
        LinkedListNode<Segment> element = segments.First;

        while (element != null && element.Next != null)
        {
            Debug.DrawLine(element.Value.position, element.Next.Value.position, debugDrawColor);

            if (element == until)
                break;

            element = element.Next;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Une trail qui vous notifie à chaque fois qu'une loop se crée.
/// La trail est alors reset.
/// </summary>
public class LoopTrail : Trail
{
    public float minConnectDistance;
    private float snapDistance;
    private Action<Vector2[], float> onLoop;

    public LoopTrail(Transform followTarget, 
        float maxLength, 
        float minSegmentLength, 
        float minConnectDistance,
        Action<Vector2[], float> onLoop)
        : base(followTarget, maxLength, minSegmentLength)
    {

        float c2 = (minSegmentLength / 2) * (minSegmentLength / 2);
        snapDistance = Mathf.Sqrt(c2 + c2) * 1.25f;

        this.onLoop = onLoop;
        this.minConnectDistance = minConnectDistance;

        ResetTrail();
    }

    protected override void OnNewNode()
    {
        base.OnNewNode();

        //On verifie si le nouveau segment entre en intersection
        float currentLength = 0;

        LinkedListNode<Segment> element = segments.Last;

        while (element != null && element != segments.First)
        {
            currentLength += element.Value.distanceToNext;

            //Trop proche de la tete ?
            if (currentLength - trailLength > -minConnectDistance)
                break;

            if ((element.Value.position - (Vector2)target.position).magnitude < snapDistance)
            {
                LoopOn(element);
                break;
            }

            element = element.Previous;
        }
    }

    void LoopOn(LinkedListNode<Segment> node)
    {
        if (onLoop != null)
        {
            //On snap le premier noeud au dernier 
            segments.First.Value.position = node.Value.position;
            segments.First.Value.distanceToNext = (segments.First.Value.position - segments.First.Next.Value.position).magnitude;

            //On compte le nombre de segment
            int count = 0;
            LinkedListNode<Segment> element = segments.First;
            while (element != node)
            {
                count++;
                element = element.Next;
            }

            //On place les positions dans l'array et on calcul la longeur total
            Vector2[] points = new Vector2[count];
            float sectionLength = 0;
            element = segments.First;

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = element.Value.position;
                sectionLength += element.Value.distanceToNext;
                element = element.Next;
            }

            onLoop(points, sectionLength);
        }

        ResetTrail();
    }
}

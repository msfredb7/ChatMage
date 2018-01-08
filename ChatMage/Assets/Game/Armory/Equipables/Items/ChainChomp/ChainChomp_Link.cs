﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChomp_Link : MonoBehaviour
{
    public ChainChomp_Link previousLink;
    public ChainChomp_Link nextLink;
    public HingeJoint2D nextHingeJoint;
    public HingeJoint2D previousHingeJoint;
    public bool updateJointsOnStart = true;

    [Header("Break Off Animation")]
    public float breakOff_linDrag = 1;
    public float breakOff_angDrag = 1;
    public float breakOff_minVel = 1;
    public float breakOff_maxVel = 3;
    public float breakOff_minAngVel = 360;
    public float breakOff_maxAngVel = 720;

    [System.NonSerialized] public Rigidbody2D rb;
    [System.NonSerialized] public Transform tr;

    private const float AX = 0;
    private const float AY = 0.15f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = transform;
    }

    void Start()
    {
        if (updateJointsOnStart)
            UpdateJoints();
    }

    public void SetPreviousJoint(Rigidbody2D rb, ChainChomp_Link link = null)
    {
        previousHingeJoint.connectedBody = rb;
        previousLink = link;
        UpdatePreviousJoint();
    }

    public void SetNextJoint(Rigidbody2D rb, ChainChomp_Link link = null)
    {
        nextHingeJoint.connectedBody = rb;
        nextLink = link;
        UpdateNextJoint();
    }

    public void UpdateJoints()
    {
        UpdateNextJoint();
        UpdatePreviousJoint();
    }
    public void UpdateNextJoint()
    {
        UpdateJoint(nextHingeJoint, true, nextLink);
    }
    public void UpdatePreviousJoint()
    {
        UpdateJoint(previousHingeJoint, false, previousLink);
    }

    private void UpdateJoint(HingeJoint2D joint, bool positiveAnchor, ChainChomp_Link otherLink)
    {
        if (joint != null && joint.connectedBody != null)
        {
            if (otherLink != null)
            {
                joint.connectedAnchor = positiveAnchor ? otherLink.PreviousAnchor() : otherLink.NextAnchor();
            }
            else
            {
                //C'est la target
                joint.connectedAnchor = Vector2.zero;
            }
        }
    }

    public Vector2 PreviousAnchor()
    {
        return previousHingeJoint.anchor;
    }
    public Vector2 NextAnchor()
    {
        return nextHingeJoint.anchor;
    }
    public Vector2 TransformedPreviousAnchor()
    {
        return tr.TransformPoint(previousHingeJoint.anchor);
    }
    public Vector2 TransformedNextAnchor()
    {
        return tr.TransformPoint(nextHingeJoint.anchor);
    }

    public void BreakOff()
    {
        if (previousHingeJoint != null)
            previousHingeJoint.enabled = false;
        if (nextHingeJoint != null)
            nextHingeJoint.enabled = false;

        float randomAngle = Random.Range(0, 360);
        float randomSpeed = Random.Range(breakOff_minVel, breakOff_maxVel);
        rb.velocity = randomAngle.ToVector() * randomSpeed;

        bool positiveRotate = randomAngle.RoundedToInt().IsEvenNumber();
        rb.angularVelocity = (positiveRotate ? 1 : -1) * Random.Range(breakOff_minAngVel, breakOff_maxAngVel);

        rb.drag = breakOff_linDrag;
        rb.angularDrag = breakOff_angDrag;
    }

    private static Vector2 PositiveAnchor { get { return new Vector2(AX, AY); } }
    private static Vector2 NegativeAnchor { get { return new Vector2(-AX, -AY); } }

    public bool IsBall { get { return previousHingeJoint == null; } }

    public void RemoveStrain()
    {
        Rigidbody2D next = nextHingeJoint != null ? nextHingeJoint.connectedBody : null;
        Rigidbody2D previous = previousHingeJoint != null ? previousHingeJoint.connectedBody : null;


        // POSITION
        var position = Vector2.zero;
        int c = 0;
        if (next != null)
        {
            position += next.position;
            c++;
        }
        if (previous != null)
        {
            position += previous.position;
            c++;
        }
        tr.position = position / c;

        //ROTATION
        Vector3 rotation = Vector3.zero;
        if (c == 1)
        {
            if (next != null)
                rotation = next.rotation.ToVector();
            else
                rotation = previous.rotation.ToVector();
        }
        else if (c == 2)
        {
            rotation = next.position - previous.position;
        }
        tr.rotation = Quaternion.Euler(rotation);
    }

    public void KillVelocity()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }
}

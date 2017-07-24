using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillier : MovingUnit
{
    [Header("Linking")]
    public SimpleColliderListener listener;
    public Collider2D breakCollider;
    public Transform centerSprite;
    public Transform cap;
    public Transform bottom;
    public Animator fallAnimator;

    [Header("Settings")]
    public bool breakOnlyByAllies = false;
    public float minBreakVelocity = 3;

    private bool strechCenter;

    protected override void Awake()
    {
        base.Awake();
        listener.onCollisionEnter += Listener_onCollisionEnter;
    }

    private void Listener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if (collision.relativeVelocity.sqrMagnitude < minBreakVelocity * minBreakVelocity)
            return;

        if (breakOnlyByAllies && other.parentUnit.allegiance != Allegiance.Ally)
            return;

        Break(listener.transform.position - other.transform.position);
    }

    public void Break(Vector2 direction)
    {
        Break(Vehicle.VectorToAngle(direction));
    }
    public void Break(float direction)
    {
        direction -= 90;
        rb.rotation = direction;
        breakCollider.enabled = false;
        strechCenter = true;
        fallAnimator.enabled = true;
    }

    protected override void Update()
    {
        if (strechCenter)
        {
            centerSprite.localPosition = cap.localPosition / 2;
            centerSprite.localScale = new Vector3(0.75f, cap.localPosition.y, 1);
        }
    }
}

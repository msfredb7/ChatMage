using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerCollision : SoundPlayer
{
    public SimpleColliderListener colliderListener;

    public collisionmoment CollisionMoment;
    public enum collisionmoment { onEnter = 0, onExit = 1 }

    public collisiontype CollisionType;
    public enum collisiontype { collision = 0, trigger = 1 }

    new void Start()
    {
        base.Start();
        if (colliderListener == null)
            return;
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
        colliderListener.onTriggerExit += ColliderListener_onTriggerExit;
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter;
        colliderListener.onCollisionExit += ColliderListener_onCollisionExit;
    }

    private void ColliderListener_onCollisionExit(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if (CollisionMoment == collisionmoment.onExit && CollisionType == collisiontype.collision)
            PlaySound();
    }

    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if (CollisionMoment == collisionmoment.onEnter && CollisionType == collisiontype.collision)
            PlaySound();
    }

    private void ColliderListener_onTriggerExit(ColliderInfo other, ColliderListener listener)
    {
        if (CollisionMoment == collisionmoment.onExit && CollisionType == collisiontype.trigger)
            PlaySound();
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (CollisionMoment == collisionmoment.onEnter && CollisionType == collisiontype.trigger)
            PlaySound();
    }
}

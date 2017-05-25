using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

[RequireComponent(typeof(RubanPlayer))]
public class MapFollower : BaseBehavior
{
    public bool isFollowing = true;
    
    [InspectorHeader("Settings")]
    public float marginSize = 9;
    public float centerSize = 0;
    public float maxPlaySpeed = 10;
    [InspectorHeader("Reverse Function")]
    public float forwardHeight = 4.5f;
    public float reverseHeight = 4.5f;
    public float heightChangeSpeed = 1;
    [InspectorRange(-2, 0)]
    public float reverseSpeedThreshold = 0;
    public bool canGoReverse = true;

    private bool forward = true;
    private float height = 4.5f;
    private Vehicle target;
    private RubanPlayer rubanPlayer;

    protected override void Awake()
    {
        base.Awake();
        rubanPlayer = GetComponent<RubanPlayer>();
        SetToForwardHeight();
    }

    public void SetToForwardHeight()
    {
        forward = true;
        height = forwardHeight;
    }

    public void SetToReverseHeight()
    {
        forward = false;
        height = reverseHeight;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }

    public void FollowPlayer()
    {
        target = Game.instance.Player.vehicle;
        isFollowing = true;
    }

    void FixedUpdate()
    {
        if (!isFollowing || target == null || rubanPlayer == null)
            return;

        UpdateZonePosition();
        
        float tHeight = target.Position.y;
        float relativeHeight = tHeight - height;

        int dir = relativeHeight < 0 ? -1 : 1;

        float strength = 0;

        //Is the target in the center ?
        if (Mathf.Abs(relativeHeight) > centerSize / 2)
        {
            strength = dir * Mathf.Min((Mathf.Abs(relativeHeight) - centerSize / 2) / (marginSize / 2 - centerSize / 2), 1);
        }

        rubanPlayer.PlaySpeed = strength * maxPlaySpeed;
    }

    void UpdateZonePosition()
    {

        if (!canGoReverse)
        {
            height = Mathf.MoveTowards(height, forwardHeight, ChangeRate());
            forward = true;
            return;
        }
        
        //Le joueur va vers l'avant ?
        if (forward)
        {
            //Going reverse ?
            if(target.Speed.y < reverseSpeedThreshold)
            {
                forward = false;
                HeightReverse();
            }
            else
            {
                HeightForward();
            }
        }
        else
        {
            //Going forward ?
            if (target.Speed.y >= reverseSpeedThreshold)
            {
                forward = true;
                HeightForward();
            }
            else
            {
                HeightReverse();
            }
        }
    }

    private void HeightForward()
    {
        height = Mathf.MoveTowards(height, forwardHeight, ChangeRate());
    }

    private void HeightReverse()
    {
        height = Mathf.MoveTowards(height, reverseHeight, ChangeRate());
    }

    private float ChangeRate()
    {
        return Mathf.Abs(target.Speed.y) * Time.fixedDeltaTime * heightChangeSpeed;
    }
}

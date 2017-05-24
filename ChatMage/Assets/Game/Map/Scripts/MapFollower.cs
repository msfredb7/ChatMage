using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

[RequireComponent(typeof(RubanPlayer))]
public class MapFollower : BaseBehavior
{
    public bool isFollowing = true;

    [InspectorRange(0, 1)]
    public float slider = 0;
    public float marginSize = 9;
    public float centerSize = 0;
    public float height = 4.5f;
    public float maxPlaySpeed = 10;

    private Transform target;
    private RubanPlayer rubanPlayer;

    protected override void Awake()
    {
        base.Awake();
        rubanPlayer = GetComponent<RubanPlayer>();
    }

    public void Follow(Transform target)
    {
        this.target = target;
        isFollowing = true;
    }

    void FixedUpdate()
    {
        if (!isFollowing || target == null || rubanPlayer == null)
            return;

        float tHeight = target.position.y;
        float relativeHeight = tHeight - height;

        int dir = relativeHeight < 0 ? -1 : 1;

        float strength = 0;

        //Is the target in the center ?
        if (Mathf.Abs(relativeHeight) > centerSize / 2)
        {
            strength = dir * Mathf.Min((Mathf.Abs(relativeHeight) - centerSize / 2) / (marginSize / 2 - centerSize / 2), 1);
        }

        slider = strength;

        rubanPlayer.PlaySpeed = strength * maxPlaySpeed;
    }
}

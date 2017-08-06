using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeVehicle : EnemyVehicle
{
    [Header("Slime")]
    public AnimationCurve visualScaleOverJump;
    public Transform visuals;
    public float jumpDuration;
    public float pauseDuration;
    public SlimeAnimator animator;

    public event SimpleEvent onJump;

    private bool IsJumping { get { return sqTimer > pauseDuration; } }
    private bool CanJump { get { return sqTimer < 0; } }
    private float JumpTime { get { return sqTimer - pauseDuration; } }
    private float sqTimer;
    private float maxJumpSpeed;
    private float rotBuffer;
    private bool rotBufferSet = false;

    void Start()
    {
        maxJumpSpeed = MoveSpeed;
    }

    public override int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, otherUnit, source);

        if (amount > 0 || isDead)
        {
            Die();
            return 0;
        }

        return 1;
    }

    protected override void Update()
    {
        base.Update();

        bool currentlyJumping = IsJumping;

        //Currently jumping ?
        if (currentlyJumping)
        {
            //visuals.localScale = Vector3.one * visualScaleOverJump.Evaluate(JumpTime / jumpDuration);
        }

        sqTimer -= DeltaTime();

        //Just stopped jumping ?
        if (currentlyJumping && !IsJumping)
        {
            if (rotBufferSet)
            {
                rotBufferSet = false;
                Rotation = rotBuffer;
            }
        }

        //Should we jump ?
        if (IsEnginOn && CanJump)
        {
            if (goingToTargetPosition)
            {
                Jump(targetPosition);
            }
            else
            {
                Jump();
            }
        }
    }

    public override float Rotation
    {
        set
        {
            if (IsJumping)
            {
                rotBuffer = value;
                rotBufferSet = true;
                return;
            }
            base.Rotation = value;
        }
    }

    void Jump(Vector2 destination)
    {
        _Jump();
        MoveSpeed = ((destination - Position).magnitude / jumpDuration).Capped(maxJumpSpeed);
    }
    void Jump()
    {
        _Jump();
        MoveSpeed = maxJumpSpeed;
    }
    void _Jump()
    {
        sqTimer = jumpDuration + pauseDuration;
        turnSpeed = 0;
        if (onJump != null)
            onJump();
    }

    protected override float ActualMoveSpeed()
    {
        return IsJumping ? currentMoveSpeed : 0;
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}

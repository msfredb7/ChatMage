using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrollRockAttackBehavior : BaseTweenBehavior<TrollVehicle>
{
    private JesusRockV2 rock;
    private bool turnTowardsTarget = false;
    private bool throwingRock = false;
    private bool rockInHands = false;
    private TweenCallback onComplete;

    public TrollRockAttackBehavior(TrollVehicle vehicle, JesusRockV2 rock, TweenCallback onComplete, bool alreadyPickedUp) : base(vehicle)
    {
        this.rock = rock;
        this.onComplete = onComplete;

        if (alreadyPickedUp)
            OnPickUpComplete();
        else
            tween = vehicle.animator.PickUpRockAnimation(AttachRockToArms).OnComplete(OnPickUpComplete);
    }

    public override void Enter(Unit target)
    {
        base.Enter(target);
        rock.PickedUpState();
        vehicle.Stop();
    }

    public override void Update(Unit target, float deltaTime)
    {
        if (turnTowardsTarget && target != null && !throwingRock)
        {
            //Vector entre moi et la cible
            Vector2 meToTarget = target.Position - vehicle.Position;

            float meToRockAngle = CCC.Math.Vectors.VectorToAngle(meToTarget);
            float deltaAngle = Mathf.Abs(vehicle.Rotation - meToRockAngle);

            //Est-ce qu'on regarde vers la cible ?
            if (deltaAngle < 5)
            {
                //On lance la roche !
                ThrowRock(meToTarget);
            }
            else
            {
                //On fait simplement se tourner vers la cible
                vehicle.TurnToDirection(meToRockAngle, deltaTime);
            }
        }
        else
        {
            vehicle.rb.angularVelocity = 0;
        }
    }

    protected override void OnCancel()
    {
        base.OnCancel();

        //Drop rock ?
        if (rockInHands)
        {
            rock.transform.SetParent(Game.instance.unitsContainer, true);
            rock.StoppedState();
        }
    }

    private void AttachRockToArms()
    {
        rockInHands = true;
        rock.transform.SetParent(vehicle.rockTransporter, true);
    }

    private void OnPickUpComplete()
    {
        turnTowardsTarget = true;
    }

    private void ThrowRock(Vector2 direction)
    {
        throwingRock = true;
        SetTween(vehicle.animator.ThrowRockAnimation(delegate ()
        {
            //Detach rock from arms + give speed
            rock.flySpeed = vehicle.throwSpeed * (vehicle.TimeScale + 1) / 2;
            rock.ThrownState(direction, vehicle);
            rock.transform.SetParent(Game.instance.unitsContainer, true);

            rockInHands = false;
        })
        .OnComplete(onComplete));
    }
}

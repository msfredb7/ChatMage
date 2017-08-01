using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using DG.Tweening;

public class SwordsmanBrain : EnemyBrain<SwordsmanVehicle>
{
    [InspectorHeader("Swordsman Brain")]
    public float startAttackRange = 2;
    public bool movementPrediction = true;
    [InspectorTooltip("Il va prédire le mouvement du joueur dans x s."), InspectorShowIf("movementPrediction")]
    public float thinkAheadLength = 1;

    //private bool isLosingArmor = false;

    protected override void Awake()
    {
        base.Awake();
        vehicle.onArmorLoss += Vehicle_onArmorLoss;
    }

    private void Vehicle_onArmorLoss()
    {
        //Get animation
        Tween armorLossAnim = vehicle.animator.LoseArmor();

        //Create forced behavior
        EnemyBehavior forcedBehavior = new TweenBehavior(vehicle, armorLossAnim);

        //Enforce behavior
        ForceBehavior(forcedBehavior, true);

        //Remove forced behavior when it's animation is complete
        armorLossAnim.OnComplete(delegate ()
        {
            RemoveForcedBehavior(forcedBehavior);
        });
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25F);
        Gizmos.DrawSphere(transform.position, startAttackRange);
    }

    protected override void UpdateWithTarget()
    {
        //Si on est entrain d'attaquer, on ne fait pas de changement a notre behavior
        if (!vehicle.CanAttack)
            return;

        Vector2 meToPPredic = meToTarget;
        if (target is MovingUnit)
            meToPPredic += (target as MovingUnit).Speed * thinkAheadLength;

        float dist = meToTarget.magnitude;

        if (dist <= startAttackRange ||  //En range d'attaque
            (movementPrediction && meToPPredic.sqrMagnitude <= startAttackRange * startAttackRange))
        {

            //Attack mode
            if (vehicle.CanAttack && !IsForcedIntoState)
            {
                SetBehavior(new SwordsmanAttackBehavior(vehicle));
            }
            else if (CanGoTo<LookTargetBehavior>())
            {
                SetBehavior(new LookTargetBehavior(vehicle));
            }
        }
        else
        {
            //Go to player
            if (CanGoTo<FollowBehavior>())
                SetBehavior(new FollowBehavior(vehicle));
        }
    }

    protected override void UpdateWithoutTarget()
    {
        if (!vehicle.IsAttacking && CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
    }
}

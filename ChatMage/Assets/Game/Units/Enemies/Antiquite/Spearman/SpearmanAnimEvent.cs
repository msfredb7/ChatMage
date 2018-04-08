using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanAnimEvent : StateMachineBehaviour
{
    public bool attack;
    public bool death;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attack)
        {
            animator.GetComponent<SpearmanAnimatorV2>()._AttackComplete();
        }
        else if (death)
        {
            animator.GetComponent<SpearmanAnimatorV2>()._DeathComplete();
        }
    }
}

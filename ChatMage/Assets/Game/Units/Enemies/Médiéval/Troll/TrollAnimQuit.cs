using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollAnimQuit : StateMachineBehaviour
{
    public bool throwAnim;
    public bool pickupAnim;
    public bool deathAnim;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (throwAnim)
        {
            animator.GetComponent<TrollAnimatorV2>()._ThrowComplete();
        }
        else if (pickupAnim)
        {
            animator.GetComponent<TrollAnimatorV2>()._PickUpComplete();
        }
        else if (deathAnim)
        {
            animator.GetComponent<TrollAnimatorV2>()._DeathComplete();
        }
    }
}

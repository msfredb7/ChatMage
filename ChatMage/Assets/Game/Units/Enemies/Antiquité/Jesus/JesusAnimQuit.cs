
using UnityEngine;

public class JesusAnimQuit : StateMachineBehaviour
{
    public bool throwAnim;
    public bool pickupAnim;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (pickupAnim)
        {
            animator.GetComponent<JesusV2AnimatorV2>()._PickUpComplete();
        }
        else if (throwAnim)
        {
            animator.GetComponent<JesusV2AnimatorV2>()._ThrowComplete();
        }
    }
}

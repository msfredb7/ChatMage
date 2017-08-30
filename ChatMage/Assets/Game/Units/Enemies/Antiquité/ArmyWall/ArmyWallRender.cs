using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyWallRender : MonoBehaviour
{
    public Animator spearmanAnimator;

    void Start()
    {
        spearmanAnimator.SetBool("moving", true);
    }
}

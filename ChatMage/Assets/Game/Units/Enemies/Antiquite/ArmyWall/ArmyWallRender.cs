using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyWallRender : MonoBehaviour
{
    public Animator spearmanAnimator;

    bool listeningToTimescale = false;

    void Start()
    {
        spearmanAnimator.SetBool("moving", true);
    }

    void Update()
    {
        if (!listeningToTimescale && Game.Instance != null)
        {
            listeningToTimescale = true;
            Game.Instance.worldTimeScale.onSet.AddListener((x) => spearmanAnimator.speed = x);
        }
    }

}

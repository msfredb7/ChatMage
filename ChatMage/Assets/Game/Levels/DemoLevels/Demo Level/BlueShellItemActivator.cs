using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BlueShellItemActivator : MonoBehaviour, IActivator
{
    public Transform[] stars;
    public float rotateDuration = 5;

    private bool hasBeenTriggered = false;

    void OnEnable()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].DORotate(Vector3.forward * 360, rotateDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].DOKill();
        }
    }
    public void Activate()
    {
        if (hasBeenTriggered)
            return;
        hasBeenTriggered = true;

        Debug.Log("Button Activated");
        if(Scenes.FindRootObject<PlayerBuilder>(SceneManager.GetSceneByName("Framework")).items[0] is ITM_BlueShell)
            (Scenes.FindRootObject<PlayerBuilder>(SceneManager.GetSceneByName("Framework")).items[0] as ITM_BlueShell).enable = true;
        // else ?
    }
}

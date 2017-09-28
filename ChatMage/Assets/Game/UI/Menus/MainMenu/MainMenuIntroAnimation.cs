using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuIntroAnimation : MonoBehaviour {

    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    public MainMenu mainMenu;
    public MainMenuCarAnimation carAnimation;

    void Start()
    {
        mainMenu.Init();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeDuration).OnComplete(delegate () {
            carAnimation.Init();
        });
    }
}

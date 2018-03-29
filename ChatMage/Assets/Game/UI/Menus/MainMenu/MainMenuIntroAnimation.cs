using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuIntroAnimation : MonoBehaviour
{
    public Image frontFade;
    public float fadeDuration = 1f;

    void Start()
    {
        frontFade.enabled = true;
        frontFade.DOFade(0, fadeDuration).onComplete = () => frontFade.enabled = false;
    }
}

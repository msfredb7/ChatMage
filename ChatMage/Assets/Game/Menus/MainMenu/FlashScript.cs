using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FlashScript : MonoBehaviour {

    public float fadeDuration = 1f;

    public void Init()
    {
        GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 0);
        FadeIn();
    }

    void FadeIn()
    {
        Tween myAnimation = GetComponent<Text>().DOFade(1, fadeDuration);
        myAnimation.OnComplete(FadeOut);
    }

    void FadeOut()
    {
        Tween myAnimation = GetComponent<Text>().DOFade(0, fadeDuration);
        myAnimation.OnComplete(FadeIn);
    }
}

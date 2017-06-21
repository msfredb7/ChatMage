using CCC.UI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotlightAnimation : MonoBehaviour {

    public float fadeDuration = 0.75f;

	public void Init(GameObject canvas)
    {
        gameObject.SetActive(true);
        foreach (Transform child in transform)
        {
            Image currentImage = child.GetComponent<Image>();
            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b,255);
            //currentImage.DOFade(255, fadeDuration);
        }
    }

    public void Close(Action onComplete)
    {
        foreach (Transform child in transform)
        {
            Image currentImage = child.GetComponent<Image>();
            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 0);
            //currentImage.DOFade(0, fadeDuration);
        }
        gameObject.SetActive(false);
        onComplete.Invoke();
    }
}

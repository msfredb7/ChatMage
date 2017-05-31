using CCC.UI;
using DG.Tweening;
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
            float currentAlpha = currentImage.color.a;
            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b,0);
            if (currentImage != null)
                currentImage.DOFade(currentAlpha, fadeDuration);
        }
    }

    public void Close()
    {
        foreach (Transform child in transform)
        {
            Image image = child.GetComponent<Image>();
            Color startcolor = image.color;
            if (image != null)
                image.DOFade(0, fadeDuration).OnComplete(delegate() {
                    gameObject.SetActive(false);
                    image.color = startcolor;
                });
        }
    }
}

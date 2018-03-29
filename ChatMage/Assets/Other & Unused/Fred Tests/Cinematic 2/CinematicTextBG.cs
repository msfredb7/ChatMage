using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CinematicTextBG : MonoBehaviour
{
    public Image image;

    void OnEnable()
    {
        image.DOFade(0.66f, 0.3f);
    }

    void OnDisable()
    {
        image.DOKill();
        image.DOFade(0, 0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AC130ShootAnim : MonoBehaviour {

    public float fadeDuration = 0.3f;
    public float reloadDelay = 0.75f;
    public CanvasGroup group;

    public void Show()
    {
        group.DOKill();
        group.DOFade(1, fadeDuration).SetUpdate(false);
    }

    public void Shoot(Vector2 screenPosition)
    {
        group.DOKill();
        group.alpha = 1;


        group.DOFade(0, fadeDuration).SetLoops(10, LoopType.Yoyo).SetDelay(reloadDelay).SetUpdate(false);
    }

    public void Hide()
    {
        group.DOKill();
        group.DOFade(0, fadeDuration).SetUpdate(false);
    }
}

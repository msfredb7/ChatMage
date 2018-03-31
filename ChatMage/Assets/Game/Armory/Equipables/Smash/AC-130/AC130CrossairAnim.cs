using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AC130CrossairAnim : MonoBehaviour
{
    [Header("Linking")]
    public CanvasGroup group;
    public RectTransform container;
    public CanvasScaler canvasScaler;

    [Header("Settings")]
    public float fadeDuration = 0.3f;
    public float reloadDelay = 0.5f;

    public void Show()
    {
        group.DOKill();
        group.DOFade(1, fadeDuration).SetUpdate(false);
    }

    public void Shoot(Vector2 pixelPosition, float reloadDuration)
    {
        //On trouve la position
        Vector2 scaling = new Vector2(canvasScaler.referenceResolution.x / Screen.width, canvasScaler.referenceResolution.y / Screen.height);
        Vector2 anchoredTouchPosition = new Vector2(pixelPosition.x * scaling.x, pixelPosition.y * scaling.y);


        //Wtf unity.
        container.anchoredPosition = Vector2.one;
        container.anchoredPosition = anchoredTouchPosition;

        group.DOKill();
        group.alpha = 1;

        int totalLoops = Mathf.RoundToInt(reloadDuration / fadeDuration);

        //Minimum 4
        if (totalLoops < 4)
            totalLoops = 4;

        //On s'assure d'avoir un nombre pair
        if (totalLoops % 2 != 0)
            totalLoops++;
        

        group.DOFade(0, reloadDuration / totalLoops)
            .SetLoops(totalLoops, LoopType.Yoyo)
            .SetDelay(Mathf.Min(reloadDelay, reloadDuration * 0.25f))
            .SetUpdate(false);
    }

    public void Hide()
    {
        group.DOKill();
        group.DOFade(0, fadeDuration).SetUpdate(false);
    }
}

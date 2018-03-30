using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageShadow : MonoBehaviour
{
    public Vector2 shadowOrientation;
    public Transform parentBase;
    public Transform basedOn;
    public float fullShadowScale = 1.15f;
    public float noShadowScale = 0.85f;

    private Vector2 anchor;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        anchor = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if(basedOn != null)
        {
            var targetScale = basedOn.lossyScale.x / parentBase.lossyScale.x;
            var mag = (targetScale - noShadowScale) / (fullShadowScale - noShadowScale);
            rectTransform.anchoredPosition = anchor + shadowOrientation * mag;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PCDemoMessage : MonoBehaviour
{
    public RectTransform firstText;
    public RectTransform secondText;
    public Vector2 firstTextDestination;
    public Vector2 secondTextDestination;
    public float duration;
    public Ease ease;

    bool hasSwapped = false;
    bool isSwapping = false;

    void Start()
    {
        enabled = false;
        PersistentLoader.LoadIfNotLoaded(() => enabled = true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            if (hasSwapped)
            {
                LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
            }
            else if (!isSwapping)
            {
                isSwapping = true;
                firstText.DOAnchorPos(firstTextDestination, duration).SetEase(ease);
                secondText.DOAnchorPos(secondTextDestination, duration).SetEase(ease).OnComplete(() => { isSwapping = false; hasSwapped = true; });
            }
        }
    }
}

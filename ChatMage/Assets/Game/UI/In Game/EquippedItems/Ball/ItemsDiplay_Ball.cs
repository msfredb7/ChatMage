using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemsDiplay_Ball : ItemsDisplay_Controller.PoolItem
{
    [SerializeField] private ItemsDisplay_Gravity gravity;
    public ItemsDisplay_Gravity GetGravityComponent() { return gravity; }

    [SerializeField] private Image itemImage;
    public Image GetItemImage() { return itemImage; }

    [SerializeField] private Image underBall;
    [SerializeField] private Image upperBall;

    [SerializeField] private Image[] brokenPieces;

    [Header("Break Animation"), SerializeField] private Vector2[] anchoredPiecesDestination;
    [SerializeField] private float breakDuration = 0.4f;
    [SerializeField] private Ease breakPiecesEase = Ease.OutQuint;
    [Header("Fade"), SerializeField] private float breakFadeDelay = 0.4f;
    [SerializeField] private float breakFadeDuration = 0.6f;
    [SerializeField] AudioPlayable breakSFX;

    [Header("Scale"), SerializeField] private float breakScaleDuration = 0.5f;
    [SerializeField] private float breakFinalSize = 0.6f;
    
    public Transform Tr { get; private set; }

    void Awake()
    {
        Tr = transform;
    }

    public void BreakAnimation()
    {
        if (breakSFX != null)
            DefaultAudioSources.PlaySFX(breakSFX);

        GetGravityComponent().enabled = false;
        upperBall.enabled = false;
        underBall.enabled = false;

        Sequence sq = DOTween.Sequence();

        //Scale up, mais slmt si on est pas deja gros
        if (Tr.localScale.x < breakFinalSize)
            sq.Join(Tr.DOScale(breakFinalSize, breakScaleDuration).SetEase(Ease.OutQuint));

        //Move pieces
        for (int i = 0; i < brokenPieces.Length; i++)
        {
            brokenPieces[i].enabled = true;
            sq.Join(brokenPieces[i].rectTransform.DOAnchorPos(anchoredPiecesDestination[i], breakDuration).SetEase(breakPiecesEase));
        }

        //Fade out
        for (int i = 0; i < brokenPieces.Length; i++)
        {
            sq.Insert(breakFadeDelay, brokenPieces[i].DOFade(0, breakFadeDuration));
        }
        sq.Insert(breakFadeDelay, itemImage.DOFade(0, breakFadeDuration));

        sq.OnComplete(PutBackIntoPool);
    }

    public void AppearAnimation()
    {
    }

    public override void Activate()
    {
        itemImage.SetAlpha(1);
        Tr.localScale = Vector3.one;
        upperBall.enabled = true;
        underBall.enabled = true;
        for (int i = 0; i < brokenPieces.Length; i++)
        {
            brokenPieces[i].enabled = false;
            brokenPieces[i].SetAlpha(1);
            brokenPieces[i].rectTransform.anchoredPosition = Vector2.zero;
        }
        gameObject.SetActive(true);
        AppearAnimation();
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

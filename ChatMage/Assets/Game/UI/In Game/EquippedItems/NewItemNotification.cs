using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewItemNotification : MonoBehaviour
{
    private const string GAMELOCK = "newItem";
    private const string EXCEPTION = "ITM_SimpleHP";

    [Header("Animation References")]
    public Image blackBG;
    public Sprite simpleHpSprite;
    public Text title;
    public Text description;
    public Image itemImage;
    public RectTransform godRays;
    public RectTransform container;
    public CanvasGroup content;
    public AudioAsset sfx;

    [Header("Settings")]
    public float pauseDuration = 2f;

    private bool animDone;
    private float cantClickDelay = 0.5f;

    Queue<Item> notifQueue = new Queue<Item>();
    float blackBGDefaultAlpha;
    Tween introTween;
    Tween outroTween;

    private void Awake()
    {
        blackBGDefaultAlpha = blackBG.color.a;
        gameObject.SetActive(false);
    }

    public void Init(PlayerItems playerItems)
    {
    }

    private void CheckForNextInQueue()
    {
        if (notifQueue.Count > 0)
            Notify(notifQueue.Dequeue());
    }

    public void Notify(Item item)
    {
        cantClickDelay = 0.6f;
        animDone = false;

        gameObject.SetActive(true);

        Game.Instance.gameRunning.Lock(GAMELOCK);

        // Setup
        container.localScale = Vector3.one * 0.25f;
        content.alpha = 0;
        blackBG.SetAlpha(0);
        description.text = item.description;
        itemImage.sprite = item.name == EXCEPTION ? simpleHpSprite : item.ingameIcon;

        DefaultAudioSources.PlayStaticSFX(sfx);

        // Open
        var sq = DOTween.Sequence().SetUpdate(true);
        sq.Append(blackBG.DOFade(blackBGDefaultAlpha, 0.25f));
        sq.Join(container.DOScale(1, 0.35f).SetEase(Ease.OutBack));
        sq.Join(content.DOFade(1, 0.15f));

        // Pause
        sq.AppendInterval(pauseDuration);
        sq.AppendCallback(() => animDone = true);

        introTween = sq;

        // Spin dem rays
        godRays.DOKill();
        godRays.DORotate(Vector3.forward * 360, sq.Duration() * 2, RotateMode.LocalAxisAdd).SetUpdate(true).SetLoops(-1);
    }

    void Update()
    {
        if (animDone)
            cantClickDelay -= Time.deltaTime;

        if (Input.anyKeyDown)
        {
            if (animDone && cantClickDelay <= 0)
            {
                // Close
                var sq = DOTween.Sequence().SetUpdate(true);
                sq.Append(content.DOFade(0, 0.4f));
                sq.Join(blackBG.DOFade(0, 0.4f));
                sq.OnComplete(() =>
                {
                    Game.Instance.gameRunning.Unlock(GAMELOCK);
                    gameObject.SetActive(false);
                    CheckForNextInQueue();
                });
                outroTween = sq;
            }
        }
    }

    private void OnDestroy()
    {
        if (introTween != null && introTween.IsActive())
        {
            introTween.Kill();
            outroTween.Kill();
        }

    }
}

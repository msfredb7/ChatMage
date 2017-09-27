using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SmashDisplay : MonoBehaviour
{
    public bool showOnGameStart = true;
    public bool canBeShown = true;

    [Header("Progression"), Tooltip("Progression ^ power = displayed progression")]
    public float progressionPower = 2;

    [Header("Fade")]
    public CanvasGroup group;
    public float fadeDuration = 0.5f;

    [Header("Mask Top")]
    public RectTransform topMask;
    public Vector2 topMask_startSizeDelta;
    public Vector2 topMask_endSizeDelta;

    [Header("Mask Bottom")]
    public RectTransform bottomMask;
    public Vector2 bottomMask_startSizeDelta;
    public Vector2 bottomMask_endSizeDelta;

    [Header("Cintille")]
    public Image cintille;
    public float cintilleDuration = 0.2f;
    public float cintilleFinalScale = 1;

    SmashManager smasher;

    void Awake()
    {
        Hide(false);
        cintille.enabled = false;
    }

    public void Init(PlayerController player)
    {
        smasher = Game.instance.smashManager;
        if (!player.playerSmash.SmashEquipped)
            gameObject.SetActive(false);

        //Events
        Game.instance.onGameStarted += Game_onGameStarted;
        smasher.onSmashSpawned += Smasher_onSmashSpawned;
        player.playerSmash.onSmashCompleted += PlayerSmash_onSmashCompleted;
    }

    private void PlayerSmash_onSmashCompleted()
    {
        Show(true);
    }

    private void Smasher_onSmashSpawned()
    {
        cintille.rectTransform.localScale = Vector3.zero;
        cintille.enabled = true;
        cintille.transform.DORotate(Vector3.forward * 360, cintilleDuration, RotateMode.LocalAxisAdd);
        cintille.transform.DOScale(cintilleFinalScale, cintilleDuration / 2)
            .SetEase(Ease.OutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(delegate ()
            {
                cintille.enabled = false;
                Hide(true);
            });

    }

    private void Game_onGameStarted()
    {
        if (showOnGameStart)
            Show(true);
    }

    void Update()
    {
        if (smasher != null)
        {
            if(Game.instance.smashManager.activateV2)
                SetDisplay(Mathf.Pow(1f - (smasher.smashCounter / smasher.smashCounterMax), progressionPower));
            else
                SetDisplay(Mathf.Pow(1f - (smasher.RemainingTime / smasher.TotalCooldown), progressionPower));
        }
    }

    public void Hide(bool fadeOut = false)
    {
        if (fadeOut)
        {
            group.DOKill();
            group.DOFade(0, fadeDuration).OnComplete(delegate ()
            {
                gameObject.SetActive(false);
            });
        }
        else
        {
            gameObject.SetActive(false);
            group.alpha = 0;
        }
    }

    public void Show(bool fadeIn = false)
    {
        if (!canBeShown)
            return;

        gameObject.SetActive(true);

        if (fadeIn)
        {
            group.DOKill();
            group.DOFade(1, fadeDuration);
        }
        else
        {
            group.alpha = 1;
        }
    }

    void SetDisplay(float completion)
    {
        topMask.sizeDelta = Vector2.Lerp(topMask_startSizeDelta, topMask_endSizeDelta, completion);
        bottomMask.sizeDelta = Vector2.Lerp(bottomMask_startSizeDelta, bottomMask_endSizeDelta, completion);

    }
}

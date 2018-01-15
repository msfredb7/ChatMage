using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Pack : MovingUnit
{
    [Header("Pack"), Header("Pick up settings")]
    public bool isPreSpawned = true;

    [Header("Linking")]
    public SimpleColliderListener colliderListener;
    public Collider2D myCollider;
    public SpriteRenderer shadow;
    public SpriteRenderer[] sprites;
    public Transform spriteHolder;

    [Header("Animation Settings")]
    public string sortingLayerAbovePlayer = "Above Player";
    public float spawnDuration = 1;
    public int spawnRotateLoops = 5;
    public float flipMaxSize = 1.6f;

    [Header("Audio")]
    public AudioPlayable pickupSound;

    private Tween animTween;
    private string sortingLayerStd;

    private void Start()
    {
        colliderListener.onTriggerEnter += PickUp;
        sortingLayerStd = sprites[0].sortingLayerName;

        if (isPreSpawned)
        {
            CanPickUp = true;
        }
        else
        {
            CanPickUp = false;


            //Spawn animation
            Vector3 spriteNormalScal = spriteHolder.localScale;
            Color shadowColor = shadow.color;
            float shadowA = shadowColor.a;
            shadow.color = shadowColor.ChangedAlpha(0);

            Sequence sq = DOTween.Sequence();

            animTween = sq;

            //Scale up
            sq.Append(
                spriteHolder.transform.DOScale(spriteNormalScal * flipMaxSize, spawnDuration / 2)
                .SetEase(Ease.OutSine));

            //Scale down
            sq.Append(
                spriteHolder.transform.DOScale(spriteNormalScal, spawnDuration / 2)
                .SetEase(Ease.InSine));

            //Spin !
            sq.Insert(
                0,
                spriteHolder.transform.DORotate(Vector3.up * 180f, spawnDuration / spawnRotateLoops, RotateMode.LocalAxisAdd)
                .SetLoops(spawnRotateLoops, LoopType.Restart)
                .SetEase(Ease.Linear));

            //Shadow fade in
            sq.Insert(spawnDuration * 0.8f, shadow.DOFade(shadowA, spawnDuration * 0.2f));

            sq.OnComplete(delegate () { CanPickUp = true; });

            UpdateTimeScale(this);

            onTimeScaleChange += UpdateTimeScale;
        }
    }

    void UpdateTimeScale(Unit myself)
    {
        if (animTween != null)
            animTween.timeScale = TimeScale;
    }

    bool CanPickUp
    {
        get { return myCollider.enabled; }
        set
        {
            myCollider.enabled = value;
            string sortingLayerName = value ? sortingLayerStd : sortingLayerAbovePlayer;
            foreach (var spr in sprites)
            {
                spr.sortingLayerName = sortingLayerName;
            }
        }
    }

    public void PickUp(ColliderInfo info, ColliderListener listener)
    {
        if (!isDead && info.parentUnit == Game.Instance.Player.vehicle)
        {
            if (pickupSound != null)
                SoundManager.PlaySFX(pickupSound);
            OnPickUp();
            Die();
        }
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }

    protected abstract void OnPickUp();
}

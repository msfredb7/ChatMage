using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmorPacks : MovingUnit
{
    [Header("Health Pack"), Header("Linking")]
    public SimpleColliderListener colliderListener;
    public Collider2D myCollider;
    public SpriteRenderer sprite;
    public int regenAmount = 1;

    [Header("Animation Settings")]
    public string sortingLayerAbovePlayer;
    public float spawnDuration;
    public int spawnRotateLoops;
    public float spawnMaxSize;
    public float spawnMinSize;

    private Tween animTween;
    private string sortingLayerStd;

    private void Start()
    {
        sortingLayerStd = sprite.sortingLayerName;

        CanPickUp = false;
        colliderListener.onTriggerEnter += PickUp;


        //Spawn animation
        sprite.transform.localScale = Vector3.one * spawnMinSize;

        Sequence sq = DOTween.Sequence();

        animTween = sq;

        //Scale up
        sq.Append(
            sprite.transform.DOScale(spawnMaxSize, spawnDuration / 2)
            .SetEase(Ease.OutSine));

        //Scale down
        sq.Append(
            sprite.transform.DOScale(spawnMinSize, spawnDuration / 2)
            .SetEase(Ease.InSine));

        //Spin !
        sq.Insert(
            0,
            sprite.transform.DORotate(Vector3.up * 180f, spawnDuration / spawnRotateLoops, RotateMode.Fast)
            .SetLoops(spawnRotateLoops, LoopType.Restart)
            .SetEase(Ease.Linear));

        sq.OnComplete(delegate () { CanPickUp = true; });

        UpdateTimeScale(this);

        onTimeScaleChange += UpdateTimeScale;
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
            sprite.sortingLayerName = value ? sortingLayerStd : sortingLayerAbovePlayer;
        }
    }

    public void PickUp(ColliderInfo info, ColliderListener listener)
    {
        if (info.parentUnit == Game.instance.Player.vehicle)
            Game.instance.Player.playerStats.GiveArmor();
        Die();
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}

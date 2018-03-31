using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AC130Bullet : Unit
{
    [Header("Linking")]
    public Collider2D col;
    public SimpleColliderListener listener;

    [Header("Settings")]
    public float arriveDelay = 2;
    [Range(0, 1)]
    public float timescaleInfluence = 0.5f;

    [Header("Audio")]
    public AudioAsset explosionSFX;

    [Header("Animation")]
    public SpriteRenderer cloudTwo;
    public float cloudTwoFinalScale = 2;
    public SpriteRenderer cloudThree;
    public float cloudThreeFinalScale = 3;
    public SpriteRenderer shockWave;
    public float shockWaveFinalSize = 10;

    private bool hasReceivedEvent = false;
    private bool lastedAtLeastAFrame = false;
    private Image blackFade;
    private Tween tween;
    private float detonateTimer;

    void Start()
    {
        listener.onTriggerEnter += Listener_onTriggerEnter;

        detonateTimer = arriveDelay;
    }

    protected override void Update()
    {
        base.Update();

        if(detonateTimer > 0)
        {
            detonateTimer -= Mathf.Lerp(Time.deltaTime, DeltaTime(), timescaleInfluence);
            if (detonateTimer <= 0)
            {
                Detonate();
            }
        }
    }

    public void Init(Image blackFade)
    {
        this.blackFade = blackFade;
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        hasReceivedEvent = true;

        //Attack unit
        var unit = other.parentUnit;
        if (unit.allegiance == Allegiance.Enemy || unit.allegiance == Allegiance.SmashBall)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            var wasDead = unit.IsDead;
            if (attackable != null)
                attackable.Attacked(other, 1, listener.info.parentUnit, listener.info);
            if (unit.IsDead && !wasDead && Game.Instance.Player != null)
                Game.Instance.Player.playerStats.RegisterKilledUnit(unit);
        }
    }

    void Animate()
    {
        cloudTwo.enabled = true;
        cloudThree.enabled = true;
        shockWave.enabled = true;

        Sequence sq = DOTween.Sequence();

        sq.Join(cloudTwo.transform.DOScale(cloudTwoFinalScale, 3).SetEase(Ease.OutCirc));
        sq.Join(cloudThree.transform.DOScale(cloudThreeFinalScale, 3).SetEase(Ease.OutCirc));
        sq.Join(shockWave.transform.DOScale(shockWaveFinalSize, 0.5f).SetEase(Ease.Linear));
        sq.Join(shockWave.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(delegate () { shockWave.enabled = false; }));

        blackFade.color = new Color(1, 1, 1, 0.35f);

        sq.InsertCallback(0.05f, delegate () { blackFade.color = new Color(0, 0, 0, 0); });
        //sq.Insert(0.05f, blackFade.DOFade(0.35f, 0.5f));
        //sq.Insert(0.55f, blackFade.DOFade(0, 2));

        sq.Insert(1, cloudTwo.DOFade(0, 4).SetEase(Ease.Linear));
        sq.Join(cloudThree.DOFade(0, 4).SetEase(Ease.Linear));

        sq.OnComplete(Die);

        sq.timeScale = timeScale;

        tween = sq;

        Game.Instance.gameCamera.vectorShaker.Shake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (col.enabled)
        {
            if (lastedAtLeastAFrame || hasReceivedEvent)
                col.enabled = false;

            lastedAtLeastAFrame = true;
        }
    }

    void Detonate()
    {
        if (explosionSFX != null)
            DefaultAudioSources.PlaySFX(explosionSFX);
        col.enabled = true;
        Animate();
    }

    public override float TimeScale
    {
        get
        {
            return base.TimeScale;
        }

        set
        {
            base.TimeScale = value;

            if (tween != null && tween.IsActive())
            {
                tween.timeScale = timeScale;
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        if (tween != null && tween.IsActive())
            tween.Kill();

        Destroy();
    }
}

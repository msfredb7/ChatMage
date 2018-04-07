using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Unit
{
    [Header("Explosion Settings")]
    [Forward]
    public bool detonateOnStart = true;
    public bool killOnComplete = true;
    public Targets targets;
    public float radius;
    public SimpleColliderListener colliderListener;
    [ShowIf("UseColliderListener", HideShowBaseAttribute.Type.Property)]
    public float colliderDuration = 0.5f;
    [HideIf("UseColliderListener", HideShowBaseAttribute.Type.Property)]
    public LayerMask explosionLayerMask;
    public bool UseColliderListener { get { return colliderListener != null; } }

    [Header("Animation Settings")]
    public AudioPlayable explosion_SFX;

    [Header("Camera Shake")]
    public float cameraShake_strength = 0.75f;

    [Header("Core Animation")]
    public Animator coreAnimator;
    public float coreSizeMultiplier = 0.65f;
    public float coreBeginSize = 0.5f;
    public float coreGrowTime = 0.36f;
    public Ease coreEase = Ease.OutBack;

    [Header("Light Animation")]
    public SpriteRenderer lightFade;
    public float lightSizeMultiplier = 0.8f;
    public float lightBeginopacity = 0.7f;
    public float lightIncreaseTime = 0.2f;

    [Header("Shockwave Animation")]
    public SpriteRenderer shockWave;
    public float shockWaveBeginSize = 0.5f;
    public float shockWaveEndSize = 4;
    public float shockWaveDuration = 0.15f;

    private Sequence tween;
    private float colliderRemainingDuration;

    protected override void Awake()
    {
        base.Awake();
        colliderListener.onTriggerEnter += OnEnemyEnter;
    }

    public void Start()
    {
        if (detonateOnStart)
            Detonate();
    }

    public void Detonate()
    {
        //SFX
        DefaultAudioSources.PlaySFX(explosion_SFX);

        //Enables
        lightFade.enabled = true;
        shockWave.enabled = true;
        coreAnimator.gameObject.SetActive(true);

        Sequence sq = DOTween.Sequence();

        //Grosseurs initiales
        coreAnimator.transform.localScale = Vector2.one * coreSizeMultiplier * coreBeginSize * radius;
        lightFade.transform.localScale = Vector2.one * lightSizeMultiplier * radius;
        shockWave.transform.localScale = Vector2.one * shockWaveBeginSize;
        Color stdColor = shockWave.color;
        shockWave.color = new Color(stdColor.r, stdColor.g, stdColor.b, 1);

        //animations
        sq.Join(coreAnimator.transform.DOScale(coreSizeMultiplier * radius, coreGrowTime).SetEase(coreEase));
        sq.Join(lightFade.DOFade(1, lightIncreaseTime));
        sq.Join(shockWave.transform.DOScale(shockWaveEndSize, shockWaveDuration));
        sq.Join(shockWave.DOFade(0, shockWaveDuration).SetEase(Ease.InSine));
        sq.Append(lightFade.DOFade(0, 1 - lightIncreaseTime).SetEase(Ease.InSine));

        sq.OnComplete(delegate ()
        {
            coreAnimator.SetTrigger("End");
            coreAnimator.gameObject.SetActive(false);
            lightFade.enabled = false;
            coreAnimator.enabled = false;
            if (killOnComplete)
                Die();
        });

        tween = sq;

        //Time scale
        ApplyTimescaleToAnimation();

        //Camera shake
        Game.Instance.gameCamera.vectorShaker.Shake(cameraShake_strength);

        // Damage all units
        if (UseColliderListener)
        {
            colliderListener.GetComponent<Collider2D>().enabled = true;
            colliderRemainingDuration = colliderDuration;
        }
        else
        {
            List<ColliderInfo> infos = UnitDetection.OverlapCircleAll(Position, radius, explosionLayerMask);
            for (int i = 0; i < infos.Count; i++)
            {
                UnitHit(infos[i]);
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (UseColliderListener)
        {
            if(colliderRemainingDuration > 0)
            {
                colliderRemainingDuration -= DeltaTime();
                if(colliderRemainingDuration <= 0)
                {
                    colliderListener.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }
    protected override void Die()
    {
        base.Die();

        Destroy();
    }

    private void OnEnemyEnter(ColliderInfo other, ColliderListener listener)
    {
        UnitHit(other);
    }

    private void UnitHit(ColliderInfo other)
    {
        Unit unit = other.parentUnit;
        if (unit != null && targets.IsValidTarget(unit))
        {
            IAttackable attackable = unit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, this);
            }
        }
    }

    public override float TimeScale
    {
        get { return base.TimeScale; }
        set
        {
            base.TimeScale = value;
            ApplyTimescaleToAnimation();
        }
    }

    private void ApplyTimescaleToAnimation()
    {
        if (tween != null)
            tween.timeScale = TimeScale;
        if (coreAnimator.gameObject.activeInHierarchy)
            coreAnimator.SetFloat("speed", TimeScale);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0.5f, 0.5f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}

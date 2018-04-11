using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CCC._2D;

public class ChainChomp : MovingUnit, IAttackable
{
    [Header("Links")]
    public Rigidbody2D anchor;
    public Rigidbody2D followingBall;
    public Rigidbody2D realBall;
    public ChainChomp_ChainSpawner chainSpawner;
    public SimpleColliderListener colliderListener;
    public GameObject container;
    public SpriteRenderer anchorRenderer;
    public ChainChomp_Sounds sounds;

    [Header("Ball")]
    public int hp = 1;
    public bool isVulnerable = true;
    public SpriteRenderer ballRenderer;

    [Header("Chains")]
    public float distancePerChain = 0.7f;
    public int chainsOnStart = 8;

    [Header("Get Hit")]
    public AudioPlayable GetHitSFX;

    [Header("Attack"), Forward]
    public int hitDamage = 1;
    public Targets targets;
    public AudioPlayable hitSFX;
    public float hitScreenShake;

    [Header("Spawn Animation")]
    public float spawnAnim_sizeMultiplier = 0.25f;
    public float spawnAnim_distanceMargin = 0f;
    public float spawnAnim_duration = 0.5f;
    public Ease spawnAnim_Ease = Ease.OutSine;

    [Header("Break Animation")]
    public AudioPlayable breakSFX;

    [Header("Fade Out Animation")]
    public SpriteGroup spriteGroup;
    public float fadeOut_delay = 0.5f;
    public float fadeOut_duration = 0.35f;

    private bool teleported = false;
    private Transform chainAnchor;
    private PlayerController player;
    private bool isDisapearing = false;
    private Tween fadeAnim;

    protected override void Awake()
    {
        base.Awake();
        rb = realBall;
    }

    void Start()
    {
        Rotation = 0;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //Remove les listeners
        if (player != null)
        {
            player.vehicle.onTeleportPosition -= OnPlayerTeleport;
            player.vehicle.onDestroy -= OnPlayerDeath;
        }
    }

    public void Init(Transform chainAnchor, PlayerController player)
    {
        this.player = player;
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter;
        player.vehicle.onTeleportPosition += OnPlayerTeleport;
        player.vehicle.OnDeath += OnPlayerDeath;
        sounds.SetAnchoredTransform(chainAnchor);

        this.chainAnchor = chainAnchor;
    }

    public void IncreaseLength(float approximateDistance)
    {
        if (IsDead)
            return;
        var chainsToAdd = (approximateDistance / distancePerChain).RoundedToInt().Raised(1);
        chainSpawner.SpawnChains(chainsToAdd);
        UpdateBallDistanceJoint();
    }

    public float GetLength()
    {
        return chainSpawner.ChainCount * distancePerChain;
    }

    public void Spawn()
    {
        //On deplace la balle pres du char
        //On rapetisse la balle
        //On la fait grossir, comme si elle faisait un 'spawn into the world'

        Transform realBallTr = realBall.transform;
        CircleCollider2D ballCollider = realBall.GetComponent<CircleCollider2D>();

        float originalBallRadius = ballCollider.radius * realBallTr.lossyScale.x;
        float originalBallSize = realBallTr.localScale.x;
        float newBallSize = originalBallSize * spawnAnim_sizeMultiplier;
        float newBallRadius = originalBallRadius * spawnAnim_sizeMultiplier;


        //Set Unit Rotation
        tr.rotation = player.transform.rotation;

        //Set ball position
        Vector2 ballInitialPosition = (Vector2)chainAnchor.position - (player.vehicle.WorldDirection2D() * (newBallRadius + spawnAnim_distanceMargin));
        realBallTr.position = ballInitialPosition;

        //Set ball scale
        realBallTr.localScale = newBallSize * Vector3.one;

        //Animate ball back to normalSize
        realBallTr.DOScale(originalBallSize, spawnAnim_duration).SetEase(spawnAnim_Ease);

        //Ajuste la longueur de la realBall
        chainSpawner.SpawnChains(chainsOnStart);
        UpdateBallDistanceJoint();

        //Place anchor and ball
        anchor.transform.position = chainAnchor.position;
        followingBall.transform.position = realBallTr.position;
    }

    public void UpdateBallDistanceJoint()
    {
        realBall.GetComponent<DistanceJoint2D>().distance = distancePerChain * chainSpawner.ChainCount;
    }

    public void BreakFromPlayerAndDisappear() { BreakFromPlayerAndDisappear(null); }
    public void BreakFromPlayerAndDisappear(TweenCallback onComplete)
    {
        if (isDisapearing)
            return;

        //SFX
        DefaultAudioSources.PlaySFX(GetHitSFX);
        DefaultAudioSources.PlaySFX(breakSFX);
        sounds.RattleSound = false;

        DetachAnchor();

        DetachRealBall();

        //Brise les chaines
        var breakCount = (chainSpawner.ChainCount * 0.3f).RoundedToInt().Raised(2);
        var velocity = player != null ? player.vehicle.Speed : (realBall != null ? realBall.velocity : Vector2.zero);
        chainSpawner.BreakOffChains(breakCount, velocity);

        Disappear(onComplete, true);
    }

    public void DetachBallAndDisappear() { DetachBallAndDisappear(null); }
    public void DetachBallAndDisappear(TweenCallback onComplete)
    {
        if (isDisapearing)
            return;

        //SFX
        DefaultAudioSources.PlaySFX(breakSFX);
        sounds.RattleSound = false;

        DetachRealBall();

        //Brise les chaines
        var chainIndex = 2.Capped(chainSpawner.ChainCount - 1);
        chainSpawner.CutChainsAt(chainIndex);

        DisappearBelow(chainIndex + 2, onComplete, true);
    }

    public void DetachRealBall()
    {
        if (realBall == null)
            return;
        realBall.drag = 4;
        realBall.GetComponent<DistanceJoint2D>().enabled = false;
    }

    public void DetachAnchor()
    {
        chainAnchor = null;
        anchor.isKinematic = false;
        anchorRenderer.enabled = false;
    }

    public void Disappear() { Disappear(null, true); }
    public void Disappear(TweenCallback onComplete, bool destroy)
    {
        spriteGroup.GatherChildData();

        KillFadeAnim();
        fadeAnim =spriteGroup.DOFade(0, fadeOut_duration).SetDelay(fadeOut_delay).OnComplete(
            () =>
            {
                if (destroy)
                    Destroy();
                if (onComplete != null)
                    onComplete();
            });

        isDisapearing = true;
    }
    public void DisappearBelow() { DisappearBelow(0, null, false); }
    public void DisappearBelow(int chainIndex, TweenCallback onComplete, bool deactivate)
    {
        if (chainIndex < 0 || isDisapearing)
            return;
        chainIndex = chainIndex.Capped(chainSpawner.ChainCount);

        SpriteRenderer[] renderers = new SpriteRenderer[chainIndex];
        renderers[0] = ballRenderer;

        for (int i = 0; i < chainIndex - 1; i++)
        {
            renderers[i + 1] = chainSpawner.GetChain(i).chainRenderer;
        }

        float alpha = 1;

        KillFadeAnim();
        fadeAnim =  DOTween.To(() => alpha, (x) =>
        {
            alpha = x;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].SetAlpha(alpha);
            }
        }, 0, fadeOut_duration).SetDelay(fadeOut_delay).OnComplete(() =>
        {
            if (deactivate)
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (i == 0)
                    {
                        realBall.gameObject.SetActive(false);
                    }
                    else
                    {
                        chainSpawner.GetChain(i - 1).gameObject.SetActive(false);
                    }
                }
            if (onComplete != null)
                onComplete();
        });
    }

    private void KillFadeAnim()
    {
        if (fadeAnim != null)
            fadeAnim.Kill();
        fadeAnim = null;
    }

    void OnPlayerDeath(Unit unit)
    {
        BreakFromPlayerAndDisappear(null);
    }


    // Damage handling
    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        var unit = other.parentUnit;
        if (targets.IsValidTarget(unit))
        {
            //Bump !
            if (unit is Vehicle)
            {
                Vehicle otherVeh = unit as Vehicle;
                if (otherVeh.rb.bodyType == RigidbodyType2D.Dynamic)
                    (unit as Vehicle).Bump(
                        (otherVeh.Position - realBall.position).normalized * realBall.velocity.magnitude * 1.5f,
                        0,
                        BumpMode.VelocityAdd);
                else
                    (unit as Vehicle).Bump(
                        (otherVeh.Position - realBall.position).normalized * realBall.velocity.magnitude * 1.5f,
                        0.25f,
                        BumpMode.VelocityAdd);
            }

            //Attack
            IAttackable attackable = unit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                //SFX
                DefaultAudioSources.PlaySFX(hitSFX);

                //Screen Shake
                Game.Instance.gameCamera.vectorShaker.Hit(hitScreenShake * (unit.Position - realBall.position).normalized);

                bool wasDead = unit.IsDead;
                Game.Instance.commonVfx.HitWhite(collision.contacts[0].point);
                attackable.Attacked(other, hitDamage * Game.Instance.Player.playerStats.damageMultiplier, this, listener.info);

                if (unit.IsDead && !wasDead)
                {
                    if (Game.Instance.Player != null)
                        Game.Instance.Player.playerStats.RegisterKilledUnit(unit);
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        // Handle post-teleporting
        if (!container.activeSelf)
        {
            if (teleported)
                teleported = false;
            else
                container.SetActive(true);
        }

        // Bouge l'anchor vers la cible (genre la boule du vehicule)
        if (chainAnchor != null)
        {
            anchor.MovePosition(chainAnchor.position);
            
            //On fait pivoter l'anchor
            if (player != null)
                anchor.rotation = player.vehicle.Rotation;
        }



        //On fait pivoter la balle
        //float angle = 0;
        if (realBall != null)
        {
            float strength = 0.05f * realBall.velocity.sqrMagnitude.Clamped(0, 2);
            var firstChain = chainSpawner.GetChain(0);
            if (firstChain != null && chainAnchor != null)
                realBall.rotation = Mathf.LerpAngle(realBall.rotation, firstChain.rb.rotation + 180, FixedLerp.FixedFix(strength));
        }


        if (realBall != null && followingBall != null)
            followingBall.MovePosition(realBall.position);
    }

    void OnPlayerTeleport(Unit player, Vector2 delta)
    {
        container.SetActive(false);
        container.transform.position += new Vector3(delta.x, delta.y);
        teleported = true;
        rb.velocity = Vector3.zero;
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (IsDead)
            return 0;

        DefaultAudioSources.PlaySFX(GetHitSFX);

        if (isVulnerable)
        {
            hp--;
            if (hp == 0)
                Die();
            return hp;
        }
        else
        {
            return 1; //La boule est invulnerable
        }
    }

    protected override void Die()
    {
        if (!IsDead && !isDisapearing)
            DetachBallAndDisappear();

        base.Die();
    }

    public float GetSmashJuiceReward()
    {
        return 0;
    }

    public override float TimeScale
    {
        get
        {
            return base.TimeScale;
        }

        set
        {
            timeScale = value;
            //base.TimeScale = value;

            var count = chainSpawner.ChainCount;
            for (int i = 0; i < count; i++)
            {
                chainSpawner.GetChain(i).TimeScale = value;
            }
        }
    }
}
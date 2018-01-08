using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CCC._2D;

public class ChainChomp : MovingUnit, IAttackable
{
    [Header("Chain Chomp")]
    public Rigidbody2D anchor;
    public Rigidbody2D followingBall;
    public Rigidbody2D realBall;
    public ChainChomp_ChainSpawner chainSpawner;
    public SimpleColliderListener colliderListener;
    public int hitDamage = 1;
    public GameObject container;
    public float distancePerChain = 0.7f;
    public int chainsOnStart = 8;

    [Header("Spawn Animation")]
    public float spawnAnim_sizeMultiplier = 0.25f;
    public float spawnAnim_distanceMargin = 0f;
    public float spawnAnim_duration = 0.5f;
    public Ease spawnAnim_Ease = Ease.OutSine;

    [Header("Fade Out Animation")]
    public SpriteGroup spriteGroup;
    public float fadeOut_delay = 0.5f;
    public float fadeOut_duration = 0.35f;

    private bool teleported = false;
    private Transform chainAnchor;
    private PlayerController player;
    private bool isDisapearing = false;

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
            player.vehicle.onDestroy -= OnPlayerDestroyed;
        }
    }

    public void Init(Transform chainAnchor, PlayerController player)
    {
        this.player = player;
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter;
        player.vehicle.onTeleportPosition += OnPlayerTeleport;
        player.vehicle.onDestroy += OnPlayerDestroyed;

        this.chainAnchor = chainAnchor;
    }

    public void IncreaseLength(float approximateDistance)
    {
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

    public void DetachAndDisapear() { DetachAndDisapear(null); }
    public void DetachAndDisapear(TweenCallback onComplete)
    {
        if (isDisapearing)
            return;

        chainAnchor = null;

        var breakCount = (chainSpawner.ChainCount * 0.3f).RoundedToInt().Raised(2);
        chainSpawner.BreakOffChains(breakCount, realBall.velocity);
        anchor.isKinematic = false;
        realBall.drag = 4;
        realBall.GetComponent<DistanceJoint2D>().enabled = false;

        spriteGroup.GatherChildData();
        spriteGroup.DOFade(0, fadeOut_duration).SetDelay(fadeOut_delay).OnComplete(
            () =>
            {
                Destroy();
                if (onComplete != null)
                    onComplete();
            });
    }

    void OnPlayerDestroyed(Unit unit)
    {
        DetachAndDisapear(null);
    }


    // Damage handling
    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if (other.parentUnit.allegiance == Allegiance.Enemy || other.parentUnit.allegiance == Allegiance.SmashBall)
        {
            //Bump !
            if (other.parentUnit is Vehicle)
            {
                Vehicle otherVeh = other.parentUnit as Vehicle;
                if (otherVeh.rb.bodyType == RigidbodyType2D.Dynamic)
                    (other.parentUnit as Vehicle).Bump(
                        (otherVeh.Position - realBall.position).normalized * realBall.velocity.magnitude * 1.5f,
                        0,
                        BumpMode.VelocityAdd);
                else
                    (other.parentUnit as Vehicle).Bump(
                        (otherVeh.Position - realBall.position).normalized * realBall.velocity.magnitude * 1.5f,
                        0.25f,
                        BumpMode.VelocityAdd);
            }

            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                Game.instance.commonVfx.SmallHit(collision.contacts[0].point, Color.white);
                attackable.Attacked(other, hitDamage * Game.instance.Player.playerStats.damageMultiplier, this, listener.info);
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
            anchor.MovePosition(chainAnchor.position);



        //On fait pivoter la balle
        //float angle = 0;
        float strength = 0.05f * realBall.velocity.sqrMagnitude.Clamped(0, 2);
        var firstChain = chainSpawner.GetChain(0);
        if (firstChain != null && chainAnchor != null)
            realBall.rotation = Mathf.LerpAngle(realBall.rotation, firstChain.rb.rotation + 180, FixedLerp.FixedFix(strength));

        //On fait pivoter l'anchor
        if (player != null)
            anchor.rotation = player.vehicle.Rotation;

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
        return 1; //La boule est invulnerable
    }

    public float GetSmashJuiceReward()
    {
        return 0;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChainChomp : MovingUnit, IAttackable
{
    [Header("Chain Chomp")]
    public Rigidbody2D anchor;
    public Rigidbody2D followingBall;
    public Rigidbody2D realBall;
    public Rigidbody2D fistBallLink;
    public Rigidbody2D lastBallLink;
    public SimpleColliderListener colliderListener;
    public int hitDamage = 1;
    public GameObject container;

    [Header("Spawn Animation")]
    public float spawnAnim_sizeMultiplier = 0.25f;
    public float spawnAnim_distanceMargin = 0f;
    public float spawnAnim_duration = 0.5f;
    public Ease spawnAnim_Ease = Ease.OutSine;

    private bool teleported = false;
    private Transform chainAnchor;
    private PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        rb = realBall;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //Remove les listeners
        if(player != null)
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

    public void Spawn()
    {
        //On deplace la balle pres du char
        //On rapetisse la balle
        //On la fait grossir, comme si elle faisait un 'spawn into the world'

        Transform ballTransform = realBall.transform;
        CircleCollider2D ballCollider = realBall.GetComponent<CircleCollider2D>();

        float originalBallRadius = ballCollider.radius * ballTransform.lossyScale.x;
        float originalBallSize = ballTransform.localScale.x;
        float newBallSize = originalBallSize * spawnAnim_sizeMultiplier;
        float newBallRadius = originalBallRadius * spawnAnim_sizeMultiplier;


        //Set Unit Rotation
        tr.rotation = player.transform.rotation;

        //Set ball position
        Vector2 ballInitialPosition = (Vector2)chainAnchor.position - (player.vehicle.WorldDirection2D() * (newBallRadius + spawnAnim_distanceMargin));
        ballTransform.position = ballInitialPosition;

        //Set ball scale
        ballTransform.localScale = newBallSize * Vector3.one;

        //Animate ball back to normalSize
        ballTransform.DOScale(originalBallSize, spawnAnim_duration).SetEase(spawnAnim_Ease);
    }

    public void DetachAndDisapear()
    {
        //Temporaire
        Destroy();
    }

    void OnPlayerDestroyed(Unit unit)
    {
        DetachAndDisapear();
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
        float angle = 0;
        float strength = 0.05f * Math.Min(realBall.velocity.sqrMagnitude, 2);
        angle = Vehicle.VectorToAngle(realBall.position - fistBallLink.position) + 90;
        realBall.MoveRotation(Mathf.LerpAngle(realBall.rotation, fistBallLink.rotation, FixedLerp.FixedFix(strength)));


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
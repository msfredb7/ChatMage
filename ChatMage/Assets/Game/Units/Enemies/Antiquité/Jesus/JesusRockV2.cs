using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusRockV2 : MovingUnit
{
    public const float PHYSICAL_WIDTH = 1;

    [System.NonSerialized]
    public float flySpeed;

    [Header("Rock")]
    public new Collider2D collider;
    public float onHitShakeStrength = 0.7f;
    [Forward]
    public Targets targets;
    public bool recenterOverTime = false;
    public float recenterSpeed = 0.25f;

    [Header("SFX")]
    public CCC.Utility.RandomAudioCliptList hitSounds;
    public float hitVolume = .9f;

    private bool isFlying = false;
    public bool IsFlying { get { return isFlying; } }
    public Unit InTheHandsOf { get { return inTheHandsOf; } }

    private Unit cannotHit = null;
    private Unit inTheHandsOf;
    private bool hasDestination = false;
    private Vector2 destination;
    private GameObject go;
    private Collider2D[] contacts = new Collider2D[1];

    protected override void Awake()
    {
        base.Awake();
        go = gameObject;
    }

    public void PickedUpState(Unit holder)
    {
        inTheHandsOf = holder;
        go.layer = Layers.NO_COLLISION;
        rb.simulated = false;
        isFlying = false;
        Speed = Vector2.zero;
        collider.enabled = false;

        Vector3 lp = tr.localPosition;
        tr.localPosition = new Vector3(lp.x, lp.y, 0);
    }

    public void ThrownState(Vector2 direction, Unit cannotHit = null)
    {
        hasDestination = false;

        inTheHandsOf = null;
        go.layer = Layers.FLYING_SOLID_ENEMY;
        rb.simulated = true;

        Speed = direction.normalized * flySpeed;

        tr.position += (Vector3)Speed * FixedDeltaTime();
        collider.enabled = true;
        isFlying = true;
        rb.drag = 0;
        this.cannotHit = cannotHit;
    }

    public void ThrownState_Destination(Vector2 destination, Unit cannotHit = null)
    {
        ThrownState(destination - (Vector2)tr.position, cannotHit);
        this.destination = destination;
        hasDestination = true;
    }

    public void StoppedState()
    {
        hasDestination = false;
        inTheHandsOf = null;
        go.layer = Layers.SOLID_ENEMIES;
        rb.simulated = true;
        isFlying = false;
        Speed = Vector2.zero;
        collider.enabled = true;
        rb.drag = 20;
        cannotHit = null;

        SoundManager.PlaySFX(hitSounds.Pick(), volume: hitVolume);

        Game.instance.gameCamera.vectorShaker.Shake(onHitShakeStrength);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderInfo other = collision.collider.GetComponent<ColliderInfo>();

        //On skip, so elle est marquer comme intouchable
        if (other != null && other.parentUnit == cannotHit)
        {
            //cannotHit = null;
            return;
        }

        if (IsFlying)
        {
            //Hit une unit !
            if (other != null)
            {
                Unit unit = other.parentUnit;

                //Est-ce une autre roche ? Si oui, on fait la bump (on la throw en fait)
                if (unit is JesusRockV2)
                {
                    JesusRockV2 rock = unit as JesusRockV2;
                    rock.Speed = Speed;
                    rock.ThrownState(-collision.contacts[0].normal, this);
                }
                else if (targets.IsValidTarget(unit))
                {
                    IAttackable attackable = unit.GetComponent<IAttackable>();
                    if (attackable != null)
                    {
                        attackable.Attacked(other, 1, this, GetComponent<ColliderInfo>());
                    }
                }
            }

            StoppedState();
        }


        cannotHit = null;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isFlying && go.layer == Layers.FLYING_SOLID_ENEMY)
        {
            int contactCount = rb.GetContacts(contacts);
            if (contactCount == 0)
            {
                go.layer = Layers.SOLID_ENEMIES;
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (inTheHandsOf != null && recenterOverTime)
        {
            tr.localPosition = tr.localPosition.MovedTowards(Vector3.zero, DeltaTime() * recenterSpeed);
        }

        if (hasDestination)
        {
            if ((destination - Position).sqrMagnitude < 0.2)
                StoppedState();
        }
    }
}

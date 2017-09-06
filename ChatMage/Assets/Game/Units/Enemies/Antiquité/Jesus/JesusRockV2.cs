using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusRockV2 : MovingUnit
{
    [System.NonSerialized]
    public float flySpeed;

    [Header("Rock")]
    public new Collider2D collider;
    public float onHitShakeStrength = 0.7f;
    [Forward]
    public Targets targets;
    public bool recenterOverTime = false;
    public float recenterSpeed = 0.25f;

    private bool isFlying = false;
    public bool IsFlying { get { return isFlying; } }
    public Unit InTheHandsOf { get { return inTheHandsOf; } }

    private Unit cannotHit = null;
    private Unit inTheHandsOf;

    protected override void Awake()
    {
        base.Awake();
    }

    public void PickedUpState(Unit holder)
    {
        inTheHandsOf = holder;
        gameObject.layer = Layers.NO_COLLISION;
        rb.simulated = false;
        isFlying = false;
        Speed = Vector2.zero;
        collider.enabled = false;
    }

    public void ThrownState(Vector2 direction, Unit cannotHit = null)
    {
        inTheHandsOf = null;
        gameObject.layer = Layers.PROJECTILE;
        rb.simulated = true;
        Speed = direction.normalized * flySpeed;
        tr.position += (Vector3)Speed * FixedDeltaTime();
        collider.enabled = true;
        isFlying = true;
        rb.drag = 0;
        this.cannotHit = cannotHit;
    }

    public void StoppedState()
    {
        inTheHandsOf = null;
        gameObject.layer = Layers.SOLID_ENEMIES;
        rb.simulated = true;
        isFlying = false;
        Speed = Vector2.zero;
        collider.enabled = true;
        rb.drag = 20;

        Game.instance.gameCamera.vectorShaker.Shake(onHitShakeStrength);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderInfo other = collision.collider.GetComponent<ColliderInfo>();

        //On skip, so elle est marquer comme intouchable
        if (other != null && other.parentUnit == cannotHit)
        {
            cannotHit = null;
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

    protected override void Update()
    {
        base.Update();

        if (inTheHandsOf != null && recenterOverTime)
        {
            tr.localPosition = tr.localPosition.MovedTowards(Vector3.zero, DeltaTime() * recenterSpeed);
        }
    }
}

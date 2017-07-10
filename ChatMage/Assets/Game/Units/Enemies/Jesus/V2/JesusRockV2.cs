using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusRockV2 : Unit
{
    [Header("Rock")]
    public float flySpeed;
    public new Collider2D collider;
    public float onHitShakeStrength = 0.7f;

    private bool isFlying = false;
    public bool IsFlying { get { return isFlying; } }

    private Unit cannotHit = null;

    public void PickedUpState()
    {
        rb.simulated = false;
        isFlying = false;
        Speed = Vector2.zero;
        collider.enabled = false;
    }

    public void ThrownState(Vector2 direction, Unit cannotHit = null)
    {
        rb.simulated = true;
        Speed = direction.normalized * flySpeed;
        collider.enabled = true;
        isFlying = true;
        rb.drag = 0;
        this.cannotHit = cannotHit;
    }

    private void StoppedState()
    {
        isFlying = false;
        Speed = Vector2.zero;
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
                    (unit as JesusRockV2).ThrownState(-collision.contacts[0].normal, this);
                }
                else if (IsValidTarget(unit.allegiance))
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
}

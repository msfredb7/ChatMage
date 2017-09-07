using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollVehicle : EnemyVehicle
{
    [Header("Troll")]
    public int hp = 10;
    public float finalTimescale = 2;

    [Header("On Hit")]
    public float invulnurableDuration = 1.5f;
    public float bumpForce = 5;

    [Header("Linking")]
    public new Collider2D collider;
    public TrollAnimatorV2 animator;
    public Transform rockTransporter;

    [Header("Throw")]
    public float throwSpeed;

    [Header("Visuals")]
    public Color flashColor = Color.red;
    public Color lowHPColor = Color.red;
    public SpriteRenderer[] spriteRenderers;

    private bool damagable = true;
    private float timescaleIncrease;
    private int startHP;

    protected override void Awake()
    {
        base.Awake();

        startHP = hp;
        timescaleIncrease = 1 + ((finalTimescale - 1) / (hp - 1));
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0)
            return hp;

        if (!damagable)
            return hp;

        //Decrease hp
        hp -= amount;

        TimeScale *= timescaleIncrease;

        //Flashs animation
        damagable = false;
        UpdateVisuals();
        FlashAnimation.FlashColor(this, spriteRenderers, invulnurableDuration, flashColor, () => damagable = true);

        //Bump unit
        if (unit is Vehicle)
            (unit as Vehicle).Bump((unit.Position - Position).normalized * bumpForce, -1, BumpMode.VelocityAdd);

        if (hp <= 0)
        {
            //Dead !
            if (!isDead)
            {
                if (unit != null)
                {
                    float arrivalAngle = (Position - unit.Position).ToAngle();
                    float deltaRot = arrivalAngle - Rotation;
                    deltaRot = deltaRot.Mod(360);

                    if (deltaRot < 45)
                    {
                        //From back
                        animator.SetDeathDir(3);
                        Debug.Log("from back: " + deltaRot);
                    }
                    else if (deltaRot < 135)
                    {
                        //From right
                        animator.SetDeathDir(2);
                        Debug.Log("from right: " + deltaRot);
                    }
                    else if (deltaRot < 225)
                    {
                        //From front
                        animator.SetDeathDir(0);
                        Debug.Log("from front: " + deltaRot);
                    }
                    else if (deltaRot < 315)
                    {
                        //From left
                        animator.SetDeathDir(1);
                        Debug.Log("from left: " + deltaRot);
                    }
                    else
                    {
                        //From back
                        animator.SetDeathDir(3);
                        Debug.Log("from back: " + deltaRot);
                    }

                    deltaRot = (deltaRot / 90).Rounded() * 90;

                    tr.rotation = Quaternion.Euler(Vector3.forward * (arrivalAngle - deltaRot));
                }

                Die();
            }
        }

        return hp;
    }

    private void UpdateVisuals()
    {
        Color c = Color.Lerp(lowHPColor, Color.white, ((hp - 1f).Raised(0)) / (startHP - 1f));
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = c;
        }
    }

    protected override void Die()
    {
        if (!IsDead)
        {
            canMove.Lock("dead");
            canTurn.Lock("dead");
            GetComponent<AI.EnemyBrainV2>().enabled = false;
            animator.DeathAnimation(Destroy);
        }

        base.Die();

        collider.enabled = false;
    }
}

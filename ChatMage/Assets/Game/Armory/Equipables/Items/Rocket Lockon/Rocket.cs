using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MovingUnit
{
    public SimpleColliderListener listener;
    public float speed;

    private Transform target;

    void Start()
    {
        listener.onTriggerEnter += Listener_onTriggerEnter;
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Unit unit = other.parentUnit;
        if(unit != null && unit is IAttackable)
        {
            IAttackable attackable = unit as IAttackable;

            bool wasDead = unit.IsDead;
            attackable.Attacked(other, 1, this, listener.info);
            if (unit.IsDead && !wasDead && Game.Instance.Player != null)
                Game.Instance.Player.playerStats.RegisterKilledUnit(unit);

        }

        Die();
    }

    public void Init(Transform target)
    {
        this.target = target;
    }

    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            Vector2 v = (Vector2)target.position - Position;
            Speed = v.normalized * speed;
            Rotation = v.ToAngle();
        }
    }

    protected override void Die()
    {
        base.Die();

        //Explosion anim!

        Destroy();
    }
}

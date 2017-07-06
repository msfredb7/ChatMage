using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusVehicle : EnemyVehicle {

    public JesusBrain brain;
    public JesusAnimator animator;
    public MultipleColliderListener colliderListener;

    public int health = 5;
    public float forceToRektPlayer = 5f;

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (amount <= 0)
            return 1;

        if(unit is JesusRock)
            return Damaged();

        return 0;
    }

    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit is PlayerVehicle)
        {
            //(other.parentUnit as PlayerVehicle).Bump((other.parentUnit.Position - Position) * forceToRektPlayer, 1, BumpMode.VelocityAdd);
        }

        if (other.parentUnit is JesusRock)
        {
            if((other.parentUnit as JesusRock).canHit)
            {
                brain.ResetCooldowns();
                Attacked(other, 1, other.parentUnit);
            }            
        }
    }

    void Update()
    {
    }

    private int Damaged()
    {
        Debug.Log("AAWWWWW");
        health--;
        if (health <= 0)
        {
            Die();
            return 0;
        }
        return 1;
    }

    protected override void Die()
    {
        base.Die();

        // ANIMATION DE DESTRUCTIONS DU BOSS

        Destroy(gameObject);
    }

    public void GoToLocation(Vector2 location)
    {
        // ANIMATION DE DÉPLACEMENT
        GotoPosition(location);
    }

    Vector2 GetRandomLocationAroundScreen()
    {
        Vector2 pos = new Vector2(UnityEngine.Random.Range(Game.instance.gameCamera.Right, Game.instance.gameCamera.Left), Position.y);

        return pos;
    }
}

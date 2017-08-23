using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyWallScript : Unit {

    public float awayFromPlayer = 1f;
    public float nearPlayer = 1f;

    public float bumpStrength;
    public float bumpDuration = 0f;

    public SimpleColliderListener colliderListener;
    
    public bool marchinY = true;
    public bool beginMarching = false;

    private bool justHitPlayer;
    private float cooldown;
	
    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
        justHitPlayer = false;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit is PlayerVehicle)
        {
            Game.instance.Player.playerStats.Attacked(other, 1, this);
            if(!justHitPlayer)
                (other.parentUnit as PlayerVehicle).Bump(WorldDirection2D() * bumpStrength, bumpDuration, BumpMode.VelocityAdd);
            justHitPlayer = true;
            cooldown = 2; // Le ArmyWall recommence a marcher apres 2 secondes
        }
        if (other.parentUnit is EnemyVehicle)
            (other.parentUnit as EnemyVehicle).ForceDie();
    }

    protected override void Update ()
    {
        base.Update();
        if(beginMarching)
        {
            if(Game.instance.Player.vehicle != null)
            {
                if (marchinY)
                {
                    if (Vector2.Distance(Game.instance.Player.vehicle.Position, transform.position) > 10 || justHitPlayer)
                        transform.position = new Vector2(transform.position.x, transform.position.y + (Game.instance.Player.vehicle.DeltaTime() * awayFromPlayer)); // si hors du screen
                    else
                        transform.position = new Vector2(transform.position.x, transform.position.y + (Game.instance.Player.vehicle.DeltaTime() * nearPlayer)); // si inside screen
                } else
                {
                    if (Vector2.Distance(Game.instance.Player.vehicle.Position, transform.position) > 10 || justHitPlayer)
                        transform.position = new Vector2(transform.position.x + (Game.instance.Player.vehicle.DeltaTime() * awayFromPlayer), transform.position.y); // si hors du screen
                    else
                        transform.position = new Vector2(transform.position.x + (Game.instance.Player.vehicle.DeltaTime() * nearPlayer), transform.position.y); // si inside screen
                }
            }
        }
        if (justHitPlayer)
            if (cooldown < 0)
                justHitPlayer = false;
        cooldown -= DeltaTime();
    }
}

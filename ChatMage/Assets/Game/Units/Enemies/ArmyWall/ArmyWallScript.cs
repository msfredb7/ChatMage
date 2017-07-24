using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyWallScript : Unit {

    public float awayFromPlayer = 1f;
    public float nearPlayer = 1f;

    public SimpleColliderListener colliderListener;
    
    public bool marchinY = true;
    public bool beginMarching = false;
	
    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if(other.parentUnit is PlayerVehicle)
            (other.parentUnit as PlayerVehicle).Kill();
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
                    if (Vector2.Distance(Game.instance.Player.vehicle.Position, transform.position) > 10)
                        transform.position = new Vector2(transform.position.x, transform.position.y + (Game.instance.Player.vehicle.DeltaTime() * awayFromPlayer)); // si hors du screen
                    else
                        transform.position = new Vector2(transform.position.x, transform.position.y + (Game.instance.Player.vehicle.DeltaTime() * nearPlayer)); // si inside screen
                } else
                {
                    if (Vector2.Distance(Game.instance.Player.vehicle.Position, transform.position) > 10)
                        transform.position = new Vector2(transform.position.x + (Game.instance.Player.vehicle.DeltaTime() * awayFromPlayer), transform.position.y); // si hors du screen
                    else
                        transform.position = new Vector2(transform.position.x + (Game.instance.Player.vehicle.DeltaTime() * nearPlayer), transform.position.y); // si inside screen
                }
            }
        }
    }
}

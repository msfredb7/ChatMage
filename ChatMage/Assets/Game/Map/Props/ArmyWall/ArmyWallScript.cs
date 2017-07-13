using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyWallScript : MonoBehaviour {

    public float awayFromPlayer = 1f;
    public float nearPlayer = 1f;

    public SimpleColliderListener colliderListener;

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

    void Update ()
    {
        if(beginMarching)
        {
            if(Game.instance.Player.vehicle != null)
            {
                if (Vector2.Distance(Game.instance.Player.vehicle.Position, transform.position) > 10)
                    transform.position = new Vector2(transform.position.x, transform.position.y + (Game.instance.Player.vehicle.DeltaTime() * awayFromPlayer)); // si hors du screen
                else
                    transform.position = new Vector2(transform.position.x, transform.position.y + (Game.instance.Player.vehicle.DeltaTime() * nearPlayer)); // si inside screen
            }
        }
    }
}

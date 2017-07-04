using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashShieldObject : MonoBehaviour {
	
	void Update ()
    {
		if(Game.instance != null)
        {
            FollowPlayer();
        }
	}

    private void FollowPlayer()
    {
        // Position
        transform.position = Game.instance.Player.vehicle.transform.position;

        // Rotation
        transform.rotation = Game.instance.Player.vehicle.transform.rotation;
        //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90));
    }
}

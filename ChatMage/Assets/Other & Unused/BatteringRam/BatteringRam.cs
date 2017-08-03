using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteringRam : MonoBehaviour {

    public BatteringRamEffects effects;

    private bool equiped = false;
	
	void Update ()
    {
        if (equiped)
        {
            // When equiped...
            FollowPlayer();
            //  and do something define
            // in BatteringRamEffects
        }
	}

    private void FollowPlayer()
    {
        // Position du Ram
        transform.position = Game.instance.Player.vehicle.transform.position;

        // Rotation du Ram
        Vector3 rot = Game.instance.Player.vehicle.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y, rot.z + 180);
        transform.rotation = Quaternion.Euler(rot);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!equiped)
        {
            Debug.Log("Equip Ram");
            equiped = true;
        }
    }
}

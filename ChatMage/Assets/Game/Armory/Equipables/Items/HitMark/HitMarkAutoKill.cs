using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarkAutoKill : MonoBehaviour {

    public float delay = 1f;

	void Start ()
    {
        this.DelayedCall(delegate ()
        {
            Destroy(gameObject);
        }, delay);
	}
}

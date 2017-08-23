using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSoldiers : MonoBehaviour {

    private Vector3 startingPos;

	void Start () {
        startingPos = transform.localPosition;
    }
	
	void Update ()
    {
        transform.localPosition = startingPos;
    }
}

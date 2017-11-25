using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMapDebugStart : MonoBehaviour {

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GetComponent<RoadMapPoint>() != null)
                GetComponent<RoadMapPoint>().StartRoad(delegate ()
                {
                    print("Road Map Complete");
                });
        }
	}
}

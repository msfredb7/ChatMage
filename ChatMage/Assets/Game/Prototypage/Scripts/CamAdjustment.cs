using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjustment : MonoBehaviour {
    public Camera cam;

    void Start ()
    {
        float ratio = cam.aspect;
        print("aspect: " + ratio);
        transform.position = new Vector3(cam.orthographicSize * ratio, cam.orthographicSize, -100);
	}
}

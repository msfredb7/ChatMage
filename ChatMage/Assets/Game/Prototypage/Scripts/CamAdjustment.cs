using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjustment : MonoBehaviour {
    public Camera cam;

    void Start ()
    {
        transform.position = new Vector3(
            cam.orthographicSize * cam.aspect,  //x
            cam.orthographicSize,               //y
            -100);                              //z
	}
}

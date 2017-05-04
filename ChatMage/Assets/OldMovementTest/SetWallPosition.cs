using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWallPosition : MonoBehaviour {

    public float ratio;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3((Screen.width/ ratio), transform.position.y, transform.position.z);
	}

    void Update()
    {
        transform.position = new Vector3((Screen.width / ratio), transform.position.y, transform.position.z);
    }
}

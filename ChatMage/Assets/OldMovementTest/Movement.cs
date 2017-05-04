using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.position = transform.position + (transform.forward/25);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -2.5f, 0));
        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 2.5f, 0));
        }
    }
}

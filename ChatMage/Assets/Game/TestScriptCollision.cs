using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptCollision : MonoBehaviour {

    public bool doit;

	public void Print(string text) {
        if(doit)
            Debug.Log(text);
    }
}

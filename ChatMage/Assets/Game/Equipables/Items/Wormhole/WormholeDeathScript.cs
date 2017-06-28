using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeDeathScript : MonoBehaviour {

    public Wormhole wormhole;

	public void Kill()
    {
        wormhole.Kill();
    }
}

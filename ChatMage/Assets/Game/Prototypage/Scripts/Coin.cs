using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        transform.position = new Vector3(Random.Range(0.5f, Game.instance.ScreenBounds.x-0.5f), Random.Range(0.5f, Game.instance.ScreenBounds.y - 0.5f), 0);
    }
}

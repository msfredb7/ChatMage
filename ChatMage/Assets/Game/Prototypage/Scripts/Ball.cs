using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    void Update()
    {
        if (transform.position.x < -5 || transform.position.x > Game.instance.ScreenBounds.x + 5)
            Destroy(gameObject);
        else if (transform.position.y < -5 || transform.position.y > Game.instance.ScreenBounds.y + 5)
            Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Coin>() != null) //Coin
        {
            collision.gameObject.GetComponent<Coin>().Respawn();
        }
        Destroy(gameObject);
    }
}

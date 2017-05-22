using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleCollisionListener : MonoBehaviour
{
    public class GameObjectEvent : UnityEvent<GameObject> { }
    public GameObjectEvent onEnter = new GameObjectEvent();
    public GameObjectEvent onExit = new GameObjectEvent();


    public void OnTriggerExit2D(Collider2D other)
    {
        onExit.Invoke(other.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        onEnter.Invoke(other.gameObject);
    }
}

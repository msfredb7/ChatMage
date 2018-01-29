using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCoin : MonoBehaviour
{
    public SimpleColliderListener listener;
    public Action<RedCoin> onDeath;
    public new SpriteRenderer renderer;

    void Awake()
    {
        gameObject.SetActive(false);
        listener.onTriggerEnter += Listener_onTriggerEnter;
    }

    private void Listener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (!gameObject.activeSelf)
            return;

        if(other.parentUnit == Game.Instance.Player.vehicle)
        {
            onDeath(this);
            gameObject.SetActive(false);
        }
    }
}

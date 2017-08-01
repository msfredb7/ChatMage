using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedievalBridge : MonoBehaviour
{
    public SpriteRenderer bridgeCenterVisuals;
    public new Collider2D collider;

    public bool startState;
    private bool state;

    void Start()
    {
        if (startState)
            OpenInstant();
        else
            CloseInstant();
    }

    public void OpenInstant()
    {
        collider.enabled = false;
        //collider.transform.localScale = new Vector3(0, 1, 0);
        bridgeCenterVisuals.enabled = true;
        state = true;
    }

    public void Open()
    {
        OpenInstant();
    }

    public void CloseInstant()
    {
        collider.enabled = true;
        //collider.transform.localScale = Vector3.one;
        bridgeCenterVisuals.enabled = false;
        state = false;
    }

    public void Close()
    {
        CloseInstant();
    }

    public void Toggle()
    {
        if (!state)
            Open();
        else
            Close();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedievalBridge : MonoBehaviour
{
    public new Collider2D collider;
    public Animator controller;

    public bool startState;

    private int openHash = Animator.StringToHash("open");
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
        state = true;
        controller.SetBool(openHash, true);
    }

    public void Open()
    {
        OpenInstant();
    }

    public void CloseInstant()
    {
        collider.enabled = true;
        state = false;
        controller.SetBool(openHash, false);
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

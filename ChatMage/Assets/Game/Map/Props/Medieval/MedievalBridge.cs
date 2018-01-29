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
    private bool timescaleListener = false;
    private float timescale = 1;

    void Start()
    {
        if (startState)
            OpenInstant();
        else
            CloseInstant();
    }

    public void OpenInstant()
    {
        ApplyTimescale();
        collider.enabled = false;
        state = true;
        controller.SetBool(openHash, true);
    }

    public void Open()
    {
        ApplyTimescale();
        OpenInstant();
    }

    public void CloseInstant()
    {
        ApplyTimescale();
        collider.enabled = true;
        state = false;
        controller.SetBool(openHash, false);
    }

    public void Close()
    {
        ApplyTimescale();
        CloseInstant();
    }

    public void Toggle()
    {
        if (!state)
            Open();
        else
            Close();
    }

    void UpdateTimescale(float value)
    {
        timescale = value;
    }

    void ApplyTimescale()
    {
        if (!timescaleListener && Game.Instance != null)
        {
            Game.Instance.worldTimeScale.onSet.AddListener(UpdateTimescale);
            timescaleListener = true;

            UpdateTimescale(Game.Instance.worldTimeScale);
        }

        controller.speed = timescale;
    }

    void OnDestroy()
    {
        if (Game.Instance != null)
            Game.Instance.worldTimeScale.onSet.RemoveListener(UpdateTimescale);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    public UnityEvent onOn;
    public UnityEvent onOff;
    public UnityEvent onToggle;

    [SerializeField]
    private bool state;

    public void Toggle()
    {
        State = !state;
    }

    public void On()
    {
        if (state == true)
            return;

        state = true;
        onOn.Invoke();
        onToggle.Invoke();
    }

    public void Off()
    {
        if (state == false)
            return;

        state = false;
        onOff.Invoke();
        onToggle.Invoke();
    }

    public bool State
    {
        get
        {
            return state;
        }
        set
        {
            if (value)
                On();
            else
                Off();
        }
    }
}

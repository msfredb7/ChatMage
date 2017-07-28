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
        onToggle.Invoke();
    }

    public void On()
    {
        state = true;
        onOn.Invoke();
    }

    public void Off()
    {
        state = false;
        onOff.Invoke();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActivation : MonoBehaviour
{
    public UnityEvent onActivate = new UnityEvent();

    private bool buttonPressed;

    void OnTriggerExit2D(Collider2D other)
    {
        if (buttonPressed)
        {
            buttonPressed = false;
            onActivate.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!buttonPressed)
            buttonPressed = true;
        // Button animation
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivation : MonoBehaviour {

    public IActivator objectToActivate;

    private bool buttonPressed;

    void OnTriggerExit2D(Collider2D other)
    {
        if (buttonPressed)
        {
            buttonPressed = false;
            if (objectToActivate != null)
            {
                objectToActivate.Activate();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!buttonPressed)
            buttonPressed = true;
        // Button animation
    }
}

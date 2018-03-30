using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{
    public static List<WindowController> windowsInFocus = new List<WindowController>();

    public bool CanBeClosedWithEscape = true;
    [SerializeField] Button buttonClickToSimulate;
    [SerializeField] UnityEvent closeAction = new UnityEvent();

    public void CloseWindow()
    {
        if(buttonClickToSimulate != null)
        {
            var clickAnim = buttonClickToSimulate.GetComponent<ClickAnimation>();
            if (clickAnim != null)
                clickAnim.ManualClickAnim();

            var buttonSound = buttonClickToSimulate.GetComponent<ButtonSoundADV>();
            if (buttonSound)
                buttonSound.ManualClick();

            buttonClickToSimulate.onClick.Invoke();
        }
        closeAction.Invoke();
    }

    void OnEnable()
    {
        // Set window in focus
        if (!windowsInFocus.Contains(this))
            windowsInFocus.Add(this);
    }

    void OnDisable()
    {
        windowsInFocus.Remove(this);
    }
}

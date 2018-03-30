using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindowsWithEscape : CCC.Persistence.MonoPersistent
{
    public override void Init(Action onComplete)
    {
        onComplete();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(WindowController.windowsInFocus.Count > 0)
            {
                var window = WindowController.windowsInFocus[0];
                if (window.CanBeClosedWithEscape)
                    window.CloseWindow();
            }
        }
    }
}

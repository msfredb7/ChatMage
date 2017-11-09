using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCDemoMessage : MonoBehaviour
{
    void Start()
    {
        enabled = false;
        CCC.Manager.MasterManager.Sync(() => enabled = true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        }
    }
}

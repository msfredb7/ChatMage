using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class demoLevelScript : LevelScript {

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndLevel();
        }
    }
}

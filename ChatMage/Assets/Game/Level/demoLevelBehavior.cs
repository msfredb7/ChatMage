using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class demoLevelScript : LevelScript
{
    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    public override void OnGameReady()
    {
        
    }

    public override void OnGameStarted()
    {
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOver)
            {
                isOver = true;
                onObjectiveComplete.Invoke();
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GateOpenOnSpawn : OnSpawnAction
{
    public UnityEvent open;
    public UnityEvent close;
    public float openTime = 1f;

    private float timer = 0;
    CCC.Utility.StatFloat worldTimeScale;

    protected override void AttachedSpawn_onUnitSpawned(Unit unit)
    {
        if (unit == null)
            return;

        if (timer <= 0)
            open.Invoke();

        timer = openTime;
    }

    void Update()
    {
        if(timer > 0)
        {
            if(worldTimeScale == null)
            {
                worldTimeScale = Game.Instance != null ? Game.Instance.worldTimeScale : null;
            }
            else
            {
                timer -= worldTimeScale * Time.deltaTime;

                if(timer <= 0)
                {
                    close.Invoke();
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;

public class SM_RisingSun : Smash
{
    public Sun sunPrefab;
    public float sunDuration = 10;

    [NonSerialized, fsIgnore]
    private int sunCount = 0;
    [NonSerialized, fsIgnore]
    private ScreenAdd screenAddEffect;

    public override void OnGameReady()
    {
        screenAddEffect = Game.Instance.gameCamera.cam.gameObject.AddComponent<ScreenAdd>();
        screenAddEffect.enabled = false;
        screenAddEffect.color = Color.black;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnSmash(Action onComplete)
    {
        screenAddEffect.enabled = true;
        Sun newSun = Game.Instance.SpawnUnit(sunPrefab, Game.Instance.gameCamera.Center);

        sunCount++;

        newSun.SetVFX(screenAddEffect);
        newSun.OnDeath += delegate (Unit unit)
        {
            sunCount--;
            if (sunCount <= 0)
            {
                screenAddEffect.enabled = false;
                if (onComplete != null)
                    onComplete();
            }
        };
        newSun.SetDuration(sunDuration);
    }

    public override void OnUpdate()
    {

    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        sunCount = 0;
        screenAddEffect = null;
    }
}

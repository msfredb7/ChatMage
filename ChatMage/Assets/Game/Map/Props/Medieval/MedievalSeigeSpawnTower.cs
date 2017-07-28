using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MedievalSeigeSpawnTower : MonoBehaviour
{
    [NonSerialized]
    public MedievalSeigeSpawn originalSpawn;
    [NonSerialized]
    public bool infiniteSpawn;

    public MedievalSwitch switcher;
    public float spawnDistance = 1.5f;

    [Header("Move")]
    public Ease moveEase;
    public float moveDuration;
    public float moveOvershoot = 1.15f;

    [Header("Arrival")]
    public float screenShake = 0.2f;

    [Header("Bridge")]
    public Transform bridgeTr;
    public float bridgeFinalXScale;
    public float bridgeOpenDuration;
    public Ease bridgeOpenEase = Ease.OutSine;
    public Ease bridgeCloseEase = Ease.InSine;

    [Header("Pause")]
    public float pauseDuration;

    public void SpawnUnits(Unit[] units, float interval)
    {
        ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Length; i++)
            {
                float delay = i * interval;
                originalSpawn.SpawnUnit(units[i], delay);
            }
        });
    }
    public void SpawnUnits(List<Unit> units, float interval)
    {
        ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Count; i++)
            {
                float delay = i * interval;
                originalSpawn.SpawnUnit(units[i], delay);
            }
        });
    }
    public void SpawnUnits(Unit[] units, float interval, Action<Unit> callback)
    {
        ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Length; i++)
            {
                float delay = i * interval;
                originalSpawn.SpawnUnit(units[i], delay, callback);
            }
        });
    }
    public void SpawnUnits(List<Unit> units, float interval, Action<Unit> callback)
    {
        ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Count; i++)
            {
                float delay = i * interval;
                originalSpawn.SpawnUnit(units[i], delay, callback);
            }
        });
    }

    void ArriveAnimation(TweenCallback callback)
    {
        Vector2 destination = originalSpawn.DefaultSpawnPosition();
        float orientation = originalSpawn.DefaultSpawnRotation();
        Vector2 v = orientation.ToVector();

        transform.position = destination - (v * spawnDistance);
        transform.rotation = originalSpawn.transform.rotation;

        Sequence sq = DOTween.Sequence();

        sq.Join(transform.DOMove(destination, moveDuration).SetEase(moveEase, moveOvershoot));

        sq.AppendCallback(() =>
        {
            Game.instance.gameCamera.vectorShaker.Shake(screenShake);
            BridgeOpen();
        });

        sq.AppendInterval(pauseDuration + bridgeOpenDuration);

        sq.OnComplete(delegate ()
        {
            switcher.InstantRestoreToggle();
            callback();
        });
    }

    public void QuitAnimation()
    {
        float orientation = transform.eulerAngles.z;
        Vector2 v = orientation.ToVector();
        Vector2 destination = (Vector2)transform.position - (v * spawnDistance);

        Sequence sq = DOTween.Sequence();

        sq.Join(BridgeClose());

        sq.Append(transform.DOMove(destination, moveDuration).SetEase(moveEase, moveOvershoot));

        sq.OnComplete(delegate()
        {
            sq.Kill();
            Destroy(gameObject);
        });
    }

    void BridgeOpen()
    {
        switcher.Toggle();
        bridgeTr.DOKill();
        bridgeTr.DOScaleX(bridgeFinalXScale, bridgeOpenDuration).SetEase(bridgeOpenEase);
    }

    Tween BridgeClose()
    {
        bridgeTr.DOKill();
        return bridgeTr.DOScaleX(0, bridgeOpenDuration).SetEase(bridgeCloseEase);
    }
}

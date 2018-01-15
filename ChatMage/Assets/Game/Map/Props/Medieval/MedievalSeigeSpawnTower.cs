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
    public bool selfQuit = false;

    public MedievalSwitch switcher;
    public float spawnDistance = 1.5f;

    [Header("Move")]
    public Ease moveEase;
    public float moveDuration;
    public float moveOvershoot = 1.15f;

    [Header("Arrival")]
    public float screenShake = 0.2f;

    [Header("Pause before fate")]
    public float pauseBeforeGate;

    [Header("Bridge")]
    public Transform bridgeTr;
    public float bridgeFinalXScale;
    public float bridgeOpenDuration;
    public Ease bridgeOpenEase = Ease.OutSine;
    public Ease bridgeCloseEase = Ease.InSine;

    [Header("Pause before spawn")]
    public float pauseBeforeSpawn;

    [Header("Quit after wave")]
    public float quitAfter;

    private bool isSetup = false;
    private bool hasArrived = false;
    private Sequence sq = null;
    private CCC.Utility.StatFloat worldTimeScale;
    private float timeToLive = -1;

    void Update()
    {
        if (selfQuit && hasArrived)
        {
            if (worldTimeScale == null)
                worldTimeScale = Game.Instance.worldTimeScale;
            else
            {
                bool wasAlive = timeToLive > 0;
                timeToLive -= worldTimeScale * Time.deltaTime;

                if (timeToLive <= 0 && wasAlive)
                    switcher.GetComponent<Switch>().Off();
            }
        }
    }

    public void StayAliveFor(bool enableSelfQuit, float duration)
    {
        selfQuit = enableSelfQuit;
        timeToLive = Mathf.Max(duration + quitAfter, timeToLive);
    }

    public bool IsSetup { get { return isSetup; } }

    public void ArriveAnimation(TweenCallback callback)
    {
        //Si on est deja la, on fait juste callback
        if (isSetup)
        {
            callback();
            return;
        }

        isSetup = true;

        gameObject.SetActive(true);

        Vector2 destination = originalSpawn.DefaultSpawnPosition();
        float orientation = originalSpawn.DefaultSpawnRotation();
        Vector2 v = orientation.ToVector();

        transform.position = destination - (v * spawnDistance);
        transform.rotation = originalSpawn.transform.rotation;

        if (sq != null)
            sq.Kill();

        sq = DOTween.Sequence();

        sq.Join(transform.DOMove(destination, moveDuration).SetEase(moveEase, moveOvershoot));

        sq.AppendCallback(() =>
        {
            Game.Instance.gameCamera.vectorShaker.Shake(screenShake);
        });

        sq.AppendInterval(pauseBeforeGate);

        sq.AppendCallback(() =>
        {
            BridgeOpen();
        });

        sq.AppendInterval(pauseBeforeSpawn + bridgeOpenDuration);

        sq.OnComplete(delegate ()
        {
            switcher.InstantRestoreToggle();
            callback();
            hasArrived = true;
        });
    }

    public void Quit()
    {
        isSetup = false;
        hasArrived = false;
        originalSpawn.CancelSpawning();

        float orientation = transform.eulerAngles.z;
        Vector2 v = orientation.ToVector();
        Vector2 destination = (Vector2)transform.position - (v * spawnDistance);

        if (sq != null)
            sq.Kill();

        sq = DOTween.Sequence();

        sq.Join(BridgeClose());

        sq.Append(transform.DOMove(destination, moveDuration).SetEase(moveEase, moveOvershoot));

        sq.OnComplete(delegate()
        {
            gameObject.SetActive(false);
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

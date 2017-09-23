using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public abstract class BaseCinematicScene : MonoBehaviour
{
    public const float DOUBLETAP_INTERVAL = 0.35f;

    [Header("Base Settings")]
    public PointerListener screenClicker;
    public UnityEvent playEvent;
    public Text doubleTapTip;

    protected bool skipOnDoubleTap;
    protected double lastTapTime = -1;

    protected bool arrivalComplete = false;
    protected float time;

    protected enum State { Inactive = -1, Entering = 0, Playing = 1, Exiting = 2 }
    protected State state = State.Inactive;

    public virtual void ApplySettings<T>(T settings) where T : BaseCinematicSettings
    {
        skipOnDoubleTap = settings.skipOnDoubleTap;
        Enter();
    }

    public virtual void OnArrivalComplete()
    {
        arrivalComplete = true;
        screenClicker.enabled = skipOnDoubleTap;
        screenClicker.onClick.AddListener(OnTap);
    }

    void OnTap()
    {
        if (skipOnDoubleTap && state == State.Playing)
        {
            if (time - lastTapTime <= DOUBLETAP_INTERVAL)
            {
                Skip();
            }
            else
            {

                doubleTapTip.DOKill();
                doubleTapTip.DOFade(1, 0.25f).OnComplete(() => doubleTapTip.DOFade(0, 0.5f).SetDelay(1.75f));

                lastTapTime = time;
            }
        }
    }

    protected void Enter()
    {
        if (state == State.Inactive)
        {
            state = State.Entering;
            OnEnter();
        }
    }

    protected void Play()
    {
        if (state == State.Entering)
        {
            state = State.Playing;
            OnPlay();
            playEvent.Invoke();
        }
    }

    public void Exit()
    {
        if(state == State.Playing)
        {
            state = State.Exiting;

            doubleTapTip.DOKill();
            doubleTapTip.DOFade(0, 0.5f);

            OnExit();
        }
    }

    protected abstract void OnEnter();
    protected abstract void OnPlay();
    protected abstract void OnExit();

    protected virtual void Skip()
    {
        Exit();
    }

    void Update()
    {
        time += Time.deltaTime;
    }
}

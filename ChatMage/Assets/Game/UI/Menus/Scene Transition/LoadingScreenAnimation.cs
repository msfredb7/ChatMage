using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;

public class LoadingScreenAnimation : MonoBehaviour
{
    public enum State { BeforeIntro, Intro, Waiting, Outro, PostOutro }
    public Image bg;
    public Camera cam;
    public AudioMixerSnapshot normalSnapshot;
    public State GetState() { return state; }
    private State state = State.BeforeIntro;

    void Awake()
    {
        bg.color = LoadingScreen.color.ChangedAlpha(0);
    }

    public void Intro(UnityAction onComplete)
    {
        normalSnapshot.TransitionTo(0.5f);
        state = State.Intro;
        bg.DOFade(1, 1).OnComplete(delegate ()
        {
            state = State.Waiting;
            if (Camera.main != null)
                Camera.main.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            onComplete();
        }).SetUpdate(true);
    }

    public void Outro(UnityAction onComplete)
    {
        state = State.Outro;
        cam.gameObject.SetActive(false);
        bg.DOFade(0, 1).OnComplete(delegate ()
        {
            state = State.PostOutro;
            onComplete();
        }).SetUpdate(true);
    }

    public void OnNewSceneLoaded()
    {
        cam.gameObject.SetActive(false);
    }
}

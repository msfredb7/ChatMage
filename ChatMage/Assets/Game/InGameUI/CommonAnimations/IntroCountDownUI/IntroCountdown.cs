using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class IntroCountdown : WrapAnimation
{
    public SimpleEvent onCountdownOver;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Sequence sq = DOTween.Sequence();
        
        sq.InsertCallback(0, delegate () { GetComponent<Text>().text = "3"; });
        sq.InsertCallback(1, delegate () { GetComponent<Text>().text = "2"; });
        sq.InsertCallback(2, delegate () { GetComponent<Text>().text = "1"; });
        sq.OnComplete(delegate() { End(); });
    }

    public override void End()
    {
        base.End();
        if (onCountdownOver != null)
            onCountdownOver();
        Destroy(gameObject);
    }
}

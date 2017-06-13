using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class IntroCountdown : BaseIntro
{
    public override void Play(Action onComplete)
    {
        Sequence sq = DOTween.Sequence();

        sq.InsertCallback(0, delegate () { GetComponent<Text>().text = "3"; })
            .InsertCallback(1, delegate () { GetComponent<Text>().text = "2"; })
            .InsertCallback(2, delegate () { GetComponent<Text>().text = "1"; })
            .SetUpdate(false)
            .OnComplete(delegate ()
            {
                if (onComplete != null)
                    onComplete();

                Destroy(gameObject);
            });
    }
}

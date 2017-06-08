using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCountdown : MonoBehaviour
{
    public SimpleEvent onCountdownOver;

    void Start()
    {
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "3"; }, 0);
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "2"; }, 1);
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "1"; }, 2);
        DelayManager.CallTo(delegate ()
        {
            if (onCountdownOver != null)
                onCountdownOver();
            Destroy(gameObject);
        }, 3);
    }
}

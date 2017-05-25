using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCountdown : MonoBehaviour {

    public UnityEvent onCountdownOver = new UnityEvent();

    void Start()
    {
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "3"; }, 0);
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "2"; }, 1);
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "1"; }, 2);
        DelayManager.CallTo(delegate () { onCountdownOver.Invoke(); Destroy(gameObject); }, 3);
    }
}

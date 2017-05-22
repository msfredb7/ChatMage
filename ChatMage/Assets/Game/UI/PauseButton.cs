using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    public bool onPause = false;

	public void OnClick()
    {
        if (!onPause)
            Pause();
        else
            UnPause();
    }

    void Pause()
    {
        onPause = true;
        Time.timeScale = 0;
    }

    void UnPause()
    {
        onPause = false;
        Time.timeScale = 1;
    }
}

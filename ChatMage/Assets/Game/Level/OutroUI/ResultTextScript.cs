using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTextScript : MonoBehaviour {

    public Text result;
    public Text score;

	public void UpdateResult(bool win)
    {
        if(win)
            result.text = "YOU WIN";
        else
            result.text = "YOU LOST";
    }

    public void Restart()
    {
        Game.instance.framework.RestartLevel();
    }

    public void GoToMenu()
    {
        Game.instance.Quit();
    }
}

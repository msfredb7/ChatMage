using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RabbitCounter : MonoBehaviour
{
    public Color colorCritical;
    public Color colorOk;
    public Text text;

    private int max = 10;
    private int count = 0;

    public void SetMax(int max)
    {
        this.max = max;

    }

    public void UpdateCount(int count)
    {
        this.count = count;
    }

    private void RefreshDisplay()
    {
        text.text = "Slimes: " + count + "/" + max;
        text.color = Color.Lerp(colorOk, colorCritical, count / max);
    }
}

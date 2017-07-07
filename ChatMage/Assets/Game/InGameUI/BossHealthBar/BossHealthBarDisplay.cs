using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarDisplay : MonoBehaviour {

    public Slider healthBar;
    public Text bossName;

    public void DisplayHealthBar(string bossName)
    {
        healthBar.value = 1;
        AdjustText(bossName);
        gameObject.SetActive(true);
    }

    public void DeactivateHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void AdjustSlider(float value)
    {
        if (value > 1 || value < 0)
            return;
        healthBar.value = value;
    }

    public void AdjustText(string name)
    {
        bossName.text = name;
    }
}

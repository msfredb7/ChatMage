using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarDisplay : MonoBehaviour {

    public Slider healthBar;
    public Text bossName;

    void Awake()
    {
        Hide();
    }

    public void DisplayBoss(string bossName)
    {
        SetSliderValue01(1);
        ChangeBossNameText(bossName);

        gameObject.SetActive(true);
    }

    public void SetSliderValue01(float value)
    {
        value = Mathf.Clamp01(value);
        healthBar.value = value;
    }

    public bool IsVisible { get { return gameObject.activeSelf; } }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeBossNameText(string name)
    {
        bossName.text = name;
    }
}

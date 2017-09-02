using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarDisplay : MonoBehaviour
{

    public Slider healthBar;
    public Text bossName;

    private bool doAwake = true;

    void Awake()
    {
        if (doAwake)
            Hide();
    }

    public void SetSliderValue01(float value)
    {
        value = Mathf.Clamp01(value);
        healthBar.value = value;
    }

    public bool IsVisible { get { return gameObject.activeSelf; } }

    public void Hide()
    {
        doAwake = false;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        doAwake = false;
        gameObject.SetActive(true);
    }

    public void SetBossName(string name)
    {
        bossName.text = name;
    }
}

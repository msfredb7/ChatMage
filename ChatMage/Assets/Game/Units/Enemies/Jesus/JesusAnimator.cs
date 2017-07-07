using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusAnimator : MonoBehaviour
{
    public JesusVehicle vehicle;
    public SpriteRenderer render;
    public float unhitableDuration = 1;

    public void OnHitFlashAnimation()
    {
        vehicle.canBeHit = false;

        FlashAnimation.Flash(vehicle, render, unhitableDuration, delegate ()
          {
              vehicle.canBeHit = true;
          });
    }

    public void DisplayHealthBar()
    {
        Game.instance.ui.bossHealthBar.DisplayHealthBar("Jesus");
    }

    public void UpdateHealthBar(float hp, float hpMax)
    {
        Game.instance.ui.bossHealthBar.AdjustSlider(hp / hpMax);
    }

    public void JesusDeath()
    {
        Game.instance.ui.bossHealthBar.DeactivateHealthBar();
    }
}

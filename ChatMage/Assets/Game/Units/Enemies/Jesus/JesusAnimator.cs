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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorShaker : MonoBehaviour
{
    [Header("Shake")]
    public float speed = 7;
    public float arriveduration = 0;
    public float quitDuration = 0.25f;
    [Header("Hit")]
    public float hitQuitSpeed = 0.1f;

    [NonSerialized]
    private float strength = 0;
    [NonSerialized]
    private float time = 0;
    [NonSerialized]
    private float currentSpeed = 0;
    [NonSerialized]
    private float currentStrength = 0;

    [NonSerialized]
    private bool isShakeOn = false;
    [NonSerialized]
    private float remainingDuration;
    [NonSerialized]
    private Vector2 shakeDelta;

    [NonSerialized]
    private Vector2 hitDelta;

    void Update()
    {
        if (remainingDuration <= 0 && isShakeOn)
        {
            isShakeOn = false;
        }

        //if(hitDelta.sqrMagnitude > 0.01f)
        //{
            hitDelta = Vector2.Lerp(hitDelta, Vector2.zero, FixedLerp.Fix(hitQuitSpeed));
        //}

        if (isShakeOn)
        {
            currentStrength = Mathf.MoveTowards(currentStrength, strength, (strength / arriveduration) * Time.deltaTime);
            currentSpeed = Mathf.MoveTowards(currentSpeed, speed, (speed / arriveduration) * Time.deltaTime);
        }
        else
        {
            currentStrength = Mathf.MoveTowards(currentStrength, 0, (strength / quitDuration) * Time.deltaTime);
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, (speed / quitDuration) * Time.deltaTime);
        }

        if (strength > 0)
        {
            float height = (Mathf.PerlinNoise(time, 0) * 2 - 1) * currentStrength;
            float width = (Mathf.PerlinNoise(0, time) * 2 - 1) * currentStrength;

            shakeDelta = new Vector2(height, width);
        }
        else
        {
            shakeDelta = Vector2.zero;
        }

        remainingDuration -= Time.deltaTime;
        time += Time.deltaTime * currentSpeed;

    }

    public Vector2 CurrentVector { get { return shakeDelta + hitDelta; } }

    public void Shake(float strength = 1, float duration = 0.01f)
    {
        if (!isShakeOn)
            this.strength = strength;
        else
            this.strength = Mathf.Max(this.strength, strength);
        isShakeOn = true;
        remainingDuration = Mathf.Max(remainingDuration, duration);
    }

    public void Hit(Vector2 strength)
    {
        hitDelta += strength;
    }
}

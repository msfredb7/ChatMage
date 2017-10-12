using CCC.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorShaker : MonoBehaviour
{
    [Header("Smoothing"), Range(1, 100)]
    public float smoothFactor;

    [Header("Shake")]
    public bool unscaledTime = false;
    public float timescale = 1;
    public float max = 1.2f;
    public float speed = 7;
    public float quitDuration = 0.25f;

    [Header("Hit")]
    public float hitQuitSpeed = 0.1f;

    [NonSerialized]
    private float time = 0;
    [NonSerialized]
    private float currentSpeed = 0;
    [NonSerialized]
    private float currentStrength = 0;

    [NonSerialized]
    private Vector2 shakeDelta;

    [NonSerialized]
    private Vector2 hitDelta;

    [NonSerialized]
    private Vector2 realSmoothedDelta;

    [NonSerialized]
    private LinkedList<IShaker> shakers = new LinkedList<IShaker>();
    [NonSerialized]
    private LinkedList<TimedShaker> timedShakers = new LinkedList<TimedShaker>();

    void Update()
    {
        float deltaTime = (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * timescale;

        if (hitDelta.sqrMagnitude > 0)
        {
            hitDelta = Vector2.Lerp(hitDelta, Vector2.zero, FixedLerp.Fix(hitQuitSpeed, FPSCounter.GetFPS() / timescale));
        }

        float targetStrength = GetTargetStrength();
        float targetSpeed = targetStrength > 0 ? speed : 0;


        if (quitDuration <= 0)
        {
            currentSpeed = targetSpeed;
            currentStrength = targetStrength;
        }
        else
        {
            float speedMoveSpeed = (speed / quitDuration) * deltaTime;

            float strengthMoveSpeed = float.PositiveInfinity;
            float deltaSpeed = Math.Abs(targetSpeed - currentSpeed);
            if (deltaSpeed > 0)
                strengthMoveSpeed = (Mathf.Abs(currentStrength - targetStrength) /      //Quantite restante
                (deltaSpeed / speedMoveSpeed));                                         //Nombre de frame restante

            currentStrength = Mathf.MoveTowards(currentStrength, targetStrength, strengthMoveSpeed);
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedMoveSpeed);
        }

        if (currentStrength > 0)
        {
            float height = (Mathf.PerlinNoise(time, 0) * 2 - 1) * currentStrength;
            float width = (Mathf.PerlinNoise(0, time) * 2 - 1) * currentStrength;

            shakeDelta = new Vector2(height, width);
        }
        else
        {
            shakeDelta = Vector2.zero;
        }

        if (smoothFactor > 1)
            realSmoothedDelta = Vector2.Lerp(realSmoothedDelta, hitDelta + shakeDelta, FixedLerp.Fix(1 / smoothFactor, FPSCounter.GetFPS() / timescale));
        else
            realSmoothedDelta = hitDelta + shakeDelta;

        time += deltaTime * currentSpeed;
        UpdateTimedShakers(deltaTime);
    }

    private void UpdateTimedShakers(float deltaTime)
    {
        LinkedListNode<TimedShaker> node = timedShakers.First;
        while (node != null)
        {
            LinkedListNode<TimedShaker> next = node.Next;

            if (node.Value.DecreaseTime(deltaTime) <= 0)
            {
                RemoveShaker(node.Value);
                timedShakers.Remove(node);
            }

            node = next;
        }
    }

    private float GetTargetStrength()
    {
        float strength = 0;
        LinkedListNode<IShaker> node = shakers.First;
        while (node != null)
        {
            strength = Mathf.Max(node.Value.GetShakeStrength(), strength);

            node = node.Next;
        }
        return strength;
    }

    public Vector2 CurrentVector { get { return realSmoothedDelta; } }

    public void AddShaker(IShaker shaker)
    {
        shakers.AddLast(shaker);
    }

    public bool RemoveShaker(IShaker shaker)
    {
        return shakers.Remove(shaker);
    }

    public void Shake(float strength = 1.2f)
    {
        //Shake instantan�
        currentStrength = Mathf.Max(currentStrength, strength);
        currentStrength = Mathf.Min(currentStrength, max);
        currentSpeed = speed;
    }

    public void Shake(float strength, float duration)
    {
        TimedShaker timedShaker = new TimedShaker(strength, duration);
        timedShakers.AddLast(timedShaker);
        AddShaker(timedShaker);
        Shake(strength);
    }

    public void Hit(Vector2 strength)
    {
        hitDelta += strength;
    }
}

public interface IShaker
{
    float GetShakeStrength();
}

class TimedShaker : IShaker
{
    private float duration;
    private float strength;
    public TimedShaker(float strength, float duration)
    {
        this.strength = strength;
        this.duration = duration;
    }
    public float GetShakeStrength()
    {
        return duration > 0 ? strength : 0;
    }
    public float DecreaseTime(float amount)
    {
        return duration -= amount;
    }
}
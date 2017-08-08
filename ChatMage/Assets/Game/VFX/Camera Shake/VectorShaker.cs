using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorShaker : MonoBehaviour
{
    [Header("Shake")]
    public float speed = 7;
    public float quitDuration = 0.25f;
    [Header("Hit")]
    public float hitQuitSpeed = 0.1f;

    //[NonSerialized]
    //private float strength = 0;
    [NonSerialized]
    private float time = 0;
    [NonSerialized]
    private float currentSpeed = 0;
    [NonSerialized]
    private float currentStrength = 0;

    //[NonSerialized]
    //private bool isShakeOn = false;
    [NonSerialized]
    private Vector2 shakeDelta;

    [NonSerialized]
    private Vector2 hitDelta;

    [NonSerialized]
    private LinkedList<IShaker> shakers = new LinkedList<IShaker>();
    [NonSerialized]
    private LinkedList<TimedShaker> timedShakers = new LinkedList<TimedShaker>();

    void Update()
    {
        if (hitDelta.sqrMagnitude > 0)
        {
            hitDelta = Vector2.Lerp(hitDelta, Vector2.zero, FixedLerp.Fix(hitQuitSpeed));
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
            float speedMoveSpeed = (speed / quitDuration) * Time.deltaTime;

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

        time += Time.deltaTime * currentSpeed;
        UpdateTimedShakers(Time.deltaTime);
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

    public Vector2 CurrentVector { get { return shakeDelta + hitDelta; } }

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
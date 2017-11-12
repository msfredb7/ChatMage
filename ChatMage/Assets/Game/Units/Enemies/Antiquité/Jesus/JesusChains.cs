using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Math;
using CCC.Utility;

public class JesusChains : MonoBehaviour
{
    public Transform aT;
    public Transform bT;
    public Transform[] chains;

    [Header("Break setting")]
    public float minSpeed = 10;
    public float maxSpeed = 15;
    public float minAngSpeed = 360;
    public float maxAngSpeed = 720;
    public float minDuration = 0.1f;
    public float maxDuration = 0.25f;
    public float brakeStrength = 0.15f;
    public float minBreakAngle;
    public float maxBreakAngle;

    [System.NonSerialized]
    private FlyingChain[] flyingChains;

    [System.NonSerialized]
    private bool broken = false;
    [System.NonSerialized]
    private float timer = 0;
    [System.NonSerialized]
    private Vector2 z = Vector2.zero;
    [System.NonSerialized]
    private float stayEnabledFor = 0.5f;
    private StatFloat worldTimescale;

    private void Update()
    {
        if (broken)
            UpdateBroken();
        else
            UpdateChained();
    }

    void UpdateChained()
    {
        if (chains == null || chains.Length == 0)
            return;

        Vector3 a = aT.position;
        Vector3 b = bT.position;

        Vector3 perChain = (b - a) / chains.Length;
        Vector3 halfChain = perChain / 2;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, perChain);

        for (int i = 0; i < chains.Length; i++)
        {
            chains[i].position = a + (i * perChain) + halfChain;
            chains[i].rotation = rot;
        }

        if (!bT.gameObject.activeSelf)
            Break();
    }

    void UpdateBroken()
    {
        if (flyingChains == null)
            return;

        float timescale = 1;
        if (worldTimescale == null)
        {
            if (Game.instance != null)
                worldTimescale = Game.instance.worldTimeScale;
        }
        else
            timescale = worldTimescale;

        float dt = Time.deltaTime * timescale;
        timer += dt;
        float fixedLerp = FixedLerp.Fix(brakeStrength, FPSCounter.GetFPS() / timescale);
        bool goOn = false;

        for (int i = 0; i < flyingChains.Length; i++)
        {
            flyingChains[i].tr.position += (Vector3)(flyingChains[i].velocity * dt);
            flyingChains[i].tr.Rotate(Vector3.forward, flyingChains[i].angVelocity * dt);

            if(flyingChains[i].duration < timer)
            {
                flyingChains[i].velocity = flyingChains[i].velocity.Lerpped(z, fixedLerp);
                flyingChains[i].angVelocity = flyingChains[i].angVelocity.Lerpped(0, fixedLerp);
            }
            else
            {
                goOn = true;
            }
        }

        if (!goOn)
        {
            if (stayEnabledFor > 0)
                stayEnabledFor -= dt;
            else
                enabled = false;
        }
    }

    private void OnValidate()
    {
        Update();
    }

    public void Break()
    {
        if (broken)
            return;

        broken = true;

        SoundPlayer soundPlayer = GetComponent<SoundPlayer>();
        soundPlayer.PlaySound();

        flyingChains = new FlyingChain[chains.Length];

        for (int i = 0; i < chains.Length; i++)
        {
            flyingChains[i] = new FlyingChain()
            {
                tr = chains[i],
                velocity = Vectors.RandomVector2(minBreakAngle, maxBreakAngle, minSpeed, maxSpeed),
                angVelocity = Random.Range(minAngSpeed, maxAngSpeed) * (Random.value > 0.5f ? 1 : -1),
                duration = Random.Range(minDuration, maxDuration)
            };
        }
    }

    private struct FlyingChain
    {
        public Transform tr;
        public Vector2 velocity;
        public float angVelocity;
        public float duration;

    }
}

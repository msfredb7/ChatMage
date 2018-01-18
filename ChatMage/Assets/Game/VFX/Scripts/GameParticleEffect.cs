using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParticleEffect : InGameTimescaleListener
{
    [SerializeField, Range(0, 1)]
    private float TimescaleInfluence = 1f;

    [SerializeField] private float AnimationDuration = 1;
    [SerializeField] private bool DeactivateOnAwake = true;


    float DeactivationTimer;
    float MyCurrentTimescale = 1;

    List<ParticleSystem> PartSys;
    Transform Tr;
    GameObject Go;

    void Awake()
    {
        PartSys = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
        Go = gameObject;
        Tr = transform;
        if (DeactivateOnAwake)
            Deactivate();
    }

    void Start()
    {
        AddTimescaleListener();
    }

    public void MoveTo(Vector2 position)
    {
        Tr.position = position;
    }

    public void RotateTo(float angle)
    {
        Tr.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    public void Activate()
    {
        Go.SetActive(true);
        DeactivationTimer = AnimationDuration;
    }

    public void Deactivate()
    {
        Go.SetActive(false);
    }

    public bool IsPlaying
    {
        get { return Go.activeSelf; }
    }

    void Update()
    {
        if (IsPlaying)
        {
            DeactivationTimer -= MyCurrentTimescale * Time.deltaTime;
            if (DeactivationTimer <= 0)
                Deactivate();
        }
    }

    protected override void UpdateTimescale(float worldTimescale)
    {
        float myActualTimescale = ConvertToMyTimescale(worldTimescale);

        float mult = myActualTimescale / MyCurrentTimescale;

        MyCurrentTimescale = myActualTimescale;

        for (int i = 0; i < PartSys.Count; i++)
        {
            ModifyParticleSystemModules(PartSys[i], mult);
        }
    }

    private float ConvertToMyTimescale(float otherTimescale)
    {
        return Mathf.Lerp(1, otherTimescale, TimescaleInfluence);
    }

    private void ModifyParticleSystemModules(ParticleSystem system, float timescaleMultiplier)
    {
        var main = system.main;
        main.simulationSpeed *= timescaleMultiplier;


        //var rate = system.emission;
        //rate.rateOverDistanceMultiplier /= timescaleMultiplier;

        //var velocity = system.inheritVelocity;
        //velocity.curveMultiplier /= timescaleMultiplier;
    }
}

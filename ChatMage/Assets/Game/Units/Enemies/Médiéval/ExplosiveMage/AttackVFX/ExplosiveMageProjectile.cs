using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosiveMageProjectile : InGameTimescaleListener
{
    public Unit explosionPrefab;
    public Transform trail;

    public Transform core;
    public SpriteRenderer arriveShockwave;
    public SpriteFlicker coreFlicker;

    [Header("Animation")]
    public AudioPlayable arriveSFX;
    public AudioPlayable startSFX;
    public float coreSizeIncrease = 1.5f;
    public Ease coreSizeEase = Ease.OutSine;
    public float detonationDelay = 0.75f;
    public float arriveShockwaveDuration = 0.25f;
    public Ease arriveShockwaveEase = Ease.Linear;

    private Vector3 shockWaveEndScale;

    private float wasTimescale = 1;
    private float timeRequired = float.PositiveInfinity;
    private float elapsedTime = 0;
    private bool travelling = false;
    private Tween predetonateAnim;
    private Vector2 destination;

    void Awake()
    {
        var arriveShockwaveTR = arriveShockwave.transform;
        shockWaveEndScale = arriveShockwaveTR.localScale;
        arriveShockwaveTR.localScale = Vector3.zero;
    }

    public void GoTo(Vector2 destination)
    {
        if (startSFX != null)
            DefaultAudioSources.PlaySFX(startSFX);

        this.destination = destination;
        var v = destination - (Vector2)trail.position;
        var farBeyond = (Vector2)trail.position + v.normalized * 100;
        var traveler = trail.GetComponent<Traveler>();
        //traveler.TravelSpeed = destination.magnitude / Time.deltaTime;
        traveler.TravelTo(farBeyond);

        timeRequired = v.magnitude / traveler.TravelSpeed;
        elapsedTime = 0;
        travelling = true;

        AddTimescaleListener();
    }

    void LateUpdate()
    {
        if (travelling)
        {
            core.position = trail.position;
        }

        if (elapsedTime < timeRequired)
        {
            elapsedTime += wasTimescale * Time.deltaTime;
            if (elapsedTime >= timeRequired)
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        if (arriveSFX != null)
            DefaultAudioSources.PlaySFX(arriveSFX);

        core.position = destination;
        travelling = false;

        Sequence sq = DOTween.Sequence().OnComplete(Detonate);
        sq.Join(core.DOScale(coreSizeIncrease * core.localScale.x, detonationDelay)
            .SetEase(coreSizeEase));
        float realArriveShockwaveDuration = Mathf.Min(arriveShockwaveDuration, detonationDelay);
        sq.Join(arriveShockwave.transform.DOScale(shockWaveEndScale, realArriveShockwaveDuration).SetEase(arriveShockwaveEase));
        sq.Join(arriveShockwave.DOFade(0, realArriveShockwaveDuration));

        predetonateAnim = sq;

        coreFlicker.range = new Vector2(-2f, 5f);
        trail.GetComponent<ExplosiveMageTrailCutOff>().CutOff(core.position);

        UpdateTimescale();
    }

    void Detonate()
    {
        Game.Instance.SpawnUnit(explosionPrefab, core.position);
        Destroy(gameObject);
    }

    protected override void UpdateTimescale(float worldTimescale)
    {
        var change = worldTimescale / wasTimescale;

        trail.GetComponent<MaterialAnimator>().offsetSpeed *= change;
        trail.GetComponent<Traveler>().TravelSpeed *= change;
        coreFlicker.speed *= change;
        trail.GetComponent<TrailRenderer>().time /= change;

        if (predetonateAnim != null)
            predetonateAnim.timeScale = worldTimescale;

        wasTimescale = worldTimescale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using CCC.DesignPattern;

public class Marker : Pool<Marker>.PoolItem
{
    [SerializeField, ReadOnlyInPlayMode]
    private Transform lookAtTarget;
    [ReadOnlyInPlayMode]
    public SpriteRenderer[] renderers;
    public bool independantUpdate;

    [Header("Show animation")]
    public float preShow_Size = 0.25f;
    public float show_Size = 1;
    public Ease show_Ease = Ease.OutElastic;
    public float show_duration = 0.4f;

    [Header("Hide animation")]
    public float hide_Size = 2;
    public Ease hide_Ease = Ease.OutSine;
    public float hide_duration = 0.25f;

    private Transform myTr;
    private Sequence sq;
    private float[] alphas;
    private Transform followTarget;

    void Awake()
    {
        myTr = GetComponent<Transform>();
        CheckSetAlphas();
    }

    void Update()
    {
        UpdatePositionAndRotation();
    }

    void UpdatePositionAndRotation()
    {
        //Position
        if (followTarget != null)
        {
            myTr.position = followTarget.position;
        }

        //Rotation
        if (lookAtTarget != null)
        {
            Vector2 meToTarget = lookAtTarget.position - myTr.position;
            float angle = meToTarget.ToAngle();
            myTr.rotation = (Vector3.forward * angle).ToEulerRotation();
        }
    }

    public void SetLookAtTarget(MarkerReceiver receiver)
    {
        receiver.onHide += Remove;
        SetLookAtTarget(receiver.transform);
    }

    public void SetLookAtTarget(Transform tr)
    {
        lookAtTarget = tr;
    }

    public void DeployOn(Unit unit)
    {
        Transform theTr = null;
        if (unit is EnemyVehicle)
            theTr = ((EnemyVehicle)unit).bodyCenter;
        else
            theTr = unit.transform;

        DeployOn(theTr);
        unit.OnDeath += Remove;
    }

    public void DeployOn(Transform tr)
    {
        followTarget = tr;
        UpdatePositionAndRotation();
        Show();
    }

    void Show()
    {
        PreAnimate();

        myTr.localScale = Vector3.one * preShow_Size;
        sq.Join(myTr.DOScale(show_Size, show_duration).SetEase(show_Ease));

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = renderers[i].color.ChangedAlpha(0);

            sq.Join(renderers[i].DOFade(alphas[i], show_duration));
        }
    }

    public void Remove()
    {
        ClearTargets();

        PreAnimate();

        sq.Join(myTr.DOScale(hide_Size, hide_duration).SetEase(hide_Ease));

        for (int i = 0; i < renderers.Length; i++)
        {
            sq.Join(renderers[i].DOFade(0, hide_duration));
        }

        sq.OnComplete(PutBackIntoPool);
    }

    private void Remove(Unit u)
    {
        Remove();
    }

    void ClearTargets()
    {
        if (followTarget != null)
        {
            Unit u = followTarget.GetComponent<Unit>();
            if (u != null)
            {
                u.OnDeath -= Remove;
            }
        }
        followTarget = null;

        if(lookAtTarget != null)
        {
            MarkerReceiver mr = lookAtTarget.GetComponent<MarkerReceiver>();
            if(mr != null)
            {
                mr.onHide -= Remove;
            }
        }
        lookAtTarget = null;
    }

    void CheckSetAlphas()
    {
        if (alphas == null)
        {
            alphas = new float[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                alphas[i] = renderers[i].color.a;
            }
        }
    }

    void PreAnimate()
    {
        if (sq != null)
        {
            sq.Kill();
        }
        sq = DOTween.Sequence();
        sq.SetUpdate(independantUpdate);
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

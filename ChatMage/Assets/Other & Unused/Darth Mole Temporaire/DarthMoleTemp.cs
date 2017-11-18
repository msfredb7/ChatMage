using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DarthMoleTemp : MonoBehaviour
{
    public Vector2 onPlayerPos;
    public Vector3 onPlayerRotation;
    public Vector3 onPlayerScale;

    public Targets targets;
    public SimpleColliderListener colliderListenerRIGHT;
    public SimpleColliderListener colliderListenerLEFT;
    public Transform leftSword;
    public Transform rightSword;
    public float destinedSize;
    public float openDuration;
    public Ease openEase;

    void Awake()
    {
        colliderListenerRIGHT.info.parentUnit = Game.instance.Player.vehicle;
        colliderListenerLEFT.info.parentUnit = Game.instance.Player.vehicle;

        colliderListenerRIGHT.onTriggerEnter += ColliderListener_onTriggerEnter;
        colliderListenerLEFT.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    void Start()
    {
        Transform tr = transform;
        tr.localScale = onPlayerScale;
        tr.localPosition = onPlayerPos;
        tr.localRotation = Quaternion.Euler(onPlayerRotation);

        leftSword.transform.localScale = new Vector3(0, 1, 1);
        rightSword.transform.localScale = new Vector3(0, 1, 1);
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Unit u = other.parentUnit;
        if(u != null && u is IAttackable && targets.IsValidTarget(u))
        {
            (u as IAttackable).Attacked(other, 1, Game.instance.Player.vehicle, listener.info);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            Open();
    }

    public void Open()
    {
        leftSword.DOScaleX(destinedSize, openDuration).SetEase(openEase);
        rightSword.DOScaleX(destinedSize, openDuration).SetEase(openEase);
    }
}

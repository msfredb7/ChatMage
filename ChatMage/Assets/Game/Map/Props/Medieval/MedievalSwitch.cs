using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Switch))]
public class MedievalSwitch : MonoBehaviour
{
    public float toggleInterval = 0.5f;
    public Transform visuals;
    public float toggleRotation = 90;
    public SimpleColliderListener listener;

    [Header("Animation")]
    public float duration = 0.2f;
    public Ease rotateEase = Ease.OutBack;
    public float overshoot;

    private Switch switcher;
    private bool canToggle = true;
    private bool rotState = false;

    void Awake()
    {
        switcher = GetComponent<Switch>();
        listener.onCollisionEnter += Listener_onCollisionEnter;
        switcher.onToggle.AddListener(OnSwitchToggle);
    }

    void OnSwitchToggle()
    {
        rotState = !rotState;
        Vector3 endValue = Vector3.forward * (rotState ? toggleRotation : -toggleRotation);
        visuals.DORotate(endValue, duration, RotateMode.LocalAxisAdd).SetEase(rotateEase, overshoot);
    }

    private void Listener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if (other.parentUnit is PlayerVehicle && canToggle)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        listener.GetComponent<Collider2D>().enabled = false;
        canToggle = false;
        StopAllCoroutines();
        StartCoroutine(RestoreToggle());

        switcher.Toggle();
    }

    public void Off()
    {
        if (switcher.State != false)
            Toggle();
    }

    public void On()
    {
        if (switcher.State != true)
            Toggle();
    }

    public void InstantRestoreToggle()
    {
        StopAllCoroutines();
        canToggle = true;
        listener.GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator RestoreToggle()
    {
        yield return new WaitForSeconds(toggleInterval);
        InstantRestoreToggle();
    }
}

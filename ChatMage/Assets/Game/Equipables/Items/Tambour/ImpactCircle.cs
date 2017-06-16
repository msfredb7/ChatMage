using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ImpactCircle : MonoBehaviour {

    public float endScaleValue;
    public GameObject visual;
    public float enemyBumpForce = 1;
    public float bumpDuration = 1;
    public float impactDuration = 1;
    public SimpleColliderListener colliderListener;

    private float timer;

    void Start()
    {
        timer = impactDuration;
        colliderListener.info.parentUnit = Game.instance.Player.vehicle;
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Debug.Log("Impact Collision");
        if(other.parentUnit is EnemyVehicle)
            (other.parentUnit as EnemyVehicle).Bump((other.transform.position - Game.instance.Player.transform.position).normalized * enemyBumpForce, bumpDuration, BumpMode.VelocityAdd);
    }

    void Update ()
    {
        transform.position = Game.instance.Player.vehicle.Position;
        visual.transform.DOScale(endScaleValue, impactDuration);
        if (timer < 0)
            Destroy(gameObject);
        timer -= Time.deltaTime;
	}
}

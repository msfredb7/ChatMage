using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShellScript : Vehicle
{
    public SimpleEvent onHit;

    public SimpleColliderListener colliderListener;

    private float loopIntensity = 1;
    private float noiseSpeed = 1;
    private float wanderCooldown = 2;

    private float speed;
    private GameObject target = null;
    private float counter;

    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
        TeleportPosition(Game.instance.Player.vehicle.Position);
    }

    void Init()
    {
        MoveSpeed = speed;
        counter = wanderCooldown;
    }

    void Update()
    {
        if (counter < 0)
        {
            target = FindTarget(transform.position);
            if (target != null)
                targetDirection = VectorToAngle(target.transform.position - transform.position);
            else
                targetDirection = Mathf.PerlinNoise(Time.time * noiseSpeed / loopIntensity, 70) * (360 * loopIntensity);
        }
        else
            targetDirection = Mathf.PerlinNoise(Time.time * noiseSpeed / loopIntensity, 70) * (360 * loopIntensity);
        counter -= Time.deltaTime;
    }

    public void SetValues(float speed, float noiseSpeed, float wonderCooldown, float loopIntensity)
    {
        this.speed = speed;
        this.noiseSpeed = noiseSpeed;
        this.wanderCooldown = wonderCooldown;
        this.loopIntensity = loopIntensity;
        Init();
    }

    GameObject FindTarget(Vector3 pos)
    {
        List<Unit> units = Game.instance.units;
        GameObject closestUnit = null;
        if (units.Count < 1)
            return closestUnit;
        float previousDistance = 0;
        for (int i = 0; i < units.Count; i++)
        {
            float currentDistance = Vector3.Distance(units[i].gameObject.transform.position, pos);
            if (closestUnit == null || currentDistance < previousDistance)
            {
                if (units[i] != Game.instance.Player.vehicle)
                    closestUnit = units[i].gameObject;
                previousDistance = currentDistance;
            }
        }
        return closestUnit;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit.GetComponent<IAttackable>() != null && // Si on peut l'attaquer
            other.parentUnit.GetComponent<Unit>().allegiance != Allegiance.Ally) // Si cest pas un ally
            other.parentUnit.GetComponent<IAttackable>().Attacked(other, 1, this); // attaque le
        Explode();
    }

    private void Explode()
    {
        // TODO
        Debug.Log("BAM");
        Destroy(gameObject);
        onHit.Invoke();
    }
}

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
    private GameObject target;
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
        // Si on utilise la cible qui avait été trouvé, mais on pourrait refaire une recherche!
        if(counter < 0)
            if(target != null)
                targetDirection = VectorToAngle(target.transform.localPosition - transform.localPosition);
            else
            {
                // On essaie de trouver une nouvelle cible autre que le joueur
                // on ne veut pas vraiment reparcourir la liste des units ici (trop lourd?)
                Unit newTarget = Game.instance.units[(int)Random.Range(0, Game.instance.units.Count - 1)]; // essaie 1
                if(newTarget == Game.instance.Player.vehicle)
                {
                    newTarget = Game.instance.units[(int)Random.Range(0, Game.instance.units.Count - 1)];  // essaie 2
                    if (newTarget == Game.instance.Player.vehicle)
                        Explode();                                                                         // Fuck off
                }
                targetDirection = VectorToAngle(newTarget.transform.localPosition - transform.localPosition);
            }
        else
            targetDirection = Mathf.PerlinNoise(Time.time * noiseSpeed / loopIntensity, 70) * (360 * loopIntensity);
        counter -= Time.deltaTime;
    }

    public void SetValues(GameObject target, float speed, float noiseSpeed, float wonderCooldown, float loopIntensity)
    {
        this.target = target;
        this.speed = speed;
        this.noiseSpeed = noiseSpeed;
        this.wanderCooldown = wonderCooldown;
        this.loopIntensity = loopIntensity;
        Init();
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
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

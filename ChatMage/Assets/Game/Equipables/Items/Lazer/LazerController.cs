using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;

public class LazerController : Unit
{
    [InspectorHeader("Linking")]
    public GameObject mainLazer;
    public GameObject mainLazerCollider;
    
    private float animationDuration;

    [HideInInspector]
    public SimpleEvent onComplete;

    void Start()
    {
        // Le collider n'est pas la au début
        mainLazerCollider.SetActive(false);
        // mais on devra recevoir les évennement quand il sera la
        mainLazerCollider.GetComponent<SimpleColliderListener>().onTriggerEnter += LazerController_onTriggerEnter;

        // Le sprite doit être invisible au début
        Color originalColor = mainLazer.GetComponentInChildren<SpriteRenderer>().color;
        mainLazer.GetComponentInChildren<SpriteRenderer>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    void Update()
    {
        // Le lazer doit être orienter en fonction de la direction du véhicule
        FollowPlayer();
    }

    // Le temps de l'animation est décidé par l'item
    public void SetAnimationDuration(float animationDuration)
    {
        this.animationDuration = animationDuration;
    }

    public void ShootLazer()
    {
        // Le collider apparait se qui active les évennements
        mainLazerCollider.SetActive(true);
        // On débute l'animation du lazer ! Il doit apparaitre en fade in
        mainLazer.GetComponentInChildren<SpriteRenderer>().DOFade(1, animationDuration).OnComplete(delegate ()
        {
            // On finalise l'animation du lazer ! Il doit disparaitre en fade out
            mainLazer.GetComponentInChildren<SpriteRenderer>().DOFade(0, animationDuration).OnComplete(delegate () {
                // Activation l'évennement de complétion car on a fini
                onComplete.Invoke();
                // On supprime le lazer qui a fini sa job
                Destroy(gameObject);
            });
        });
    }

    // Quand on enemie est toucher par le lazer
    private void LazerController_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        // Si c'est pas un enemie
        if (other.parentUnit.allegiance != Allegiance.Ally)
        {
            // On le tue
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, this);
            }
        }
    }

    private void FollowPlayer()
    {
        // Position du Ram
        transform.position = Game.instance.Player.vehicle.transform.position;

        // Rotation du Ram
        transform.rotation = Game.instance.Player.vehicle.transform.rotation;
        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90));
    }
}

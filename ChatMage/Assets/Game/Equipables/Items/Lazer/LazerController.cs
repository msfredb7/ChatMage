using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;

public class LazerController : Unit {

    [InspectorHeader("Linking")]
    public GameObject leftLazer;
    public GameObject rightLazer;
    public GameObject mainLazer;
    public SimpleColliderListener mainLazerCollider;

    [InspectorHeader("Duration"),InspectorTooltip("Shoot a unit will take this amount of time * 2")]
    public float animationDuration;

    [HideInInspector]
    public SimpleEvent onComplete;

    private bool stopAimingLazer = false;
    private bool justKilled = false;

	void Start ()
    {
        mainLazerCollider.onTriggerEnter += LazerController_onTriggerEnter;

        Color originalColor = mainLazer.GetComponentInChildren<SpriteRenderer>().color;
        mainLazer.GetComponentInChildren<SpriteRenderer>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        mainLazer.SetActive(false);

        justKilled = false;

        Sequence sq = DOTween.Sequence();
        sq.Append(leftLazer.transform.DOLocalRotate(new Vector3(0,0,-25), animationDuration));
        sq.Join(rightLazer.transform.DOLocalRotate(new Vector3(0,0,25), animationDuration));
        sq.OnComplete(ShootLazer);
    }
	
	void Update ()
    {
        FollowPlayer();

        if (!stopAimingLazer)
        {
            // TODO: flicker the lazers a la Halo
        } else
        {
            leftLazer.SetActive(false);
            rightLazer.SetActive(false);
        }
    }
    
    void ShootLazer()
    {
        stopAimingLazer = true;
        mainLazer.SetActive(true);
        Tweener anim = mainLazer.GetComponentInChildren<SpriteRenderer>().DOFade(1, animationDuration);
        // TODO: Ajouter des particules effects
        anim.OnComplete(delegate ()
        {
            mainLazer.GetComponentInChildren<SpriteRenderer>().DOFade(0, animationDuration).OnComplete(delegate() {
                mainLazer.SetActive(false);
                onComplete.Invoke();
            });
        });
    }

    private void LazerController_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit.allegiance != Allegiance.Ally)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, this);
                justKilled = true;
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

    public void Delete()
    {
        if (justKilled)
            onComplete += delegate () { Destroy(gameObject); };
        else
            Destroy(gameObject);
    }
}

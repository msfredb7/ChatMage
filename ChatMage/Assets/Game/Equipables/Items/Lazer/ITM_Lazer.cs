using DG.Tweening;
using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Lazer : Item
{
    public LazerController lazerPrefab;
    private LazerController lazer;

    private GameObject lazerTargetZone;
    private GameObject zone;

    public GameObject countainerPrefab;
    private GameObject countainer;

    public GameObject leftLazerPrefab;
    private GameObject leftLazer;

    public GameObject rightLazerPrefab;
    private GameObject rightLazer;

    [InspectorHeader("Duration"), InspectorTooltip("Shoot a unit will take this amount of time * 2")]
    public float animationDuration;

    private bool lazerAlreadyShoot = false;

    private ColliderInfo currentTarget;

    private Sequence lazerAnim;

    public override void OnGameReady()
    {
        // Nous n'avons pas encore shooter
        lazerAlreadyShoot = false;

        // Création de la claymore
        countainer = Instantiate(countainerPrefab, Game.instance.Player.vehicle.transform);
        leftLazer = Instantiate(leftLazerPrefab, countainer.transform);
        rightLazer = Instantiate(rightLazerPrefab, countainer.transform);

        // Claymore doit suivre la rotation du joueur
        FollowPlayer(countainer);
    }

    public override void OnGameStarted()
    {
        // Creation de la zone du lazer
        zone = Instantiate(lazerTargetZone, Game.instance.Player.vehicle.transform);

        // Evennement de sortie et d'entrée dans la zone d'effet du lazer
        zone.GetComponent<SimpleColliderListener>().onTriggerEnter += ITM_Lazer_onTriggerEnter;
        zone.GetComponent<SimpleColliderListener>().onTriggerExit += ITM_Lazer_onTriggerExit; ;
    }

    public override void OnUpdate()
    {
        // Ajustement des rotations
        FollowPlayer(zone);
        FollowPlayer(countainer);
    }

    private void ITM_Lazer_onTriggerExit(ColliderInfo other, ColliderListener listener)
    {
        // Si l'unité qui s'en va est celle qu'on shootait
        if(other == currentTarget)
        {
            // et si elle a pas été tué
            if(other.gameObject != null)
            {
                // et qu'on a fait un lazer
                if (lazer != null)
                {
                    // On doit annuler le lazer

                    // Delete le lazer
                    Destroy(lazer.gameObject);

                    // On ne shoot pu
                    lazerAlreadyShoot = false;

                    // Animation des claymores
                    if (lazerAnim.IsPlaying())
                        lazerAnim.Kill();
                    lazerAnim = DOTween.Sequence();
                    lazerAnim.Append(leftLazer.transform.DOLocalRotate(new Vector3(0, 0, 0), animationDuration));
                    lazerAnim.Join(rightLazer.transform.DOLocalRotate(new Vector3(0, 0, 0), animationDuration));
                }
            }
        }
    }

    private void ITM_Lazer_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        // Si on a pas créer de lazer et qu'on a pas shooter
        if (lazer == null && !lazerAlreadyShoot)
        {
            // Notre nouvelle cible
            currentTarget = other;

            // recevra un lazer
            lazer = Instantiate(lazerPrefab, Game.instance.Player.vehicle.transform);
            // qui durera un certain temps
            lazer.SetAnimationDuration(animationDuration);

            // après que l'animation des claymores soit faites
            lazerAnim = DOTween.Sequence();
            lazerAnim.Append(leftLazer.transform.DOLocalRotate(new Vector3(0, 0, -25), animationDuration));
            lazerAnim.Join(rightLazer.transform.DOLocalRotate(new Vector3(0, 0, 25), animationDuration));
            lazerAnim.OnComplete(delegate() {
                // On lance le lazer
                lazerAlreadyShoot = true;
                lazer.ShootLazer();
            });

            // et quand le lazer aura terminé de tuer tout le monde
            lazer.onComplete += delegate () {
                // on arrête toute animation s'il y en a
                if (lazerAnim.IsPlaying())
                    lazerAnim.Kill();
                // et on ne shoot plus
                lazerAlreadyShoot = false;
            };
        }
    }

    private void FollowPlayer(GameObject obj)
    {
        // Position du Ram
        obj.transform.position = Game.instance.Player.vehicle.transform.position;

        // Rotation du Ram
        obj.transform.rotation = Game.instance.Player.vehicle.transform.rotation;
        obj.transform.Rotate(new Vector3(obj.transform.rotation.x, obj.transform.rotation.y, obj.transform.rotation.z - 90));
    }
}

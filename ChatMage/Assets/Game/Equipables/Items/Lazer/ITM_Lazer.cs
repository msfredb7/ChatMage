using DG.Tweening;
using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Lazer : Item
{
    public LazerController lazerPrefab;
    [NonSerialized,fsIgnore]
    private LazerController lazer;

    public GameObject lazerTargetZone;
    [NonSerialized, fsIgnore]
    private GameObject zone;

    public GameObject countainerPrefab;
    [NonSerialized, fsIgnore]
    private GameObject countainer;

    public GameObject leftLazerPrefab;
    [NonSerialized, fsIgnore]
    private GameObject leftLazer;

    public GameObject rightLazerPrefab;
    [NonSerialized, fsIgnore]
    private GameObject rightLazer;

    [InspectorHeader("Duration"), InspectorTooltip("Shoot a unit will take this amount of time * 2")]
    public float animationDuration;

    [NonSerialized, fsIgnore]
    private bool lazerAlreadyShoot = false;

    [NonSerialized, fsIgnore]
    private ColliderInfo currentTarget;

    [NonSerialized, fsIgnore]
    private Sequence lazerAnim;

    public override void OnGameReady()
    {
        // Nous n'avons pas encore shooter
        lazerAlreadyShoot = false;

        // Création de la claymore
        if (countainerPrefab == null || leftLazerPrefab == null || rightLazerPrefab == null)
            throw new Exception();
        countainer = Instantiate(countainerPrefab, Game.instance.Player.vehicle.transform);
        leftLazer = Instantiate(leftLazerPrefab, countainer.transform);
        rightLazer = Instantiate(rightLazerPrefab, countainer.transform);

        // Claymore doit suivre la rotation du joueur
        FollowPlayer(countainer);
    }

    public override void OnGameStarted()
    {
        // Creation de la zone du lazer
        if (lazerTargetZone == null)
            throw new Exception();
        zone = Instantiate(lazerTargetZone, Game.instance.Player.vehicle.transform);

        // Evennement de sortie et d'entrée dans la zone d'effet du lazer
        zone.GetComponent<SimpleColliderListener>().onTriggerEnter += ITM_Lazer_onTriggerEnter;
        zone.GetComponent<SimpleColliderListener>().onTriggerExit += ITM_Lazer_onTriggerExit; ;
    }

    public override void OnUpdate()
    {
        // Ajustement des rotations
        if (zone != null)
            FollowPlayer(zone);
        if (countainer != null)
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
            if (lazerPrefab == null)
                throw new Exception();
            lazer = Instantiate(lazerPrefab, Game.instance.Player.vehicle.transform);
            // qui durera un certain temps
            //lazer.SetAnimationDuration(animationDuration);

            // après que l'animation des claymores soit faites
            lazerAnim = DOTween.Sequence();
            lazerAnim.Append(leftLazer.transform.DOLocalRotate(new Vector3(0, 0, -15), animationDuration));
            lazerAnim.Join(rightLazer.transform.DOLocalRotate(new Vector3(0, 0, 15), animationDuration));
            lazerAnim.OnComplete(delegate() {
                // On lance le lazer
                lazerAlreadyShoot = true;
                lazer.AimAt(other.transform.position);
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
        // Position
        obj.transform.position = Game.instance.Player.vehicle.transform.position;

        // Rotation
        obj.transform.rotation = Game.instance.Player.vehicle.transform.rotation;
        obj.transform.Rotate(new Vector3(obj.transform.rotation.x, obj.transform.rotation.y, obj.transform.rotation.z - 90));
    }
}

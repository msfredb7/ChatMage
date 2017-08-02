using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using AI;

public class AC130Effect : MonoBehaviour
{
    [Header("Linking")]
    public Image blackFade;
    public Material blackAndWhiteMat;
    public AC130CrossairAnim crossair;
    public Text ammoText;
    public GameObject container;
    public AC130Bullet bulletPrefab;

    [Header("Fade Settings")]
    public float fadeDuration = 0.3f;

    [Header("Shoot Settings")]
    public float reloadDuration = 1.5f;
    public float shakeIntensity = 1;
    public float shakeDuration = 1;

    Action onComplete;
    float remainingDuration = 0;
    private bool ending = false;
    private Invert_BlackAndWhite blackAndWhite;
    private int ammo = 3;
    private float remainingReloadTime;

    private List<Goal> forcedGoals;

    void Awake()
    {
        gameObject.SetActive(false);
        container.SetActive(false);
    }

    void Start()
    {
        blackAndWhite = Game.instance.gameCamera.cam.gameObject.AddComponent<Invert_BlackAndWhite>();
        blackAndWhite.material = blackAndWhiteMat;
        blackAndWhite.enabled = false;
    }

    void Update()
    {
        if (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime * Game.instance.worldTimeScale;

            if (remainingDuration <= 0)
            {
                End();
            }
        }

        if (remainingReloadTime > 0)
        {
            remainingReloadTime -= Time.deltaTime * Game.instance.worldTimeScale;

            //Si on a plus d'ammo, end
            if (remainingReloadTime <= 0 && ammo == 0)
                End();
        }
        else if (Toucher.IsTouching && !ending)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector2 pos = Game.instance.gameCamera.cam.ScreenToWorldPoint(Toucher.GetTouchPosition());
        Instantiate(bulletPrefab.gameObject, pos, Quaternion.identity, Game.instance.unitsContainer)
            .GetComponent<AC130Bullet>().Init(blackFade);

        //Reload time
        remainingReloadTime = reloadDuration;

        //crossair anim
        crossair.Shoot(Toucher.GetTouchPosition(), reloadDuration);

        //Shake
        Game.instance.gameCamera.vectorShaker.Shake(shakeIntensity, shakeDuration);

        //black fade anim ?

        //Ammo
        ammo--;
        UpdateAmmoDisplay();
    }

    public void Smash(float duration, int ammo, Action onComplete)
    {
        this.ammo = ammo;
        this.onComplete = onComplete;
        remainingDuration = duration;
        ending = false;

        gameObject.SetActive(true);

        //Black fade in
        blackFade.DOFade(1, fadeDuration).OnComplete(OnEnterCockpit).SetUpdate(false);
    }

    void OnEnterCockpit()
    {
        //Activate black and white effect
        blackAndWhite.enabled = true;

        container.SetActive(true);

        UpdateAmmoDisplay();

        //Deactivate player
        if (Game.instance.Player != null)
            Game.instance.Player.gameObject.SetActive(false);


        //Panic units
        forcedGoals = new List<Goal>(Game.instance.attackableUnits.Count);

        LinkedListNode<Unit> node = Game.instance.attackableUnits.First;
        while (node != null)
        {
            Unit val = node.Value;
            PanicUnit(val);
            node = node.Next;
        }

        Game.instance.onUnitSpawned += PanicUnit;

        //Black fade out
        blackFade.DOFade(0, fadeDuration).SetUpdate(false);
    }

    private void PanicUnit(Unit unit)
    {
        if (unit is EnemyVehicle)
        {
            EnemyBrainV2 enemyBrainV2 = unit.GetComponent<EnemyBrainV2>();
            if (enemyBrainV2 != null)
            {
                Goal panicGoal = new Goal_Panic(unit as EnemyVehicle);
                enemyBrainV2.AddForcedGoal(panicGoal, 0);
                forcedGoals.Add(panicGoal);
            }
        }
    }

    void End()
    {
        if (ending)
            return;

        ending = true;

        for (int i = 0; i < forcedGoals.Count; i++)
        {
            if (forcedGoals[i] == null)
                break;
            forcedGoals[i].ForceCompletion();
        }
        forcedGoals = null;

        //Black fade in
        blackFade.DOKill();
        blackFade.DOFade(1, fadeDuration).OnComplete(OnExitCockpit).SetUpdate(false);
    }

    void OnExitCockpit()
    {
        //Black fade out, then deactivate self
        blackFade.DOFade(0, fadeDuration).OnComplete(delegate ()
        {
            gameObject.SetActive(false);
        }).SetUpdate(false);

        //Deactivate black and white effect
        blackAndWhite.enabled = false;

        container.SetActive(false);

        //Reactivate player
        if (Game.instance.Player != null)
            Game.instance.Player.gameObject.SetActive(true);

        Game.instance.onUnitSpawned -= PanicUnit;

        if (onComplete != null)
            onComplete();
    }

    void UpdateAmmoDisplay()
    {
        ammoText.text = ammo.ToString();
    }
}

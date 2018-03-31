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

    [Header("Audio")]
    public AudioPlayable enterSound;
    public AudioSource[] ambientSources;
    public AudioPlayable shootSFX;
    public AudioPlayable preHitSFX;
    public float preHitDelay = 0.761f;
    public float audioTimeScaleEffect = 0.5f;

    [Header("Fade Settings")]
    public float fadeDuration = 0.3f;

    [Header("Shoot Settings")]
    public float reloadDuration = 1.5f;
    public float shakeIntensity = 1;
    public float shakeDuration = 1;
    public bool scaleReloadTimeWithWorldTimescale = false;

    Action onComplete;
    float remainingDuration = 0;
    private bool ending = false;
    private Invert_BlackAndWhite blackAndWhite;
    private int ammo = 3;
    private float remainingReloadTime;

    private List<Goal> forcedGoals;
    private Tween enterExitAnimation;
    private float[] ambientSourceVolumes;

    void Awake()
    {
        gameObject.SetActive(false);
        container.SetActive(false);

        ambientSourceVolumes = new float[ambientSources.Length];
        for (int i = 0; i < ambientSources.Length; i++)
        {
            ambientSourceVolumes[i] = ambientSources[i].volume;
        }
    }

    void OnEnable()
    {
        Game.Instance.Player.vehicle.OnDeath += OnPlayerDeath;
        Game.Instance.worldTimeScale.onSet.AddListener(OnTimeScaleChange);
    }

    void OnDisable()
    {
        if (Game.Instance != null && Game.Instance.Player != null)
            Game.Instance.Player.vehicle.OnDeath -= OnPlayerDeath;
    }

    void OnTimeScaleChange(float worldTimescale)
    {
        ApplyTimescaleToAnimations();
    }

    void ApplyTimescaleToAnimations()
    {
        //On ne le fait pas finalement, c'est bcp trop long pour rien
        //enterExitAnimation.timeScale = Game.Instance.worldTimeScale;
    }

    void OnPlayerDeath(Unit unit)
    {
        End();
    }

    void Start()
    {
        blackAndWhite = Game.Instance.gameCamera.cam.gameObject.AddComponent<Invert_BlackAndWhite>();
        blackAndWhite.material = blackAndWhiteMat;
        blackAndWhite.enabled = false;
    }

    void Update()
    {
        if (!Game.Instance.gameRunning)
            return;

        // CAS EXCEPTIONNEL: Il faut call le player smash à la main parce que le joueur est désactivé
        Game.Instance.Player.playerSmash.Update();

        if (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime * Game.Instance.worldTimeScale;

            if (remainingDuration <= 0)
            {
                End();
            }
        }

        if (remainingReloadTime > 0)
        {
            remainingReloadTime -= Time.deltaTime * (scaleReloadTimeWithWorldTimescale ? Game.Instance.worldTimeScale : 1f);

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
        Vector2 pos = Game.Instance.gameCamera.cam.ScreenToWorldPoint(Toucher.GetTouchPosition());

        var bullet = Game.Instance.SpawnUnit(bulletPrefab, pos);
        bullet.Init(blackFade);

        //Reload time
        remainingReloadTime = reloadDuration;

        //crossair anim
        crossair.Shoot(Toucher.GetTouchPosition(), reloadDuration);

        //Shake
        Game.Instance.gameCamera.vectorShaker.Shake(shakeIntensity, shakeDuration);

        // SFX
        if (shootSFX != null)
            DefaultAudioSources.PlaySFX(shootSFX);

        if (preHitSFX != null && !Game.Instance.Player.playerSmash.SmashInProgress)
        {
            var totalDelay = Mathf.Max(0, bullet.arriveDelay - preHitDelay);
            Game.Instance.events.AddDelayedAction(() => DefaultAudioSources.PlaySFX(preHitSFX), totalDelay);
        }

        //Ammo
        ammo--;
        UpdateAmmoDisplay();
    }

    public void Smash(float duration, int ammo, Action onComplete)
    {
        if (gameObject.activeSelf && !ending)
        {
            remainingDuration += duration;
            this.ammo += ammo;
        }
        else
        {

            this.ammo = ammo;
            this.onComplete = onComplete;
            remainingDuration = duration;
            ending = false;

            gameObject.SetActive(true);

            if (enterExitAnimation != null && enterExitAnimation.IsActive())
                enterExitAnimation.Kill();

            // Ambiant audio
            for (int i = 0; i < ambientSources.Length; i++)
            {
                ambientSources[i].DOKill();
                ambientSources[i].volume = 0;
                ambientSources[i].DOFade(ambientSourceVolumes[i], 1);
            }

            // Arrival audio
            if (enterSound != null)
                DefaultAudioSources.PlaySFX(enterSound);

            //Black fade in
            enterExitAnimation = blackFade.DOFade(1, fadeDuration).OnComplete(OnEnterCockpit);

            ApplyTimescaleToAnimations();
        }
        UpdateAmmoDisplay();
    }

    void OnEnterCockpit()
    {
        //Activate black and white effect
        blackAndWhite.enabled = true;

        container.SetActive(true);

        UpdateAmmoDisplay();

        //Deactivate player
        if (Game.Instance.Player != null)
            Game.Instance.Player.gameObject.SetActive(false);


        //Panic units
        forcedGoals = new List<Goal>(Game.Instance.attackableUnits.Count);

        LinkedListNode<Unit> node = Game.Instance.attackableUnits.First;
        while (node != null)
        {
            Unit val = node.Value;
            PanicUnit(val);
            node = node.Next;
        }

        Game.Instance.onUnitSpawned += PanicUnit;

        //Black fade out
        enterExitAnimation = blackFade.DOFade(0, fadeDuration);

        ApplyTimescaleToAnimations();
    }

    private void PanicUnit(Unit unit)
    {
        if (unit is EnemyVehicle)
        {
            EnemyBrainV2 enemyBrainV2 = unit.GetComponent<EnemyBrainV2>();
            if (enemyBrainV2 != null && enemyBrainV2.canGetForcedGoals)
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

        if (forcedGoals != null)
        {
            for (int i = 0; i < forcedGoals.Count; i++)
            {
                if (forcedGoals[i] == null)
                    break;
                forcedGoals[i].ForceCompletion();
            }
            forcedGoals = null;
        }

        Game.Instance.onUnitSpawned -= PanicUnit;


        // Ambiant audio
        for (int i = 0; i < ambientSources.Length; i++)
        {
            ambientSources[i].DOKill();
            ambientSources[i].DOFade(0, 1);
        }

        //Black fade in
        if (enterExitAnimation != null && enterExitAnimation.IsActive())
            enterExitAnimation.Kill();

        enterExitAnimation = blackFade.DOFade(1, fadeDuration).OnComplete(OnExitCockpit);
        ApplyTimescaleToAnimations();
    }

    void OnExitCockpit()
    {
        //Black fade out, then deactivate self
        enterExitAnimation = blackFade.DOFade(0, fadeDuration).OnComplete(delegate ()
        {
            gameObject.SetActive(false);
        });

        //Deactivate black and white effect
        blackAndWhite.enabled = false;

        container.SetActive(false);

        //Reactivate player
        if (Game.Instance.Player != null)
            Game.Instance.Player.gameObject.SetActive(true);

        if (onComplete != null)
            onComplete();

        ApplyTimescaleToAnimations();
    }

    void UpdateAmmoDisplay()
    {
        ammoText.text = ammo.ToString();
    }
}

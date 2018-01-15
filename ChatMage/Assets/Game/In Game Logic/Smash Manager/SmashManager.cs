using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashManager : MonoBehaviour
{
    //[SerializeField]
    //private SmashBall ballPrefab;
    //[SerializeField]
    //private float totalCooldown;
    //[SerializeField]
    //private Animator followTarget;
    //[SerializeField]
    //private Transform followTargetParent;
    //[SerializeField]
    //private bool debug = false;

    //[System.NonSerialized]
    //private SmashBall currentSmashBall;

    //private float remainingTime = 0;
    //private bool inCooldown = true;

    //public void DecreaseCooldown(float amount)
    //{
    //    remainingTime -= amount;
    //}

    public bool smashEnabled = true;
    public Locker canGainJuice = new Locker();
    //public float TotalCooldown { get { return totalCooldown; } }
    //public SmashBall CurrentSmashBall { get { return currentSmashBall; } }
    //public bool IsInCooldown { get { return inCooldown; } }
    //public float RemainingTime { get { return remainingTime; } }
    //public event SimpleEvent onSmashSpawned;

    // SMASH V2
    //public bool activateV2 = true;
    private float currentJuice;
    private float minimumActivatableJuice = 3;
    private float maxJuice = 10;

    private bool needSound = true;

    public float MinimumActivatableJuice
    {
        get { return minimumActivatableJuice; }
        set
        {
            minimumActivatableJuice = value;
            if (onMinimumJuiceChange != null)
                onMinimumJuiceChange();
        }
    }
    public float MaxJuice
    {
        get { return maxJuice; }
        set
        {
            maxJuice = value;
            if (onMaxJuiceChange != null)
                onMaxJuiceChange();
        }
    }
    public float CurrentJuice { get { return currentJuice; } }
    public bool IsEnabled
    {
        get { return enabled; }
        private set
        {
            enabled = value;
            if (onEnableOrDisable != null)
                onEnableOrDisable();
        }
    }

    public event SimpleEvent onJuiceChange;
    public event SimpleEvent onMaxJuiceChange;
    public event SimpleEvent onMinimumJuiceChange;
    public event SimpleEvent onEnableOrDisable;

    void Start()
    {
        RemoveAllJuice();

        Game.Instance.onGameStarted += OnGameStarted;
        Game.Instance.onGameReady += OnGameReady;
        //Game.instance.worldTimeScale.onSet.AddListener(OnWorldTimeScaleChanged);
        enabled = false;

        //if (followTargetParent != null)
        //    followTargetParent.gameObject.SetActive(false);
    }

    //void OnWorldTimeScaleChanged(float newValue)
    //{
    //    followTarget.speed = newValue;
    //}

    void OnGameReady()
    {
        //if (followTarget != null)
        //{
        //    followTargetParent.localScale = Game.instance.gameCamera.AdjustVector(Vector3.one);
        //    followTargetParent.transform.localPosition = Game.instance.gameCamera.AdjustVector(followTargetParent.transform.localPosition);
        //}

        //if (activateV2)
        //{
        Game.Instance.Player.playerStats.OnUnitKilled += BoostSmashCounter;
        //}
    }

    void OnGameStarted()
    {
        IsEnabled = Game.Instance.Player.playerSmash.SmashEquipped && smashEnabled;
    }

    //void ResetCooldown()
    //{
    //    float multiplier = 1;
    //    if (Game.instance.Player != null)
    //        multiplier = Game.instance.Player.playerStats.smashCooldownRate;
    //    remainingTime = totalCooldown * multiplier;

    //    inCooldown = true;
    //}

    public void RemoveAllJuice()
    {
        currentJuice = 0;
        if (onJuiceChange != null)
            onJuiceChange();
    }

    //void Update()
    //{
    //    if (activateV2)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            Game.instance.Player.playerSmash.ForceDoSmash();
    //        }
    //        return;
    //    }

    //    if (debug)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            if (currentSmashBall != null)
    //                currentSmashBall.ForceDeath();
    //            else
    //                remainingTime = 0;
    //        }
    //    }

    //    //On ne diminue pas le cooldown si une smash ball est en vie
    //    if (!inCooldown)
    //        return;

    //    if (Game.instance.Player.playerStats.smashRefreshRate < 0)
    //        return;

    //    float multiplier = 1;
    //    if (Game.instance.Player != null)
    //        multiplier = Game.instance.Player.vehicle.TimeScale * Game.instance.Player.playerStats.smashRefreshRate;
    //    remainingTime -= Time.deltaTime * multiplier;

    //    if (remainingTime <= 0)
    //        SpawnSmashBall();
    //}
    /*
    private void SpawnSmashBall()
    {
        inCooldown = false;
        bool verticalSpawn = Random.value > 0.5f;
        float y = 0;
        float x = 0;
        if (verticalSpawn)
        {
            y = (Random.value > 0.5f ? 6.5f : -6.5f) + Game.instance.gameCamera.Height; //Soit 6.5 ou -6.5
            x = (Random.value * 20) - 10;           //Entre -10 et 10
        }
        else
        {
            y = ((Random.value * 13) - 6.5f) + Game.instance.gameCamera.Height;         //Entre -6.5 et 6.5
            x = Random.value > 0.5f ? 10f : -10f;   //Soit 10 ou -10
        }

        currentSmashBall = Game.instance.SpawnUnit(ballPrefab, new Vector2(x, y));

        currentSmashBall.onDeath += OnSmashTaken;

        if (followTargetParent != null)
        {
            followTargetParent.gameObject.SetActive(true);
            currentSmashBall.followTarget = followTarget.transform;
        }

        if (onSmashSpawned != null)
            onSmashSpawned();
    }
    

    private void OnSmashTaken(Unit smashUnit)
    {
        if (followTargetParent != null)
            followTargetParent.gameObject.SetActive(false);

        currentSmashBall = null;

        Game.instance.Player.playerSmash.GainSmash();
        Game.instance.Player.playerSmash.onSmashCompleted += OnSmashCompleted;
    }
    
    private void OnSmashCompleted()
    {
        ResetCooldown();
    }
    */

    private void BoostSmashCounter(Unit unit)
    {
        if (unit.GetComponent<IAttackable>() != null)
            IncreaseSmashJuice(unit.GetComponent<IAttackable>().GetSmashJuiceReward());
    }

    public void IncreaseSmashJuice(float amount)
    {
        if (!enabled)
            return;

        //On ne gagne pas de juice si on a pas le droit
        if (amount > 0 && !canGainJuice)
            return;

        currentJuice = (CurrentJuice + amount).Clamped(0, maxJuice);

        if(CanSmash() && needSound)
        {
            needSound = false;
            Game.Instance.commonSfx.SmashActive();
        } else if(!CanSmash() && !needSound)
        {
            needSound = true;
        }

        if (onJuiceChange != null)
            onJuiceChange();
    }

    public bool CanSmash()
    {
        return enabled && CurrentJuice >= minimumActivatableJuice;
    }
}
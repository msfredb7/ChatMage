using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AC130Effect : MonoBehaviour
{
    public Image blackFade;
    public float fadeDuration = 0.3f;
    public Material blackAndWhiteMat;
    public AC130ShootAnim crossair;

    Action onComplete;
    float remainingDuration = 0;
    private bool ending = false;
    private Invert_BlackAndWhite blackAndWhite;

    void Awake()
    {
        gameObject.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.S))
        {
            crossair.Shoot(Vector2.zero);
        }
    }

    public void Smash(float duration, Action onComplete)
    {
        this.onComplete = onComplete;
        remainingDuration = duration;
        gameObject.SetActive(true);

        //Black fade in
        blackFade.DOFade(1, fadeDuration).OnComplete(OnEnterCockpit).SetUpdate(false);
    }

    void OnEnterCockpit()
    {
        //Activate black and white effect
        blackAndWhite.enabled = true;

        //Deactivate player
        if (Game.instance.Player != null)
            Game.instance.Player.gameObject.SetActive(false);

        //Panic to all units
        for (int i = 0; i < Game.instance.units.Count; i++)
        {
            if (Game.instance.units[i] != Game.instance.Player)
                PanicUnit(Game.instance.units[i]);
        }

        Game.instance.onUnitSpawned += PanicUnit;

        //Black fade out
        blackFade.DOFade(0, fadeDuration).SetUpdate(false);
    }

    private void PanicUnit(Unit unit)
    {
        EnemyBrain enemyBrain = unit.GetComponent<EnemyBrain>();
        if (enemyBrain != null)
            enemyBrain.ForceBehavior(BehaviorType.Panic, remainingDuration);
    }

    void End()
    {
        if (ending)
            return;

        ending = true;

        //Black fade in
        blackFade.DOFade(1, fadeDuration).OnComplete(OnExitCockpit).SetUpdate(false);
    }

    void OnExitCockpit()
    {
        //Black fade out, then deactivate self
        blackFade.DOFade(0, fadeDuration).OnComplete(delegate()
        {
            gameObject.SetActive(false);
        }).SetUpdate(false);

        //Deactivate black and white effect
        blackAndWhite.enabled = false;
        
        //Reactivate player
        if (Game.instance.Player != null)
            Game.instance.Player.gameObject.SetActive(true);


        Game.instance.onUnitSpawned -= PanicUnit;

        if (onComplete != null)
            onComplete();
    }
}

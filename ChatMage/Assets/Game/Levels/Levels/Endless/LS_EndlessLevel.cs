using LevelScripting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class LS_EndlessLevel : LevelScript
{
    // Gates
    SidewaysFakeGate topgate;
    SidewaysFakeGate botgate;

    // Spawn
    string spawnTag = "random";
    public List<Unit> possibleUnits;
    public float spawnInterval = 0.5f;
    GameObject playerSpawn;

    // Etage
    int currentStage;
    private const string LOCK_KEY = "transitionlock";
    public float playerEnterDelay = 1.5f;
    float transitionDuration = 1;
    float transitionPause = 0.5f;
    float gateClosingDelay = 0.5f;
    SimpleColliderListener topdetector;
    SimpleColliderListener botdetector;
    bool waitingForExit = false;
    bool waitingForEnter = false;
    UnityEvent startTransition = new UnityEvent();
    UnityEvent startNextWave = new UnityEvent();

    // Initialisation avant le debut de la partie
    protected override void OnGameReady()
    {
        Game.Instance.smashManager.smashEnabled = true;
        Game.Instance.ui.smashDisplay.canBeShown = true;

        // Obtenir les références nécessaire
        topgate = Game.Instance.map.mapping.GetTaggedObject("topgate").GetComponent<SidewaysFakeGate>();
        botgate = Game.Instance.map.mapping.GetTaggedObject("botgate").GetComponent<SidewaysFakeGate>();
        topdetector = Game.Instance.map.mapping.GetTaggedObject("topdetector").GetComponent<SimpleColliderListener>();
        topdetector.onTriggerEnter += PlayerEnteringTopDetector;
        botdetector = Game.Instance.map.mapping.GetTaggedObject("botdetector").GetComponent<SimpleColliderListener>();
        botdetector.onTriggerExit += PlayerEnteringMap;

        playerSpawn = Game.Instance.map.mapping.GetTaggedObject("respawn").gameObject;

        currentStage = 1;

        Game.Instance.ui.stageText.text = "Stage " + currentStage;
        Game.Instance.ui.stageText.gameObject.SetActive(true);

    }

    // Initialisation lors du debut de la partie
    protected override void OnGameStarted()
    {
        botgate.Close();
        SpawnWave();
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            default:
                break;
        }
    }

    // Spawn d'une vague d'ennemi durant l'étage
    private void SpawnWave()
    {
        // Definition de la vague
        UnitWaveV2 wave = new UnitWaveV2();
        wave.infiniteRepeat = false;
        wave.pauseBetweenRepeat = 0;
        wave.spawnInterval = 1;
        wave.preLaunchDialog = null;

        // What ?
        wave.what = new WaveWhat();
        wave.what.spawnSequence = new UnitPack[1];
        wave.what.spawnSequence[0] = new UnitPack();
        wave.what.spawnSequence[0].unit = possibleUnits[0];
        wave.what.spawnSequence[0].quantity = 1;

        // Where ?
        wave.where = new WaveWhereV2();
        wave.where.spawnTag = spawnTag;
        wave.where.filterType = WaveWhereV2.FilterType.None;
        wave.where.selectType = WaveWhereV2.SelectType.ByIndex;
        wave.where.index = 0;

        // When ?
        wave.when = new WaveWhen();
        wave.when.type = WaveWhen.Type.OnManualTrigger;
        wave.when.name = "wave";
        wave.when.onlyTriggerOnce = true;

        // Lorsque la vague est fini
        wave.onComplete += NextWave;

        // Initialisation de la vague
        ManuallyAddWave(wave);
        TriggerWaveManually("wave");
    }

    // Initialisation du processus de changement d'étages
    private void NextWave()
    {
        // Ouverture de la porte du haut
        waitingForExit = true;
        topgate.Open();
        Game.Instance.playerBounds.top.gameObject.SetActive(false);
        startTransition.AddListener(delegate ()
        {
            // Le joueur est dans la porte du haut
            // Apres un court delai (le temps qu'il rentre) on ferme tout et on teleporte le joueur
            Game.Instance.DelayedCall(delegate ()
            {
                topgate.Close();
                Game.Instance.playerBounds.top.gameObject.SetActive(true);
            }, gateClosingDelay);

            // Debut de la transition
            Transition(delegate() {
                // Quand la transition est fini on attend que le joueur entre
                startNextWave.AddListener(delegate () {
                    // le joueur est la on ferme tout
                    botgate.Close();
                    Game.Instance.playerBounds.bottom.gameObject.SetActive(true);
                    // on reset les event pour la prochaine fois
                    startTransition = new UnityEvent();
                    startNextWave = new UnityEvent();
                    // On spawn la prochaine vague
                    SpawnWave();
                });
            });
            currentStage++;
        });
    }

    // Animation de transition
    private void Transition(Action onComplete)
    {
        // Fade out
        Game.Instance.ui.transitionBG.DOFade(1, transitionDuration).OnComplete(delegate() {
            Game.Instance.ui.stageText.text = "Stage " + currentStage; // Changement du texte dans le ui
            Game.Instance.DelayedCall(delegate ()
            {
                // Fade in
                Game.Instance.ui.transitionBG.DOFade(0, transitionDuration).OnComplete(delegate () {
                    botgate.Open();
                    Game.Instance.playerBounds.bottom.gameObject.SetActive(false);
                    MovePlayer(onComplete);
                }).SetUpdate(true);
            }, transitionPause);
        }).SetUpdate(true);
    }

    // Deplacement du joueur
    private void MovePlayer(Action onComplete)
    {
        PlayerController player = Game.Instance.Player;
        Vehicle playerVehicle = player.vehicle;

        Game.Instance.DelayedCall(delegate () {
            player.playerDriver.enableInput = false;
            playerVehicle.TeleportPosition(playerSpawn.transform.position);
            playerVehicle.TeleportDirection(90);

            waitingForEnter = true;

            onComplete.Invoke();
        }, playerEnterDelay);

        /* Animation avec DOTWEEN qui marche pas

        Sequence sq = DOTween.Sequence();

        sq.Insert(playerEnterDelay, playerVehicle.transform.DOMove(new Vector3(0, Game.Instance.gameCamera.Height/2,0), 1 - playerEnterDelay)
            .OnComplete(delegate ()
            {
                //Re-enable player things
                player.playerDriver.enableInput = true;
            }));

        sq.InsertCallback(1, delegate ()
            {
                // La transition est vraiment fini
                onComplete.Invoke();
            })
            .SetUpdate(false);
        */
    }

    // DETECTORS

    private void PlayerEnteringTopDetector(ColliderInfo other, ColliderListener listener)
    {
        if (!waitingForExit)
            return;

        waitingForExit = false;
        startTransition.Invoke();
    }

    private void PlayerEnteringMap(ColliderInfo other, ColliderListener listener)
    {
        if (!waitingForEnter)
            return;

        waitingForEnter = false;
        Game.Instance.Player.playerDriver.enableInput = true;
        startNextWave.Invoke();
    }
}

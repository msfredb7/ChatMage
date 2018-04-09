﻿using LevelScripting;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using FullInspector;
using CCC.Math;

public class LS_EndlessLevel : LevelScript
{
    // Type d'ennemi possible et leur paramètre
    [System.Serializable]
    public class EnemyTypes : IWeight
    {
        public Unit unit;
        public int power;
        public int unlockAt = 0;
        public int cooldown = 0;
        [HideInInspector]
        public int counter = 0; // use to calculate the cooldown
        public float spawnInterval;

        public float Weight // Poid si jamais il n'y a aucun diversite
        {
            get { return power; }
        }
    }

    [System.Serializable]
    public class DifficultyProgression
    {
        public float miny;
        public float maxy;
        public FloatReference speed;
        public float minx;

        public float GetProgression(float difficultyValue)
        {
            return new NeverReachingCurve(miny, maxy, speed / 1000000f, minx).Evalutate(difficultyValue);
        }
    }

    [InspectorCategory("ENDLESS MODE"), InspectorHeader("Reset")]
    public static int stepToResetSave = 11;
    [InspectorCategory("ENDLESS MODE")]
    public Item startingItem;
    [InspectorCategory("ENDLESS MODE")]
    public SceneInfo stageSelection;

    // Spawn
    string spawnTag = "ordered";

    [InspectorCategory("ENDLESS MODE"), InspectorHeader("Ennemy Units")]
    public List<EnemyTypes> possibleUnits;

    // Progression de la difficulté
    [InspectorCategory("ENDLESS MODE"), InspectorHeader("Difficulty Progression")]
    public DifficultyProgression difficulty;
    [InspectorCategory("ENDLESS MODE")]
    public bool useCurveForSpawnInterval = false;
    [InspectorCategory("ENDLESS MODE"), InspectorHideIf("useCurveForSpawnInterval")]
    public float specificSpawnInterval = 1f;
    [InspectorCategory("ENDLESS MODE"), InspectorShowIf("useCurveForSpawnInterval")]
    public DifficultyProgression spawnInterval;
    [InspectorCategory("ENDLESS MODE")]
    public DifficultyProgression enemyDiversity;
    [InspectorCategory("ENDLESS MODE")]
    public DifficultyProgression wavePower;

    // Etage
    int currentStep; // palier courrant total incluant tous les etapes
    int currentStage; // etage courrant

    // Transition
    GameObject playerSpawn;
    private const string LOCK_KEY = "transitionlock";
    [InspectorCategory("ENDLESS MODE"), InspectorHeader("Tranistion Animation")]
    public float playerEnterDelay = 1.5f;
    float transitionDuration = 1;
    float gateAnimDelay = 0.5f;
    SimpleColliderListener topdetector;
    SimpleColliderListener botdetector;
    bool waitingForExit = false;
    bool waitingForEnter = false;
    UnityEvent startTransition = new UnityEvent();
    UnityEvent startNextWave = new UnityEvent();
    [InspectorCategory("ENDLESS MODE")]
    public float fadeTextAnim = 1.0f;

    // Gates and arrow
    SidewaysFakeGate topgate;
    SidewaysFakeGate botgate;
    GuideArrow arrow;

    // UI
    EndlessUI ui;

    // Stage at Start
    public const string stageKey = "EndlessStage";

    // Best Stage
    public const string bestStepKey = "EndlessBestStage";
    int currentBest;

    // Item Charge
    public int[] charges = { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0 };
    private int currentAmounOfCharges;

    // Initialisation avant le debut de la partie
    protected override void OnGameReady()
    {
        Game.Instance.smashManager.smashEnabled = true;
        Game.Instance.ui.smashDisplay.canBeShown = true;

        startTransition.RemoveAllListeners();
        startNextWave.RemoveAllListeners();

        // Obtenir les références nécessaire
        topgate = Game.Instance.map.mapping.GetTaggedObject("topgate").GetComponent<SidewaysFakeGate>();
        botgate = Game.Instance.map.mapping.GetTaggedObject("botgate").GetComponent<SidewaysFakeGate>();
        topdetector = Game.Instance.map.mapping.GetTaggedObject("topdetector").GetComponent<SimpleColliderListener>();
        topdetector.onTriggerEnter += PlayerEnteringTopDetector;
        botdetector = Game.Instance.map.mapping.GetTaggedObject("botdetector").GetComponent<SimpleColliderListener>();
        botdetector.onTriggerExit += PlayerEnteringMap;
        arrow = Game.Instance.map.mapping.GetTaggedObject("arrow").GetComponent<GuideArrow>();

        playerSpawn = Game.Instance.map.mapping.GetTaggedObject("respawn").gameObject;

        LoadStageInfo();

        // Chargement du UI
        // Si la scene prend du temps a charge par rapport a l'intro il faudra que le jeu attend pour CECI
        Scenes.LoadAsync(EndlessUI.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            ui = scene.FindRootObject<EndlessUI>();
            ui.waveIntro.ShowWaveIntro(currentStep);
            //ui.stageText.text = "Stage " + currentStage + " Step " + (currentStep - ((currentStage - 1) * (stepToResetSave - 1)));
            //ui.stageText.DOFade(1, fadeTextAnim);
        });
    }

    // Initialisation lors du debut de la partie
    protected override void OnGameStarted()
    {
        // La porte est ouverte par default, il faut donc fermer apres
        botgate.Close();
        // On fade out l'affichage de l'etage
        //ui.stageText.DOFade(0, fadeTextAnim);
        // On spawn maintenant la premiere vague !
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

    protected override void OnEnable()
    {
        base.OnEnable();
        // clear
        currentStep = 0;
        currentStage = 0;
    }

    private void LoadStageInfo()
    {
        // Get current stage and step
        currentStage = PlayerPrefs.GetInt(stageKey, 1);
        currentStep = ((currentStage - 1) * (stepToResetSave - 1)) + 1;
        //Debug.Log(currentStep);
        if (currentStep < 1)
            currentStep = 1;

        // Get current best
        if (dataSaver.ContainsInt(bestStepKey))
            currentBest = dataSaver.GetInt(bestStepKey);
        else
        {
            currentBest = currentStep;
            dataSaver.SetInt(bestStepKey, currentBest);
            dataSaver.Save();
        }
    }

    // Spawn d'une vague d'ennemi durant l'étage
    private void SpawnWave()
    {
        // Definition de la vague
        UnitWaveV2 wave = new UnitWaveV2();
        wave.infiniteRepeat = false;
        wave.pauseBetweenRepeat = 0;
        if (useCurveForSpawnInterval)
            wave.spawnInterval = 1 / (spawnInterval.GetProgression(Mathf.RoundToInt(difficulty.GetProgression(currentStep))));
        else
            wave.spawnInterval = specificSpawnInterval;

        UnityEngine.Random.InitState(Mathf.RoundToInt(difficulty.GetProgression(currentStep)));

        /*Debug.Log("Current Stage : " + currentStage + "\n" +
        "Current Step : " + currentStep + "\n" +
        "Step in Stage : " + (currentStep - ((currentStage - 1) * (stepToResetSave - 1))) + "\n" +
        "Spawn Interval : " + wave.spawnInterval + "\n" +
        "Amount of Charges : " + currentAmounOfCharges + "\n" +
        "Current Best : " + currentBest + "\n" +
        "Is Debug : " + useDebug + "\n" +
        "Current Difficulty : " + difficulty.GetProgression(currentStep) + "\n" +
        "Difficulty maxy : " + difficulty.maxy + "\n" +
        "Difficulty minx : " + difficulty.minx + "\n" +
        "Difficulty miny : " + difficulty.miny + "\n" +
        "Difficulty speed : " + difficulty.speed.Value + "\n" +
        "Real Wave Power : " + GetRealStepForPower(currentStep, currentStage) + "\n");*/

        // What ?
        wave.what = new WaveWhat();

        List<float> spawnIntervals;
        wave.what.spawnSequence = CreateUnitWave(GetRealStepForPower(currentStep, currentStage), possibleUnits, out spawnIntervals);
        wave.spawnIntervals = spawnIntervals.ToArray();
        wave.variableIntervals = true;

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
        wave.onComplete += delegate ()
        {
            Game.Instance.DelayedCall(delegate ()
            {
                Game.Instance.music.PlaySong(MusicManager.SongName.Ambient, true);
                GoToNextWave();
            }, 0.5f);
        };


        // Gestion des Charges
        if (charges.Length <= 0)
            GiveCharge(0);
        else
            GiveCharge(charges[(currentStep - ((currentStage - 1) * (stepToResetSave - 1))) - 1]);


        // Initialisation de la vague
        ManuallyAddWave(wave);
        TriggerWaveManually("wave");
        Game.Instance.music.PlaySong(MusicManager.SongName.Fight, true);


        // On re-randomise le jeu
        UnityEngine.Random.InitState(Mathf.RoundToInt(Time.timeSinceLevelLoad * 1000));
    }

    // Pour balance les etages, on va toujours utiliser le power d'un autre step plus bas selon cette formule
    private int GetRealStepForPower(int step, int stage)
    {
        return (stage + (step - ((stage - 1) * (stepToResetSave - 1))));
    }

    // Algorithm to create a good wave depending on the stage
    private UnitPack[] CreateUnitWave(int stepUsedForPowerMesure, List<EnemyTypes> units, out List<float> spawnIntervals)
    {
        List<UnitPack> unitPack = new List<UnitPack>();
        spawnIntervals = new List<float>();
        int difficultyTarget = Mathf.RoundToInt(difficulty.GetProgression(stepUsedForPowerMesure));
        int currentWavePower = Mathf.RoundToInt(wavePower.GetProgression(difficultyTarget));

        float currentDiversity = enemyDiversity.GetProgression(Mathf.RoundToInt(difficulty.GetProgression(stepUsedForPowerMesure)));

        //Debug.Log("Create wave of power" + currentWavePower + ",speed " + 1 / (spawnInterval.GetProgression(difficulty.GetProgression(currentStep))) + ",diversity " + currentDiversity);

        // On trouve la sommation de power qui permet d'obtenir de power total
        List<int> packsPower = FindIncreasingPartSum(currentWavePower);

        // On utilise la sommation de power pour generer les packs
        for (int i = 0; i < packsPower.Count; i++)
        {
            // On a cree un pack d'ennemi et on l'ajoute à la vague
            UnitPack currentPack = new UnitPack();
            CreateUnitPack(ref currentPack, packsPower[i], possibleUnits, currentDiversity, ref spawnIntervals);
            DecreaseCooldownOfOtherUnits(currentPack.unit);
            unitPack.Add(currentPack);
        }

        return unitPack.ToArray();
    }

    private List<int> FindIncreasingPartSum(int number)
    {
        // Si on passe 0, on quitte
        if (number <= 0)
            return null;

        // On cree le resultat
        List<int> result = new List<int>();

        // Trouver une liste qui fonctionne
        int sum = 1; // minimum de 1
        result.Add(1);
        // On additionne des nombres consecutifs jusqu'a obtenir notre nombre
        for (int i = 1; sum != number; i++)
        {
            // On ajoute le nombre courrant a la somme
            sum += i;
            // La somme est rendu trop grande ?
            if (sum > number)
            {
                // On revient en arriere et on recommence a partir de 0
                sum -= i;
                i = 0;
                continue;
            }
            else
            {
                // Notre somme st pas encore rendu a notre nombre, on ajoute donc le dernier nombre ajoute et on continue
                result.Add(i);
            }
        }
        // On a trouver la bonne combinaison ! Mais elle n'est pas encore croissante.

        // Optimisons la liste !
        for (int i = 0; i + 1 < result.Count; i++)
        {
            // Si l'element courrant est plus grand que le suivant, on doit optimiser
            if (result[i] > result[i + 1])
            {
                // On est deja la fin de la liste
                if (i + 2 > result.Count)
                {
                    // on ajoute le dernier terme a l'avant dernier et on quitte
                    result[i] += result[i + 1];
                    result.RemoveAt(i + 1);
                    break;
                }
                else
                {
                    // Sinon, on optimise tout les elements suivant
                    for (int j = i + 1; j + 1 < result.Count; j++)
                    {
                        // Si le terme suivant de celui qu'on optimise est plus petit que le terme courrant, on optimise
                        if (result[i] > result[j + 1])
                        {
                            // on concatene les deux termes suivant en un seul terme
                            result[i + 1] += result[j + 1];
                            // On l'elime
                            result.RemoveAt(j + 1);
                            // On continue tant qu'il n'y a plus d'autres suivants
                            j--;
                            continue;
                        }
                    }
                    // On a fini d'optimiser la fin de la liste en un seul terme
                    // Ce dernier terme a besoin d'etre optimiser egalement ?
                    if (result[i] > result[i + 1])
                    {
                        // on ajoute le dernier terme a l'avant dernier et on quitte
                        result[i] += result[i + 1];
                        result.RemoveAt(i + 1);
                        break;
                    }
                }
            }
        }
        //result.Print();
        return result;
    }

    // Create un pack d'ennemi a envoyé 
    private int CreateUnitPack(ref UnitPack pack, int power, List<EnemyTypes> units, float diversityFactor, ref List<float> spawnIntervals)
    {
        // On crée un lot aléatoire des ennemis accessible
        Lottery<EnemyTypes> enemyLot = new Lottery<EnemyTypes>();
        for (int i = 0; i < units.Count; i++)
        {
            // Si le power de la unit n'est pas trop fort comparé à la puissance de la wave
            if (units[i].power <= power)
            {
                // et si on a unlock la unit, on peut la spawn
                if (units[i].unlockAt <= currentStep)
                    enemyLot.Add(units[i], Mathf.FloorToInt(1 + (units[i].Weight / diversityFactor))); // chance de choisir cette unit influencer par la diversite
            }
        }

        // On choisit aleatoirement un ennemi
        EnemyTypes chosenEnemy = enemyLot.Pick();

        // Est-ce que le cooldown empeche la selection de cette unité ?
        if (chosenEnemy.counter > 0)
        {
            // Oui, on restart la selection
            return CreateUnitPack(ref pack, power, units, diversityFactor, ref spawnIntervals);
        }
        else
        {
            chosenEnemy.counter = chosenEnemy.cooldown; // Reset cooldown après avoir choisit cette unité
            pack.unit = chosenEnemy.unit;
            // On va en spawn le plus possible selon le power max de paquet d'ennemi
            pack.quantity = Mathf.RoundToInt(power / chosenEnemy.power);
            // On peut ajouter des choses qui influence le nombre d'ennemi ICI et tout s'ajuste


            for (int i = 0; i < pack.quantity; i++)
            {
                spawnIntervals.Add(chosenEnemy.spawnInterval);
            }

            // On a notre pack d'ennemi qui va spawn dans la wave
            // On retourne la puissance du pack
            return (chosenEnemy.power * pack.quantity);
        }
    }

    // Initialisation du processus de changement d'étages
    private void GoToNextWave()
    {
        Game.Instance.DelayedCall(delegate ()
        {
            // Ouverture de la porte du haut
            waitingForExit = true;
            arrow.Show();
            topgate.Open();
            Game.Instance.playerBounds.top.gameObject.SetActive(false);
            startTransition.AddListener(delegate ()
            {
                // Disable input joueur
                Game.Instance.Player.playerDriver.enableInput = false;

                // on reset les event pour la prochaine fois
                startTransition.RemoveAllListeners();

                // Le joueur est dans la porte du haut
                // Apres un court delai (le temps qu'il rentre) on ferme tout et on teleporte le joueur
                Game.Instance.DelayedCall(delegate ()
                {
                    topgate.Close();
                    arrow.Hide();
                    Game.Instance.playerBounds.top.gameObject.SetActive(true);
                }, gateAnimDelay);

                // On a battu le record !
                if (currentStep > currentBest)
                {
                    //Debug.Log("Nouveau Record : " + currentStep);
                    dataSaver.SetInt(bestStepKey, currentStep);
                    dataSaver.Save();
                    currentBest = currentStep;
                    // TODO : Faire une animation
                }

                //Debug.Log("Transitionning from step " + currentStep + ",pack " + (currentStep - ((currentStage - 1) * stepToResetSave)) + ",stage " + currentStage);

                // On passe au prochain step
                currentStep++;

                //Debug.Log("Transitionning to step " + currentStep + ",pack " + (currentStep - ((currentStage - 1) * stepToResetSave)) + ",stage " + currentStage);

                // Debut de la transition
                Transition(delegate ()
                {
                    // Quand la transition est fini on attend que le joueur entre
                    startNextWave.AddListener(delegate ()
                    {
                        // le joueur est la on ferme tout
                        botgate.Close();
                        //ui.stageText.DOFade(0, fadeTextAnim);
                        Game.Instance.playerBounds.bottom.gameObject.SetActive(true);
                        // on reset les event pour la prochaine fois
                        startNextWave.RemoveAllListeners();
                        // On spawn la prochaine vague
                        SpawnWave();
                    });
                });
            });
        }, gateAnimDelay);
    }

    // Animation de transition
    private void Transition(Action onComplete)
    {
        // Avant de Fade In
        // Si on avait franchis un palier
        if ((currentStep - ((currentStage - 1) * (stepToResetSave - 1))) >= stepToResetSave)
        {
            //Debug.Log("stage " + currentStage + " (step " + currentStep + ") done");
            // On transitionne vers le prochain pallier
            // On doit reset le endless mode
            // On sauvegarde qu'on s'est rendu a l'etage suivant
            if (currentBest < currentStage * stepToResetSave)
            {
                dataSaver.SetInt(bestStepKey, currentStage * stepToResetSave);
                dataSaver.Save();
            }
            // on revient a la selection de l'etage
            BackToStageSelection();
            return;
        }

        // Fade out
        ui.transitionBG.DOFade(1, transitionDuration).OnComplete(delegate ()
        {
            ClearRocks();

            // Teleport player
            MovePlayer(delegate ()
            {
                // Fade in
                ui.transitionBG.DOFade(0, transitionDuration).OnComplete(delegate ()
                {
                    ui.waveIntro.ShowWaveIntro(currentStep);

                    // quand le fade est fini on ouvre la porte en bas et enleve les bounds
                    botgate.Open();
                    Game.Instance.playerBounds.bottom.gameObject.SetActive(false);
                    onComplete.Invoke();
                }).SetUpdate(true);
            });
        }).SetUpdate(true);
    }

    // Deplacement du joueur (animation d'intro)
    private void MovePlayer(Action onComplete)
    {
        PlayerController player = Game.Instance.Player;
        Vehicle playerVehicle = player.vehicle;

        Game.Instance.DelayedCall(delegate ()
        {
            playerVehicle.TeleportPosition(playerSpawn.transform.position);
            playerVehicle.TeleportDirection(90);
            playerVehicle.rb.freezeRotation = true;

            // Note: Si on est dans le AC-130, il faut bouger le joueur à la main parce que son rigidbody est désactivé
            if (!player.gameObject.activeInHierarchy)
            {
                var tr = playerVehicle.transform;
                tr.position = playerSpawn.transform.position;
                tr.rotation = Quaternion.Euler(Vector3.forward * 90);
            }

            waitingForEnter = true;

            onComplete.Invoke();
        }, playerEnterDelay);
    }

    // Retour a la tour
    private void BackToStageSelection()
    {
        Win();
    }

    private void DecreaseCooldownOfOtherUnits(Unit unit)
    {
        for (int i = 0; i < possibleUnits.Count; i++)
        {
            if (possibleUnits[i].unit != unit)
                possibleUnits[i].counter--;
        }
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
        Game.Instance.Player.vehicle.rb.freezeRotation = false;
        startNextWave.Invoke();
    }

    // CHARGES

    public int GetCharges()
    {
        return currentAmounOfCharges;
    }

    public void UseCharge()
    {
        currentAmounOfCharges--;
    }

    private void GiveCharge(int amount)
    {
        currentAmounOfCharges += amount;
    }


    private void ClearRocks()
    {
        LinkedListNode<Unit> node = Game.Instance.units.First;
        while (node != null)
        {
            Unit val = node.Value;

            if (val is JesusRockV2)
                val.ForceDie();

            node = node.Next;
        }
    }
}

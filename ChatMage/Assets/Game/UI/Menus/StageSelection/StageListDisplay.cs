using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageListDisplay : MonoBehaviour
{

    public StageButton buttonPrefab;
    public Transform countainer;
    public Tower_SpeedRecorder tower_SpeedRecorder;
    public ScrollRect scrollRect;
    public DataSaver datasaver;

    [Header("Debug")]
    public bool useCheat = false;
    public int cheatLevelCount = 25;

    [Header("Animation")]
    public int animMinButtons = 10;
    public float animBaseDuration = 0.35f;
    public float animExtraDurationPerButton = 0.02f;
    public float animDelay = 0.25f;
    public Ease ease = Ease.InOutQuart;

    public int minimumStageUnlocked = 1;

    private int _stageAmountUnlocked;
    private Tween scrollAnim;

    private void Awake()
    {
        tower_SpeedRecorder.enabled = false;
    }

    private int StageUnlocked
    {
        get
        {
            return useCheat ? cheatLevelCount : _stageAmountUnlocked;
        }
        set
        {
            _stageAmountUnlocked = value;
        }
    }

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            LoadInfo();
            SpawnButtons();

            if (StageUnlocked >= animMinButtons)
                this.DelayedCall(AnimateUp, animDelay);
        });
    }

    void LoadInfo()
    {
        StageUnlocked = Mathf.FloorToInt(datasaver.GetInt(LS_EndlessLevel.bestStepKey) / LS_EndlessLevel.stepToResetSave) + 1;
        if (StageUnlocked < minimumStageUnlocked)
            StageUnlocked = minimumStageUnlocked;
        //Debug.Log(datasaver.GetInt(LS_EndlessLevel.bestStepKey));
    }

    void SpawnButtons()
    {
        for (int i = StageUnlocked; i > 0; i--)
        {
            StageButton newButton = Instantiate(buttonPrefab, countainer);
            newButton.SetButtonInfo(i);
        }
        tower_SpeedRecorder.enabled = true;
        scrollRect.verticalNormalizedPosition = 0;
    }

    public void BreakAnim()
    {
        if (scrollAnim != null)
        {
            scrollAnim.Kill();
            scrollAnim = null;
        }
    }

    void AnimateUp()
    {
        scrollAnim = scrollRect.DOVerticalNormalizedPos(1, animBaseDuration + (animExtraDurationPerButton * StageUnlocked)).SetEase(ease);
    }
}

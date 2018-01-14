using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChomp_Sounds : MonoBehaviour
{
    [Header("Links")]
    public Rigidbody2D ball;

    [Header("Chain Rattling")]
    public bool RattleSound = true;
    public AudioSource chainRattle_source;
    public float chainRattle_maxVolume;
    public float chainRattle_minAcceleration = 2;
    public float chainRattle_maxAcceleration = 5;
    public float chainRattle_enterVolumeSpeed = 2;
    public float chainRattle_exitVolumeSpeed = 1;

    private Vector3 ballRelativePos;
    private float ballDistance;
    private float ballRelativeAcceleration;
    private Transform anchoredTransform;
    private bool inDialog = false;

    private void Start()
    {
        var dialogDisplay = Game.instance != null ? Game.instance.ui.dialogDisplay : null;
        dialogDisplay.onStartDialog += OnEnterDialog;
        dialogDisplay.onEndDialog += OnExitDialog;
    }

    private void OnEnterDialog()
    {
        inDialog = true;
    }
    private void OnExitDialog()
    {
        inDialog = false;
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            var dialogDisplay = Game.instance != null ? Game.instance.ui.dialogDisplay : null;
            if (dialogDisplay != null)
            {
                dialogDisplay.onStartDialog -= OnEnterDialog;
                dialogDisplay.onEndDialog -= OnExitDialog;
            }
        }
    }

    void Update()
    {
        UpdateBallRelativePos(Time.unscaledDeltaTime);
        UpdateRattleVolume(Time.unscaledDeltaTime);
    }

    public void SetAnchoredTransform(Transform anchoredTransform)
    {
        this.anchoredTransform = anchoredTransform;
    }

    private void UpdateRattleVolume(float deltaTime)
    {
        float targetVolume = 0;
        if (RattleSound && !inDialog)
        {
            var a = (ballRelativeAcceleration - chainRattle_minAcceleration).Raised(0);
            var b = chainRattle_maxAcceleration - chainRattle_minAcceleration;
            targetVolume = (a / b) * chainRattle_maxVolume;
        }

        var speed = targetVolume > chainRattle_source.volume ? chainRattle_enterVolumeSpeed : chainRattle_exitVolumeSpeed;
        chainRattle_source.volume = chainRattle_source.volume.MovedTowards(targetVolume, speed * deltaTime);
    }

    private void UpdateBallRelativePos(float deltaTime)
    {
        if (anchoredTransform != null)
        {
            var wasPos = ballRelativePos;
            ballRelativePos = anchoredTransform.InverseTransformPoint(ball.position);

            var wasDistance = ballDistance;
            ballDistance = ((Vector2)anchoredTransform.position - ball.position).magnitude;

            ballRelativeAcceleration = (ballDistance - wasDistance).Abs() / deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.Events;

public class JesusIntroAnimation : InGameAnimator
{
    [Header("References")]
    public JesusV2Vehicle jesus;

    [Header("Jesus Awakens")]
    public Dialoguing.Dialog dialog;
    public AudioClip jesusAwakens_clip;
    public float jesusAwakens_delay = 0;
    public float jesusAwakens_volume = 1;

    [Header("Walls")]
    public BrickWallDestruction rightBrickWall;
    public BrickWallDestruction leftBrickWall;

    [Header("Rocks")]
    public JesusRockV2 rightRock;
    public SpriteRenderer rightRockRenderer;
    public Vector2 rightRockDestinationOffset;
    public JesusRockV2 leftRock;
    public SpriteRenderer leftRockRenderer;
    public Vector2 leftRockDestinationOffset;
    public float rockSpeed;
    public string temporaryRockLayer;

    [Header("Top Colliser")]
    public GameObject[] topColliders;

    [Header("Music")]
    public AudioClip bossMusic;


    private bool jesusWokeAF = false;

    private void Start()
    {
        rightRock.gameObject.SetActive(false);
        leftRock.gameObject.SetActive(false);
    }

    private void WakeJesus()
    {
        if (jesusWokeAF)
            return;
        jesusWokeAF = true;

        for (int i = 0; i < topColliders.Length; i++)
        {
            topColliders[i].SetActive(true);
        }

        jesus.enabled = true;
        jesus.ShowHP();
        jesus.GetComponent<AI.JesusV2Brain>().enabled = true;

        SoundManager.PlayMusic(bossMusic);
        jesus.onDeath += (Unit u) =>
        {
            Game.instance.levelScript.Win();
            SoundManager.StopMusic(true);
        };
    }

    private void _AnimationEnd()
    {
        RemoveTimescaleListener();
    }
    private void _LightenJesus()
    {
        jesus.LightenUp();

        int rockFinalLayer = rightRockRenderer.sortingLayerID;

        jesus.animator.Awaken(() =>
        {
            WakeJesus();
            StopPlayer(false);
        }, 
        () =>
        {
            rightRockRenderer.sortingLayerName = temporaryRockLayer;
            rightRock.gameObject.SetActive(true);
            rightRock.flySpeed = rockSpeed * Game.instance.worldTimeScale;
            rightRock.ThrownState_Destination(rightRockDestinationOffset + rightRock.Position);

            rightBrickWall.Animate(() =>
            {
                rightRockRenderer.sortingLayerID = rockFinalLayer;
            });
        }, 
        () =>
        {
            leftRockRenderer.sortingLayerName = temporaryRockLayer;
            leftRock.gameObject.SetActive(true);
            leftRock.flySpeed = rockSpeed * Game.instance.worldTimeScale;
            leftRock.ThrownState_Destination(leftRockDestinationOffset + leftRock.Position);

            leftBrickWall.Animate(() =>
            {
                leftRockRenderer.sortingLayerID = rockFinalLayer;
            });
        });
    }

    private void StopPlayer(bool stop)
    {
        PlayerController player = Game.instance.Player;
        if (player != null)
        {
            const string key = "Jesus";
            if (stop)
            {
                player.vehicle.canMove.Lock(key);
                player.vehicle.canTurn.Lock(key);
            }
            else
            {
                player.vehicle.canMove.Unlock(key);
                player.vehicle.canTurn.Unlock(key);
            }
        }
    }

    public void PlayAnimation()
    {
        AddTimescaleListener();
        SoundManager.StopMusic(true);
        Game.instance.ui.dialogDisplay.StartDialog(dialog, delegate ()
        {
            SoundManager.PlaySFX(jesusAwakens_clip, jesusAwakens_delay, jesusAwakens_volume);
            controller.enabled = true;
            StopPlayer(true);
        });
    }
}

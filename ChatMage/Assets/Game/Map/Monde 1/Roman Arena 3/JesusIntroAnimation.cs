using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

public class JesusIntroAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public JesusV2Vehicle jesus;

    [Header("Jesus Awakens")]
    public Dialoguing.Dialog dialog;
    public AudioClip jesusAwakens_clip;
    public float jesusAwakens_delay = 0;
    public float jesusAwakens_volume = 1;

    [Header("Music")]
    public AudioClip bossMusic;


    private bool jesusWokeAF = false;

    private void WakeJesus()
    {
        if (jesusWokeAF)
            return;
        jesusWokeAF = true;

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
        WakeJesus();
        StopPlayer(false);
    }
    private void _LightenJesus()
    {
        jesus.LightenUp();
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
        SoundManager.StopMusic(true);
        Game.instance.ui.dialogDisplay.StartDialog(dialog, delegate()
        {
            SoundManager.PlaySFX(jesusAwakens_clip, jesusAwakens_delay, jesusAwakens_volume);
            animator.enabled = true;
            StopPlayer(true);
        });
    }
}

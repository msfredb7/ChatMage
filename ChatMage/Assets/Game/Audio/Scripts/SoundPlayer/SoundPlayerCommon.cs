using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerCommon : SoundPlayer
{
    public enum SFXType { hit = 0, death = 1 }
    public SFXType sfxType = SFXType.hit;

    public override void PlaySound()
    {
        switch (sfxType)
        {
            case SFXType.hit:
                Game.Instance.commonSfx.Hit();
                break;
            case SFXType.death:
                Game.Instance.commonSfx.Death();
                break;
            default:
                break;
        }
    }
}

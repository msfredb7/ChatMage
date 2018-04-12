using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;

public class ITM_MagicMushroom : Item, ISpeedBuff
{
    [InspectorHeader("Grow")]
    public AudioPlayable growSFX;
    public float growDelay;
    public float animDuration = 0.75f;
    public float scaleIncrease = 0.5f;
    public float speedbuff;
    public float weigthMultiplier = 1;

    [InspectorHeader("Shrink")]
    public AudioPlayable shrinkSFX;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);

        //On utilise Delay manager et non InGameEvents parce qu'on veut pas que le delai scale avec le ZaWarudo
        Game.Instance.DelayedCall(() =>
        {
            if (player != null)
            {
                DefaultAudioSources.PlaySFX(growSFX);
                player.body.DOBlendableScaleBy(Vector3.one * scaleIncrease, animDuration).SetEase(Ease.OutElastic);
                player.vehicle.speedBuffs.Add(this);
                player.vehicle.weight *= weigthMultiplier;
            }

        }, growDelay);
    }

    public override void Unequip()
    {
        base.Unequip();

        DefaultAudioSources.PlaySFX(shrinkSFX);
        if(player != null)
        {
            player.body.DOBlendableScaleBy(Vector3.one * -scaleIncrease, animDuration).SetEase(Ease.OutElastic);
            player.vehicle.speedBuffs.Remove(this);
            player.vehicle.weight /= weigthMultiplier;
        }
    }

    public override float GetWeight()
    {
        int itemCount = Game.Instance.Player.playerItems.GetCountOf<ITM_MagicMushroom>();

        if (itemCount >= 3)
            return 0;

        return 1;
    }

    public float GetAdditionalSpeed()
    {
        return speedbuff;
    }
}

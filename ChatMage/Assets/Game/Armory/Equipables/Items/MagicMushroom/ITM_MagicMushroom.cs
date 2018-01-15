using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;

public class ITM_MagicMushroom : Item
{
    [InspectorHeader("Grow")]
    public AudioPlayable growSFX;
    public float growDelay;
    public float animDuration = 0.75f;
    public float scaleIncrease = 0.5f;

    [InspectorHeader("Shrink")]
    public AudioPlayable shrinkSFX;

    public override void Equip(int duplicateIndex)
    {
        //On utilise Delay manager et non InGameEvents parce qu'on veut pas que le delai scale avec le ZaWarudo
        DelayManager.LocalCallTo(() =>
        {
            SoundManager.PlaySFX(growSFX);
            player.body.DOBlendableScaleBy(Vector3.one * scaleIncrease, animDuration).SetEase(Ease.OutElastic);
        }, growDelay, Game.Instance);
    }

    public override void Unequip()
    {
        SoundManager.PlaySFX(shrinkSFX);
        player.body.DOBlendableScaleBy(Vector3.one * -scaleIncrease, animDuration).SetEase(Ease.OutElastic);
    }
}

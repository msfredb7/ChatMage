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
        base.Equip(duplicateIndex);

        //On utilise Delay manager et non InGameEvents parce qu'on veut pas que le delai scale avec le ZaWarudo
        Game.Instance.DelayedCall(() =>
        {
            DefaultAudioSources.PlaySFX(growSFX);
            player.body.DOBlendableScaleBy(Vector3.one * scaleIncrease, animDuration).SetEase(Ease.OutElastic);
        }, growDelay);
    }

    public override void Unequip()
    {
        base.Unequip();

        DefaultAudioSources.PlaySFX(shrinkSFX);
        player.body.DOBlendableScaleBy(Vector3.one * -scaleIncrease, animDuration).SetEase(Ease.OutElastic);
    }

    [InspectorHeader("Weight Conditions")]
    public int cantHaveMoreThen = 4;

    public override float GetWeight()
    {
        float ajustedWeight = 1;
        List<Item> playerItems = Game.Instance.Player.playerItems.items;
        int amountOfBlueShellsPlayerHave = 0;
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].GetType() == typeof(ITM_MagicMushroom))
                amountOfBlueShellsPlayerHave++;
        }
        if (amountOfBlueShellsPlayerHave >= cantHaveMoreThen)
            ajustedWeight = 0;
        return (base.GetWeight() * ajustedWeight);
    }
}

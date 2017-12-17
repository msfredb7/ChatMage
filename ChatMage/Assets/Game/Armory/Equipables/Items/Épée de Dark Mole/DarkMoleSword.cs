using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DarkMoleSword : MonoBehaviour
{
    [System.Serializable]
    public struct SwordSet
    {
        public Transform[] positions;
    }

    [Header("Linking")]
    public LaserSword[] swords;
    public SwordSet[] swordSets;

    private PlayerController player;

    public void SetController(PlayerController player)
    {
        this.player = player;
        player.vehicle.onTimeScaleChange += Vehicle_onTimeScaleChange;
        UpdateTimescale(player.vehicle.TimeScale);
    }

    void OnDestroy()
    {
        if (player != null)
            player.vehicle.onTimeScaleChange -= Vehicle_onTimeScaleChange;
    }

    private void Vehicle_onTimeScaleChange(Unit unit)
    {
        UpdateTimescale(unit.TimeScale);
    }
    private void UpdateTimescale(float timescale)
    {
        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].UpdateTimescale(timescale);
        }
    }

    public void OpenSwordSet(int index)
    {
        index = index.Clamped(0, swordSets.Length - 1);

        SwordSet set = swordSets[index];

        for (int i = 0; i < set.positions.Length; i++)
        {
            if (i >= swordSets.Length)
                break;

            Transform tr = swords[i].transform;
            tr.SetParent(set.positions[i], false);
            tr.localScale = Vector3.one;
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            swords[i].OpenSword(null);
        }
    }

    public void CloseSwords(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);

        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].CloseSword(queue.RegisterTween());
        }
        queue.MarkEnd();
    }
}

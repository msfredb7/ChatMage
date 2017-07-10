using UnityEngine;
using System.Collections;

public interface IReward
{
    int GoldAmount();
    string[] EquipableAssetsName();
}

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null);
    float GetSmashJuiceReward();
}
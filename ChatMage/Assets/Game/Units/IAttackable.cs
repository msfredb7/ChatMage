using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    int Attacked(ColliderInfo on, int amount, MonoBehaviour source);
}
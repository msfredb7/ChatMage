
using System;

namespace GameCondition
{
    public abstract class BaseWinningCondition
    {
        public abstract void Init(PlayerController player, LevelScript levelScript);
    }
}
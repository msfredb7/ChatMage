using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dialoguing
{
    [System.Flags]
    public enum SkipFlags
    {
        SkipIfRetry = 1 << 0,
        SkipIfLevelCompleted = 1 << 1
    }
}
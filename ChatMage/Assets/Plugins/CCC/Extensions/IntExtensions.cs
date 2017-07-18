using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntExtensions
{
    public static int Mod(this int value, int modulo)
    {
        if (modulo < 1)
            return 0;

        if (value < 0)
            value = 0;
        else
            return value % modulo;
        return value;
    }
}

﻿using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Permet de mesuré de temps d'éxecution entre une ou plusieurs opérations.
/// </summary>
public class Stopwatch
{
    public enum PrintType { Milliseconds, Ticks}

    System.Diagnostics.Stopwatch stopwatch;
    PrintType printType;

    public Stopwatch(PrintType printType = PrintType.Milliseconds)
    {
        this.printType = printType;
        stopwatch = System.Diagnostics.Stopwatch.StartNew();
    }

    /// <summary>
    /// Print le temps écoulé en milliseconde depuis la construction OU le dernier Print(). Le temps du log en temps que tel n'est pas inclu.
    /// </summary>
    public void Print()
    {
        float deltaTicks = stopwatch.ElapsedTicks;
        switch (printType)
        {
            default:
            case PrintType.Milliseconds:
                UnityEngine.Debug.Log("Exec time(ms): " + (double)deltaTicks / 10000);
                break;
            case PrintType.Ticks:
                UnityEngine.Debug.Log("Exec time(ticks): " + deltaTicks);
                break;
        }
        stopwatch.Reset();
        stopwatch.Start();
    }
}

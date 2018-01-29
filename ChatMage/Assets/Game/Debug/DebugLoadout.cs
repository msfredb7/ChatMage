using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time Drifter/Debug Loadout")]
public class DebugLoadout : ScriptableObject
{
    public Car car;
    public Smash smash;
    public List<Item> items;
    public bool forceUsage = false;
}

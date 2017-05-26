using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : BaseScriptableObject
{
    public string displayName;
    public int levelNumber;
    public bool unlock;
    public LevelScript levelScript;
    public List<Level> nextLevels;
}

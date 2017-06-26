using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : MonoBehaviour
{
    public LevelScript lvl;
    public TestScript child;

    public void Load()
    {
        ResourceLoader.LoadLevelScriptAsync<LevelScript>("LS_demoLevelScript", OnLevelLoaded);
    }
    public void OnLevelLoaded(LevelScript lvl)
    {
        child.StartCoroutine(Routine(child.lvl));
        child.lvl = lvl;
    }

    IEnumerator Routine(LevelScript child)
    {
        yield return null;

        while (true)
        {
            child.sceneName = child.sceneName + "";
            yield return null;
        }
    }

    public void Unload()
    {
        Resources.UnloadUnusedAssets();
    }

    public void Nullify()
    {
        //child.StopAllCoroutines();
        Destroy(child.gameObject);
        child = null;
    } 
}
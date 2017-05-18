using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : BaseBehavior
{
    public EquipablePreview eqPreview;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Equipable eq = ResourceLoader.LoadEquipable(eqPreview.equipableAssetName, eqPreview.type);
            print("name: " + eq.name);
            ResourceLoader.LoadEquipableAsync(eqPreview.equipableAssetName, eqPreview.type, OnResourceLoaded);
        }
    }

    void OnResourceLoaded(Equipable eq)
    {
        print("async name: " + eq.name);
    }
}

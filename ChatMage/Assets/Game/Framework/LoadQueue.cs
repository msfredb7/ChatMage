using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadQueue
{
    public int Count { get { return requestCount; } }
    int requestCount = 0;
    Action onComplete;
    public LoadQueue(Action onComplete)
    {
        this.onComplete = onComplete;
    }
    
    public void AddMiscUnit(string name, Action<Unit> setter)
    {
        requestCount++;
        ResourceLoader.LoadMiscUnitAsync(name, delegate (Unit unit)
        {
            setter(unit);
            requestCount--;
            Check();
        });
    }

    public void AddEnemy(string name, Action<Unit> setter)
    {
        requestCount++;
        ResourceLoader.LoadEnemyAsync(name, delegate (Unit unit)
        {
            setter(unit);
            requestCount--;
            Check();
        });
    }

    public void AddEquipable(string name, EquipableType type, Action<Equipable> setter)
    {
        requestCount++;
        ResourceLoader.LoadEquipableAsync(name, type, delegate (Equipable eq)
        {
            setter(eq);
            requestCount--;
            Check();
        });
    }

    public void AddPrefab(string name, Action<GameObject> setter)
    {
        requestCount++;
        ResourceLoader.LoadPrefabAsync(name,delegate (GameObject obj)
        {
            setter(obj);
            requestCount--;
            Check();
        });
    }

    void Check()
    {
        if (requestCount <= 0 && onComplete != null)
        {
            onComplete();
            onComplete = null;
        }
    }
}

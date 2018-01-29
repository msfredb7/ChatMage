using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using CCC.Persistence;

public class ResourceLoader : MonoPersistent
{
    private abstract class PendingRequest
    {
        public PendingRequest(ResourceRequest request)
        {
            this.request = request;
        }
        public ResourceRequest request;
        public abstract void Callback();
    }
    private class PendingRequest<T> : PendingRequest where T : UnityEngine.Object
    {
        public PendingRequest(ResourceRequest request, Action<T> callback) : base(request)
        {
            this.callback = callback;
        }
        public Action<T> callback;
        public override void Callback()
        {
            callback(request.asset as T);
        }
    }

    private List<PendingRequest> pendingRequests = new List<PendingRequest>();
    private static ResourceLoader instance;

    void Awake()
    {
        instance = this;
    }

    public override void Init(Action onComplete)
    {
        onComplete();
    }

    /// <summary>
    /// Passe à travers la listes des loading en attente, et clear ceux qui sont terminé.
    /// </summary>
    void Update()
    {
        if (pendingRequests.Count > 0)
            for (int i = 0; i < pendingRequests.Count; i++)
            {
                if (pendingRequests[i].request.isDone)
                {
                    pendingRequests[i].Callback();
                    pendingRequests.RemoveAt(i);
                    i--;
                }
            }
    }

    public const string ENEMY = "Units/Enemies/";
    public const string PLAYER = "Units/Player";
    public const string MISC = "Units/Misc/";
    public const string EQUIPABLES = "Equipables/";
    public const string UI = "UI/";
    public const string LVLSCRIPTS = "LevelScripts/";
    public const string LOOT = "Lootbox/";
    public const string TUTORIALS = "Tutorials/";

    static private T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load(path, typeof(T)) as T;
    }
    static private void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync(path, typeof(T));
        if (callback != null)
        {
            PendingRequest<T> pendingRequest = new PendingRequest<T>(request, callback);
            instance.pendingRequests.Add(pendingRequest);
        }
    }

    static public GameObject LoadPrefab(string path)
    {
        return Load<GameObject>(path);
    }
    static public void LoadPrefabAsync(string path, Action<GameObject> callback)
    {
        LoadAsync(path, callback);
    }

    static public Unit LoadEnemy(string name)
    {
        return LoadPrefab(ENEMY + name).GetComponent<Unit>();
    }
    static public void LoadEnemyAsync(string name, Action<Unit> callback)
    {
        LoadPrefabAsync(ENEMY + name, delegate (GameObject obj)
        {
            if (callback != null)
                callback.Invoke(obj.GetComponent<Unit>());
        });
    }

    static public Unit LoadMiscUnit(string name)
    {
        return LoadPrefab(MISC + name).GetComponent<Unit>();
    }
    static public void LoadMiscUnitAsync(string name, Action<Unit> callback)
    {
        LoadPrefabAsync(MISC + name, delegate (GameObject obj)
        {
            if (callback != null)
                callback.Invoke(obj.GetComponent<Unit>());
        });
    }

    static public Vehicle LoadPlayer()
    {
        return LoadPrefab(PLAYER).GetComponent<Vehicle>();
    }
    static public void LoadPlayerAsync(Action<Vehicle> callback)
    {
        LoadPrefabAsync(PLAYER, delegate (GameObject obj)
        {
            if (callback != null)
                callback.Invoke(obj.GetComponent<Vehicle>());
        });
    }

    static private string EquipableTypeToPath(EquipableType type)
    {
        switch (type)
        {
            default:
                throw new Exception("Unable to convert equipableType to path. Unsupported equipable type.");
            case EquipableType.Car:
                return "Cars/";
            case EquipableType.Smash:
                return "Smash/";
            case EquipableType.Item:
                return "Items/";
        }
    }

    static private string EquipablePreviewTypeToPath(EquipableType type)
    {
        switch (type)
        {
            default:
                throw new Exception("Unable to convert equipableType to path. Unsupported equipable type.");
            case EquipableType.Car:
                return "Cars/Previews/";
            case EquipableType.Smash:
                return "Smash/Previews/";
            case EquipableType.Item:
                return "Items/Previews/";
        }
    }

    static public Equipable LoadEquipable(string name, EquipableType type)
    {
        return Load<Equipable>(EQUIPABLES + EquipableTypeToPath(type) + name);
    }
    static public void LoadEquipableAsync(string name, EquipableType type, Action<Equipable> callback)
    {
        LoadAsync(EQUIPABLES + EquipableTypeToPath(type) + name, callback);
    }

    static public Equipable LoadEquipablePreview(string name, EquipableType type)
    {
        return Load<Equipable>(EQUIPABLES + EquipablePreviewTypeToPath(type) + name);
    }
    static public void LoadEquipablePreviewAsync(string name, EquipableType type, Action<EquipablePreview> callback)
    {
        LoadAsync(EQUIPABLES + EquipablePreviewTypeToPath(type) + name, callback);
    }

    static public void LoadUIAsync(string name, Action<GameObject> callback)
    {
        LoadPrefabAsync(UI + name, callback);
    }

    static public void LoadLevelScriptAsync<T>(string name, Action<T> callback)where T : LevelScript
    {
        LoadAsync(LVLSCRIPTS + name, callback);
    }

    static public void LoadLootBoxRefAsync(string name, Action<LootBoxRef> callback)
    {
        LoadAsync(LOOT + name, callback);
    }

    static public void LoadTutorialAsync<T>(string name, Action<T> callback) where T : Tutorial.BaseTutorial
    {
        LoadAsync(TUTORIALS + name, callback);
    }
}

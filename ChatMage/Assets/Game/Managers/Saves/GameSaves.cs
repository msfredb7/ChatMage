using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using System;
using CCC.Utility;
using FullSerializer;
using FullInspector;

public class GameSaves : BaseManager<GameSaves>
{
    [fiInspectorOnly]
    public OpenSavesButton locationButton;
    public int saveVersion = 1;

    [Serializable]
    public class Data
    {
        public Dictionary<string, int> ints = new Dictionary<string, int>();
        public Dictionary<string, float> floats = new Dictionary<string, float>();
        public Dictionary<string, string> strings = new Dictionary<string, string>();
        public Dictionary<string, bool> bools = new Dictionary<string, bool>();
    }

    public override void Init()
    {
        LoadAllAsync(CompleteInit);
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }

    private string GetPath()
    {
        return Application.persistentDataPath + "/v1_";
    }

    #region Get Value
    public int GetInt(Type type, string key)
    {
        Data data = TypeToData(type);
        if (data.ints.ContainsKey(key))
            return data.ints[key];
        else
            throw new Exception(type.ToString() + ": '" + key + "' does not exist.");
    }
    public float GetFloat(Type type, string key)
    {
        Data data = TypeToData(type);
        if (data.floats.ContainsKey(key))
            return data.floats[key];
        else
            throw new Exception(type.ToString() + ": '" + key + "' does not exist.");
    }
    public string GetString(Type type, string key)
    {
        Data data = TypeToData(type);
        if (data.strings.ContainsKey(key))
            return data.strings[key];
        else
            throw new Exception(type.ToString() + ": '" + key + "' does not exist.");
    }
    public bool GetBool(Type type, string key)
    {
        Data data = TypeToData(type);
        if (data.bools.ContainsKey(key))
            return data.bools[key];
        else
            throw new Exception(type.ToString() + ": '" + key + "' does not exist.");
    }
    #endregion

    #region Set Value
    public void SetInt(Type type, string key, int value)
    {
        Data data = TypeToData(type);
        if (data.ints.ContainsKey(key))
            data.ints[key] = value;
        else
            data.ints.Add(key, value);
    }
    public void SetFloat(Type type, string key, float value)
    {
        Data data = TypeToData(type);
        if (data.floats.ContainsKey(key))
            data.floats[key] = value;
        else
            data.floats.Add(key, value);
    }
    public void SetString(Type type, string key, string value)
    {
        Data data = TypeToData(type);
        if (data.strings.ContainsKey(key))
            data.strings[key] = value;
        else
            data.strings.Add(key, value);
    }
    public void SetBool(Type type, string key, bool value)
    {
        Data data = TypeToData(type);
        if (data.bools.ContainsKey(key))
            data.bools[key] = value;
        else
            data.bools.Add(key, value);
    }
    #endregion

    #region Contains ?
    public bool ContainsInt(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.ints.ContainsKey(key);
    }

    public bool ContainsFloat(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.floats.ContainsKey(key);
    }

    public bool ContainsString(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.strings.ContainsKey(key);
    }

    public bool ContainsBool(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.bools.ContainsKey(key);
    }
    #endregion

    #region Save/Load
    public void LoadAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        LoadDataAsync(Type.Loadout, queue.Register());
        LoadDataAsync(Type.World, queue.Register());
        LoadDataAsync(Type.Account, queue.Register());
    }

    public void LoadAll()
    {
        LoadData(Type.Loadout);
        LoadData(Type.World);
        LoadData(Type.Account);
    }

    public void SaveAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        SaveDataAsync(Type.Loadout, queue.Register());
        SaveDataAsync(Type.World, queue.Register());
        SaveDataAsync(Type.Account, queue.Register());
    }

    public void SaveAll()
    {
        SaveData(Type.Loadout);
        SaveData(Type.World);
        SaveData(Type.Account);
    }

    public void LoadDataAsync(Type type, Action onLoadComplete)
    {
        string ext = TypeToFileName(type);
        string path = GetPath() + ext;

        //Exists ?
        if (Saves.Exists(path))
            Saves.ThreadLoad(GetPath() + ext,
                delegate (object graph)
                {
                    ApplyDataByType(type, (Data)graph);
                    if (onLoadComplete != null)
                        onLoadComplete();
                });
        else
        {
            //Nouveau fichier !
            NewOfType(type);
            SaveDataAsync(type, onLoadComplete);
        }
            
    }

    public void LoadData(Type type)
    {
        string ext = TypeToFileName(type);
        string path = GetPath() + ext;

        //Exists ?
        if (Saves.Exists(path))
        {
            object graph = Saves.InstantLoad(GetPath() + ext);
            ApplyDataByType(type, (Data)graph);
        }
        else
        {
            //Nouveau fichier !
            NewOfType(type);
            SaveData(type);
        }
    }

    public void SaveDataAsync(Type type, Action onSaveComplete)
    {
        string ext = TypeToFileName(type);
        Data data = TypeToData(type);

        Saves.ThreadSave(GetPath() + ext, data, onSaveComplete);
    }

    public void SaveData(Type type)
    {
        string ext = TypeToFileName(type);
        Data data = TypeToData(type);
        Saves.InstantSave(GetPath() + ext, data);
    }

    #endregion

    #region ADD NEW CATEGORIES HERE

    private const string WORLD_FILE = "world.dat";
    private const string LOADOUT_FILE = "loadout.dat";
    private const string ACCOUNT_FILE = "account.dat";

    public enum Type { World, Loadout, Account }

    [fsIgnore]
    private Data worldData = new Data();
    [fsIgnore]
    private Data loadoutData = new Data();
    [fsIgnore]
    private Data accountData = new Data();

    private string TypeToFileName(Type type)
    {
        switch (type)
        {
            case Type.World:
                return WORLD_FILE;
            case Type.Loadout:
                return LOADOUT_FILE;
            case Type.Account:
                return ACCOUNT_FILE;
        }
        return "";
    }

    private Data TypeToData(Type type)
    {
        switch (type)
        {
            case Type.World:
                return worldData;
            case Type.Loadout:
                return loadoutData;
            case Type.Account:
                return accountData;
        }
        return null;
    }

    private void ApplyDataByType(Type type, Data newData)
    {
        switch (type)
        {
            case Type.World:
                worldData = newData;
                break;
            case Type.Loadout:
                loadoutData = newData;
                break;
            case Type.Account:
                accountData = newData;
                break;
        }
    }

    private void NewOfType(Type type)
    {
        switch (type)
        {
            case Type.World:
                worldData = new Data();
                break;
            case Type.Loadout:
                loadoutData = new Data();
                break;
            case Type.Account:
                accountData = new Data();
                break;
        }
    }
}
#endregion

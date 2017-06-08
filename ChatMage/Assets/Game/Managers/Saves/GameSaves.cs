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
        Debug.LogWarning("GameSaves: load started");
        //LoadAllAsync(delegate()
        //{
        //    Debug.LogWarning("GameSaves: load complete");
        //});
        LoadAll();
        CompleteInit();
        Debug.LogWarning("GameSaves: load complete");
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
        LoadDataAsync(Type.LevelSelect, queue.Register());
        LoadDataAsync(Type.Account, queue.Register());
        LoadDataAsync(Type.Armory, queue.Register());

        queue.MarkEnd();
    }

    public void LoadAll()
    {
        LoadData(Type.Loadout);
        LoadData(Type.LevelSelect);
        LoadData(Type.Account);
        LoadData(Type.Armory);
    }

    public void SaveAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        SaveDataAsync(Type.Loadout, queue.Register());
        SaveDataAsync(Type.LevelSelect, queue.Register());
        SaveDataAsync(Type.Account, queue.Register());
        SaveDataAsync(Type.Armory, queue.Register());

        queue.MarkEnd();
    }

    [InspectorButton()]
    public void SaveAll()
    {
        SaveData(Type.Loadout);
        SaveData(Type.LevelSelect);
        SaveData(Type.Account);
        SaveData(Type.Armory);
#if UNITY_EDITOR
        Debug.Log("All Data Saved");
#endif
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

    public void ClearAllSaves()
    {
        ClearSave(Type.Account);
        ClearSave(Type.LevelSelect);
        ClearSave(Type.Loadout);
        ClearSave(Type.Armory);
    }

    [InspectorButton()]
    public void ClearLoadout()
    {
        ClearSave(Type.Loadout);
#if UNITY_EDITOR
        Debug.Log("Loadout Cleared");
#endif
    }
    [InspectorButton()]
    public void ClearLevelSelect()
    {
        ClearSave(Type.LevelSelect);
#if UNITY_EDITOR
        Debug.Log("LevelSelect Cleared");
#endif
    }
    [InspectorButton()]
    public void ClearAccount()
    {
        ClearSave(Type.Account);
#if UNITY_EDITOR
        Debug.Log("Account Cleared");
#endif
    }
    [InspectorButton()]
    public void ClearArmory()
    {
        ClearSave(Type.Armory);
#if UNITY_EDITOR
        Debug.Log("Armory Cleared");
#endif
    }

    public void ClearSave(Type type)
    {
        Saves.Delete(GetPath() + TypeToFileName(type));
    }

    #endregion

    #region ADD NEW CATEGORIES HERE

    private const string LEVELSELECT_FILE = "levelSelect.dat";
    private const string LOADOUT_FILE = "loadout.dat";
    private const string ACCOUNT_FILE = "account.dat";
    private const string ARMORY_FILE = "armory.dat";

    public enum Type { LevelSelect = 0, Loadout = 1, Account = 2, Armory = 3 }
    
    [ShowInInspector, InspectorDisabled]
    private Data levelSelectData = new Data();
    [ShowInInspector, InspectorDisabled]
    private Data loadoutData = new Data();
    [ShowInInspector, InspectorDisabled]
    private Data accountData = new Data();
    [ShowInInspector, InspectorDisabled]
    private Data armoryData = new Data();

    private string TypeToFileName(Type type)
    {
        switch (type)
        {
            case Type.LevelSelect:
                return LEVELSELECT_FILE;
            case Type.Loadout:
                return LOADOUT_FILE;
            case Type.Account:
                return ACCOUNT_FILE;
            case Type.Armory:
                return ARMORY_FILE;
        }
        return "";
    }

    private Data TypeToData(Type type)
    {
        switch (type)
        {
            case Type.LevelSelect:
                return levelSelectData;
            case Type.Loadout:
                return loadoutData;
            case Type.Account:
                return accountData;
            case Type.Armory:
                return armoryData;
        }
        return null;
    }

    private void ApplyDataByType(Type type, Data newData)
    {
        switch (type)
        {
            case Type.LevelSelect:
                levelSelectData = newData;
                break;
            case Type.Loadout:
                loadoutData = newData;
                break;
            case Type.Account:
                accountData = newData;
                break;
            case Type.Armory:
                armoryData = newData;
                break;
        }
    }

    private void NewOfType(Type type)
    {
        switch (type)
        {
            case Type.LevelSelect:
                levelSelectData = new Data();
                break;
            case Type.Loadout:
                loadoutData = new Data();
                break;
            case Type.Account:
                accountData = new Data();
                break;
            case Type.Armory:
                armoryData = new Data();
                break;
        }
    }
}
#endregion

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
        LoadAll(CompleteInit);
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
    public void LoadAll(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        LoadData(Type.Loadout, queue.Register());
        LoadData(Type.World, queue.Register());
        //On ajoute les autres aussi
    }

    public void SaveAll(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        SaveData(Type.Loadout, queue.Register());
        SaveData(Type.World, queue.Register());
        //On ajoute les autres aussi
    }

    public void LoadData(Type type, Action onLoadComplete)
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
            SaveData(type, onLoadComplete);
        }
            
    }

    public void SaveData(Type type, Action onSaveComplete)
    {
        string ext = TypeToFileName(type);
        Data data = TypeToData(type);

        Saves.ThreadSave(GetPath() + ext, data, onSaveComplete);
    }

    #endregion

    #region ADD NEW CATEGORIES HERE

    private const string WORLD_FILE = "world.dat";
    private const string LOADOUT_FILE = "loadout.dat";

    public enum Type { World, Loadout }

    [fsIgnore]
    private Data worldData = new Data();
    [fsIgnore]
    private Data loadoutData = new Data();

    private string TypeToFileName(Type type)
    {
        switch (type)
        {
            case Type.World:
                return WORLD_FILE;
            case Type.Loadout:
                return LOADOUT_FILE;
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
        }
    }
}
#endregion

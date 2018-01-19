using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.Utility;
using CCC.Persistence;

[CreateAssetMenu(menuName = "CCC/Other/Data Saver")]
public partial class DataSaver : ScriptablePersistent
{

    public int saveVersion = 1;

    [Serializable]
    private class Data
    {
        public Dictionary<string, int> ints = new Dictionary<string, int>();
        public Dictionary<string, float> floats = new Dictionary<string, float>();
        public Dictionary<string, string> strings = new Dictionary<string, string>();
        public Dictionary<string, bool> bools = new Dictionary<string, bool>();
        public Dictionary<string, object> objects = new Dictionary<string, object>();
    }

    private class Category
    {
        public string fileName;
        public Data data;
        public Category(string fileName)
        {
            this.fileName = fileName;
            data = new Data();
        }
    }


    public static DataSaver instance;

    /// <summary>
    /// Read/Write operation queue. C'est une queue qui assure l'ordonnancement des opérations read/write
    /// </summary>
    private Dictionary<Category, Queue<Action>> rwoQueue = new Dictionary<Category, Queue<Action>>();

    public override void Init(Action onComplete)
    {
        instance = this;
        LoadAll();
        onComplete();
    }

    private string GetPath()
    {
        return Application.persistentDataPath + "/v" + saveVersion + "_";
    }

    private Data TypeToData(Type type)
    {
        return TypeToCategory(type).data;
    }
    private Category TypeToCategory(Type type)
    {
        return categories[(int)type];
    }
    private string TypeToFileName(Type type)
    {
        return TypeToCategory(type).fileName;
    }

    #region Get Value
    public int GetInt(Type type, string key, int defaultVal = 0)
    {
        Data data = TypeToData(type);
        if (data.ints.ContainsKey(key))
            return data.ints[key];
        else
            return defaultVal;
    }
    public float GetFloat(Type type, string key, float defaultVal = 0)
    {
        Data data = TypeToData(type);
        if (data.floats.ContainsKey(key))
            return data.floats[key];
        else
            return defaultVal;
    }
    public string GetString(Type type, string key, string defaultVal = "")
    {
        Data data = TypeToData(type);
        if (data.strings.ContainsKey(key))
            return data.strings[key];
        else
            return defaultVal;
    }
    public bool GetBool(Type type, string key, bool defaultVal = false)
    {
        Data data = TypeToData(type);
        if (data.bools.ContainsKey(key))
            return data.bools[key];
        else
            return defaultVal;
    }
    public object GetObjectClone(Type type, string key, object defaultVal = null)
    {
        Data data = TypeToData(type);
        if (data.objects.ContainsKey(key))
        {
            object result = data.objects[key];
            return result != null ? ObjectCopier.Clone(result) : null;
        }
        else
            return defaultVal;
    }

    public Dictionary<string, int>.KeyCollection GetIntKeys(Type type) { return TypeToData(type).ints.Keys; }
    public Dictionary<string, bool>.KeyCollection GetBoolKeys(Type type) { return TypeToData(type).bools.Keys; }
    public Dictionary<string, float>.KeyCollection GetFloatKeys(Type type) { return TypeToData(type).floats.Keys; }
    public Dictionary<string, string>.KeyCollection GetStringKeys(Type type) { return TypeToData(type).strings.Keys; }
    public Dictionary<string, object>.KeyCollection GetObjectKeys(Type type) { return TypeToData(type).objects.Keys; }
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
    public void SetObjectClone(Type type, string key, object value)
    {
        object clone = value != null ? ObjectCopier.Clone(value) : null;

        Data data = TypeToData(type);
        if (data.objects.ContainsKey(key))
            data.objects[key] = clone;
        else
            data.objects.Add(key, clone);
    }
    #endregion

    #region Delete
    public bool DeleteInt(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.ints.Remove(key);
    }
    public bool DeleteFloat(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.floats.Remove(key);
    }
    public bool DeleteString(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.strings.Remove(key);
    }
    public bool DeleteBool(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.bools.Remove(key);
    }
    public bool DeleteObjectClone(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.objects.Remove(key);
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

    public bool ContainsObject(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.objects.ContainsKey(key);
    }
    #endregion

    #region Save/Load

    #region Load
    public void LoadAll()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            LoadData(categories[i], null);
        }
    }
    public void LoadAll(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        for (int i = 0; i < categories.Length; i++)
        {
            LoadData(categories[i], queue.Register());
        }
        queue.MarkEnd();
    }
    public void LoadAllAsync()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            LoadDataAsync(categories[i], null);
        }
    }
    public void LoadAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        for (int i = 0; i < categories.Length; i++)
        {
            LoadDataAsync(categories[i], queue.Register());
        }
        queue.MarkEnd();
    }

    public void LoadDataAsync(Type type, Action onLoadComplete = null)
    {
        LoadDataAsync(TypeToCategory(type), onLoadComplete);
    }
    private void LoadDataAsync(Category type, Action onLoadComplete)
    {
        AddRWOperation(type, () =>
        {
            string path = GetPath() + type.fileName;

            //Exists ?
            if (Saves.Exists(path))
                Saves.ThreadLoad(path,
                    delegate (object graph)
                    {
                        type.data = (Data)graph;

                        if (onLoadComplete != null)
                            onLoadComplete();

                        CompleteRWOperation(type);
                    });
            else
            {
                //Nouveau fichier !
                type.data = new Data();
                SaveDataAsync(type, onLoadComplete);

                CompleteRWOperation(type);
            }
        });
    }

    public void LoadData(Type type, Action onLoadComplete = null)
    {
        LoadData(TypeToCategory(type), onLoadComplete);
    }
    private void LoadData(Category category, Action onLoadComplete)
    {
        AddRWOperation(category, () =>
        {
            string path = GetPath() + category.fileName;

            //Exists ?
            if (Saves.Exists(path))
            {
                object graph = Saves.InstantLoad(path);
                category.data = (Data)graph;

                if (onLoadComplete != null)
                    onLoadComplete();
            }
            else
            {
                //Nouveau fichier !
                category.data = new Data();
                SaveData(category, onLoadComplete);
            }

            CompleteRWOperation(category);
        });
    }
    #endregion

    #region Save
    public void SaveAll()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            SaveData(categories[i], null);
        }
#if UNITY_EDITOR
        Debug.Log("All Data Saved");
#endif
    }
    public void SaveAll(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        for (int i = 0; i < categories.Length; i++)
        {
            SaveData(categories[i], queue.Register());
        }
        queue.MarkEnd();
    }
    public void SaveAllAsync()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            SaveDataAsync(categories[i], null);
        }
    }
    public void SaveAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        for (int i = 0; i < categories.Length; i++)
        {
            SaveDataAsync(categories[i], queue.Register());
        }
        queue.MarkEnd();
    }

    public void SaveDataAsync(Type type, Action onSaveComplete = null)
    {
        SaveDataAsync(TypeToCategory(type), onSaveComplete);
    }
    private void SaveDataAsync(Category category, Action onSaveComplete)
    {
        AddRWOperation(category, () =>
        {
            Saves.ThreadSave(GetPath() + category.fileName, category.data, () =>
            {
                if (onSaveComplete != null)
                    onSaveComplete();

                CompleteRWOperation(category);
            });
        });
    }

    public void SaveData(Type type, Action onSaveComplete = null)
    {
        SaveData(TypeToCategory(type), onSaveComplete);
    }
    private void SaveData(Category category, Action onSaveComplete)
    {
        AddRWOperation(category, () =>
        {
            Saves.InstantSave(GetPath() + category.fileName, category.data);

            if (onSaveComplete != null)
                onSaveComplete();

            CompleteRWOperation(category);
        });
    }
    #endregion

    #region Clear
    public void ClearAllSaves()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            ClearSave(categories[i], null);
        }
    }
    public void ClearAllSaves(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        for (int i = 0; i < categories.Length; i++)
        {
            ClearSave(categories[i], queue.Register());
        }
        queue.MarkEnd();
    }
    public void ClearSave(Type type, Action onComplete = null)
    {
        ClearSave(TypeToCategory(type), onComplete);
    }
    private void ClearSave(Category category, Action onComplete)
    {
        AddRWOperation(category, () =>
        {
            Saves.Delete(GetPath() + category.fileName);
            category.data = new Data();

            if (onComplete != null)
                onComplete();

            CompleteRWOperation(category);
        });
    }
    #endregion

    #region RW Operations
    private void AddRWOperation(Category category, Action action)
    {
        //S'il y a deja une queue, on s'enfile et on attend
        if (rwoQueue.ContainsKey(category))
        {
            //On s'enfile
            rwoQueue[category].Enqueue(action);
        }
        else
        {
            //On cree la queue et execute l'operation
            rwoQueue.Add(category, new Queue<Action>());
            action();
        }
    }

    private void CompleteRWOperation(Category category)
    {
        if (rwoQueue.ContainsKey(category))
        {
            Queue<Action> q = rwoQueue[category];
            if (q.Count == 0)
            {
                //On est au bout de la file
                rwoQueue.Remove(category);
            }
            else
            {
                //On execute la prochain action
                Action nextOperation = q.Dequeue();
                nextOperation();
            }
        }
        else
        {
            Debug.LogError("Ne devrais pas arriver");
        }
    }
    #endregion

    #endregion
}

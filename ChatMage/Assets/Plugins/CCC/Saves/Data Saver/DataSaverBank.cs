using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;

[CreateAssetMenu(menuName = "CCC/Other/Data Saver Bank")]
public partial class DataSaverBank : ScriptablePersistent
{
    [SerializeField, HideInInspector] private DataSaver[] dataSavers = new DataSaver[Enum.GetValues(typeof(Type)).Length];

    [NonSerialized]
    public static DataSaverBank Instance;

    public override void Init(Action onComplete)
    {
        if (Instance != null)
            Debug.Log("hmm");

        Instance = this;


        InitQueue queue = new InitQueue(onComplete);
        for (int i = 0; i < dataSavers.Length; i++)
        {
            dataSavers[i].Load(queue.Register());
        }
        queue.MarkEnd();
    }

    public void SetDataSaver(Type type, DataSaver newDataSaver)
    {
        dataSavers[(int)type] = newDataSaver;
    }

    public DataSaver GetDataSaver(Type type)
    {
        return dataSavers[(int)type];
    }
    public DataSaver[] GetDataSavers()
    {
        DataSaver[] copy = new DataSaver[dataSavers.Length];
        dataSavers.CopyTo(copy, 0);
        return copy;
    }
}

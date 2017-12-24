using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "CCC/Shared Slots")]
public class SharedTables : ScriptableObject
{
    [SerializeField, Header("Seat Assignment Algorithm")]
    bool spreadClients = true;
    [SerializeField]
    bool assignRandomly = false;

    [SerializeField, Header("-1 == infinite")]
    int seatsPerTable = -1;

    [SerializeField, ReadOnlyInPlayMode]
    int startingTables = 2;

    [System.NonSerialized]
    List<List<object>> tables;

    public bool TakeSeat(object client, out int table, out int seat)
    {
        if (client == null)
            throw new System.Exception("Null Client");

        CheckArray();

        table = -1;
        seat = -1;

        //Doit-on répartir les client de manière égal ?
        if (spreadClients)
        {
            //La quantité de client la plus basse à une table
            int minClientCount = int.MaxValue;
            for (int i = 0; i < tables.Count; i++)
            {
                if (tables[i].Count < minClientCount)
                    minClientCount = tables[i].Count;
            }

            //Tous nos tables sont pleine ?
            if (seatsPerTable >= 0 && minClientCount >= seatsPerTable)
                return false;

            //Assigne-t-on les tables des manière aléatoire ou ordonné ?
            if (assignRandomly)
            {
                //On ajoute tous les tables potentielles dans une liste pour pouvoir piger aléatoirement
                List<int> potentialTables = new List<int>(tables.Count);
                for (int i = 0; i < tables.Count; i++)
                {
                    if (tables[i].Count == minClientCount)
                    {
                        potentialTables.Add(i);
                    }
                }

                table = potentialTables[Random.Range(0, potentialTables.Count)];
            }
            else
            {
                //On cherche la première table avec une quantité minimal de client
                for (int i = 0; i < tables.Count; i++)
                {
                    if (tables[i].Count == minClientCount)
                    {
                        table = i;
                        break;
                    }
                }
            }
        }
        else
        {
            //Assigne-t-on les tables des manière aléatoire ou ordonné ?
            if (assignRandomly)
            {
                //Avons-nous inifie de siege par table ?
                if (seatsPerTable == -1)
                {
                    table = Random.Range(0, tables.Count);
                }
                else
                {
                    int tableCount = tables.Count;
                    int[] tableIndexes = new int[tableCount];

                    for (int i = 0; i < tableIndexes.Length; i++)
                    {
                        tableIndexes[i] = i;
                    }

                    while (tableCount > 0)
                    {
                        int random = Random.Range(0, tableCount);
                        int pickedTable = tableIndexes[random];

                        if (tables[pickedTable].Count < seatsPerTable)
                        {
                            table = pickedTable;
                            break;
                        }
                        else
                        {
                            tableIndexes[random] = tableIndexes[tableCount - 1];
                            tableCount--;
                        }
                    }
                }
            }
            else
            {
                //Avons-nous inifie de siege par table ?
                if (seatsPerTable == -1)
                {
                    table = 0;
                }
                else
                {
                    for (int i = 0; i < tables.Count; i++)
                    {
                        if (tables[i].Count < seatsPerTable)
                        {
                            table = i;
                            break;
                        }
                    }
                }
            }
        }


        if (table == -1)
            return false;

        tables[table].Add(client);
        seat = tables[table].Count - 1;
        return true;
    }

    /// <summary>
    /// Libère le seige occupé par le client. Retourne faux si ce client n'occupait pas de siege.
    /// </summary>
    public bool ReleaseSeat(object client)
    {
        CheckArray();

        for (int i = 0; i < tables.Count; i++)
        {
            if (Internal_RemoveSeatAt(i, client))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Libère tous les seiges occupés par le client. Retourne la quantité de sieges libérés.
    /// </summary>
    public int ReleaseSeats(object client)
    {
        CheckArray();

        int removeCount = 0;

        for (int i = 0; i < tables.Count; i++)
        {
            removeCount += Internal_RemoveSeatsAt(i, client);
        }
        return removeCount;
    }

    /// <summary>
    /// Libère le seige occupé par le client. Retourne faux si ce client n'occupait pas de siege.
    /// </summary>
    public bool ReleaseSeatAt(int table, object client)
    {
        CheckArray();

        if (IsAValidTable(table))
            return Internal_RemoveSeatAt(table, client);

        return false;
    }

    /// <summary>
    /// Libère tous les seiges occupés par le client. Retourne la quantité de sieges libérés.
    /// </summary>
    public int ReleaseSeatsAt(int table, object client)
    {
        CheckArray();

        if (IsAValidTable(table))
            return Internal_RemoveSeatsAt(table, client);

        return 0;
    }

    private int Internal_RemoveSeatsAt(int table, object client)
    {
        int removeCount = 0;
        for (int i = tables[table].Count - 1; i >= 0; i--)
        {
            if (tables[table][i].Equals(client))
            {
                tables[table].RemoveAt(i);
                removeCount++;
            }
        }
        return removeCount;
    }
    private bool Internal_RemoveSeatAt(int table, object client)
    {
        for (int i = tables[table].Count - 1; i >= 0; i--)
        {
            if (tables[table][i].Equals(client))
            {
                tables[table].RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Retourne vrai si la slot n'est pas encore pleine.
    /// </summary>
    public bool IsTableAvailable(int table)
    {
        CheckArray();

        if (IsAValidTable(table))
        {
            return Internal_IsTableAvailable(table);
        }
        else
        {
            return false;
        }
    }

    private bool Internal_IsTableAvailable(int table)
    {
        return tables[table].Count < seatsPerTable || seatsPerTable == -1;
    }

    /// <summary>
    /// Retourne la quantité la liste des client à une table.
    /// </summary>
    public List<object> GetClientsAtTable(int tableIndex)
    {
        CheckArray();

        if (IsAValidTable(tableIndex))
            return tables[tableIndex];
        else
            return null;
    }

    /// <summary>
    /// Retourne la quantité de slot.
    /// </summary>
    public int GetTableCount()
    {
        CheckArray();
        return tables.Count;
    }

    /// <summary>
    /// Retourne la quantité de slot initiale.
    /// </summary>
    public int GetStartingTableCount()
    {
        return startingTables;
    }

    /// <summary>
    /// Set la quantité de table. ATTENTION: Les changements peuvent persisté dans l'éditeur d'une scène à l'autre lorsqu'on reste en Playmode.
    /// Ce comportement ne sera pas répliqué en Build Standalone. Utiliez Reset() dans l'éditeur au besoin.
    /// </summary>
    public void SetTableCount(int amount)
    {
        if (tables == null)
        {
            tables = new List<List<object>>(amount);
        }

        while (tables.Count != amount)
        {
            if (tables.Count < amount)
                tables.Add(new List<object>());
            else
                tables.RemoveLast();
        }
    }

    /// <summary>
    /// Retourne la liste des tables, avec la liste des clients à chaque table
    /// </summary>
    public List<List<object>> GetTables()
    {
        CheckArray();
        return tables;
    }

    /// <summary>
    /// Remet la quantité initiale de table et les libère tous.
    /// </summary>
    public void Reset()
    {
        SetTableCount(startingTables);
        for (int i = 0; i < tables.Count; i++)
        {
            if (tables[i] == null)
                tables[i] = new List<object>();
            else
                tables[i].Clear();
        }
    }

    /// <summary>
    /// Retire tous les clients null. C'est un cas qui ne devrait généralement pas arriver.
    /// </summary>
    public int RemoveNullClients()
    {
        int count = 0;
        for (int i = 0; i < tables.Count; i++)
        {
            for (int j = tables[i].Count - 1; j >= 0; j--)
            {
                if(tables[i][j] == null)
                {
                    tables[i].RemoveAt(j);
                    count++;
                }
            }
        }
        return count;
    }

    private void OnEnable()
    {
        Reset();
    }

    private void CheckArray()
    {
        if (tables == null)
        {
            SetTableCount(startingTables);
        }
    }

    private bool IsAValidTable(int index)
    {
        return index >= 0 && index < GetTableCount();
    }
    private bool IsAValidSeat(int table, int seat)
    {
        return table >= 0 && seat >= 0 && table < GetTableCount() && seat < tables[table].Count;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SharedTables))]
public class SharedSlotsEditor : Editor
{
    bool resetNextLayout = false;

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    public override void OnInspectorGUI()
    {
        SharedTables ss = target as SharedTables;
        base.OnInspectorGUI();

        if (!EditorApplication.isPlaying)
        {
            if (ss.GetTableCount() != ss.GetStartingTableCount() || (Event.current.type == EventType.layout && resetNextLayout))
            {
                resetNextLayout = false;
                ss.Reset();
            }
        }


        EditorGUILayout.Space();

        GUI.enabled = false;
        List<List<object>> tables = ss.GetTables();
        int count = tables.Count;
        for (int i = 0; i < count; i++)
        {
            EditorGUILayout.LabelField("Table " + i, EditorStyles.boldLabel);

            if (tables[i].Count == 0)
            {
                EditorGUILayout.LabelField("     Empty", EditorStyles.miniLabel);
            }
            else
            {
                if(!EditorApplication.isPlaying)
                    resetNextLayout = true;

                for (int j = 0; j < tables[i].Count; j++)
                {
                    object obj = tables[i][j];
                    string name;
                    if (obj == null)
                    {
                        name = "Null";
                    }
                    else
                    {
                        if(obj is Object)
                        {
                            Object unityObj = (Object)obj;
                            name = unityObj == null ? "Null" : unityObj.name;
                        }
                        else
                        {
                            name = obj.ToString();
                        }
                    }
                    EditorGUILayout.LabelField("     Client " + j + ":  " + name);
                }
            }
        }
        GUI.enabled = true;
    }
}
#endif
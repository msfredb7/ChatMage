﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(OLD_DataSaver))]
public class OLD_DataSaverEditor : Editor
{
    public enum DataType { All, Int, Bool, Float, String, Object }
    private const int dataTypeCount = 6;

    OLD_DataSaver gameSaves;
    string[] categoryNames;
    DataType chosenDataType = DataType.All;
    OLD_DataSaver.Type chosenCategory;

    string[] keys;
    int[] keyTypeCounts = new int[dataTypeCount]; // Utilisé pour dessiner le data type All

    bool loadCategory = false;
    bool clearCategory = false;

    void OnEnable()
    {
        gameSaves = target as OLD_DataSaver;

        categoryNames = System.Enum.GetNames(typeof(OLD_DataSaver.Type));

        RefreshKeys();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Save Location"))
        {
            string path = Application.persistentDataPath.Replace('/', '\\');

            if (Directory.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }


        base.OnInspectorGUI();

        if (Event.current.type == EventType.layout)
            ExecuteAwaitingActions();

        EditorGUILayout.Space();

        DrawGlobalUtilityButtons();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Category", EditorStyles.boldLabel);
        DrawCategoryButtons();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Types", EditorStyles.boldLabel);
        DrawDataTypeButtons();

        EditorGUILayout.Space();

        DrawRefreshButton();
        DrawData();

        EditorGUILayout.LabelField("Category Operations", EditorStyles.boldLabel);
        DrawUtilityButtons();

        EditorGUILayout.Space();
    }

    private void RefreshKeys()
    {
        switch (chosenDataType)
        {
            case DataType.All:
                {
                    var intKeys = gameSaves.GetIntKeys(chosenCategory);
                    var boolKeys = gameSaves.GetBoolKeys(chosenCategory);
                    var stringKeys = gameSaves.GetStringKeys(chosenCategory);
                    var floatKeys = gameSaves.GetFloatKeys(chosenCategory);
                    var objectKeys = gameSaves.GetObjectKeys(chosenCategory);

                    keys = new string[intKeys.Count + boolKeys.Count + floatKeys.Count + stringKeys.Count + objectKeys.Count];

                    var current = 0;

                    //All
                    keyTypeCounts[0] = 0;

                    //Ints
                    keyTypeCounts[1] = current;
                    intKeys.CopyTo(keys, current);
                    current += intKeys.Count;

                    //Bools
                    keyTypeCounts[2] = current;
                    boolKeys.CopyTo(keys, current);
                    current += boolKeys.Count;

                    //Floats
                    keyTypeCounts[3] = current;
                    floatKeys.CopyTo(keys, current);
                    current += floatKeys.Count;

                    //Strings
                    keyTypeCounts[4] = current;
                    stringKeys.CopyTo(keys, current);
                    current += stringKeys.Count;

                    //Objects
                    keyTypeCounts[5] = current;
                    objectKeys.CopyTo(keys, current);
                    break;
                }
            case DataType.Int:
                {
                    var newKeys = gameSaves.GetIntKeys(chosenCategory);
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.Float:
                {
                    var newKeys = gameSaves.GetFloatKeys(chosenCategory);
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.String:
                {
                    var newKeys = gameSaves.GetStringKeys(chosenCategory);
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.Object:
                {
                    var newKeys = gameSaves.GetObjectKeys(chosenCategory);
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.Bool:
                {
                    var newKeys = gameSaves.GetBoolKeys(chosenCategory);
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            default:
                break;
        }
    }

    private static Color StandardToSelectedColor(Color normalColor)
    {
        return new Color(normalColor.r * 0.25f, normalColor.g * 1, normalColor.b * 0.35f, normalColor.a);
    }
    private static Color StandardToRefreshColor(Color normalColor)
    {
        return new Color(normalColor.r * 0.8f, normalColor.g * .87f, normalColor.b * 1, normalColor.a);
    }

    private void DrawCategoryButtons()
    {
        var stdColor = GUI.color;
        var selectedColor = StandardToSelectedColor(stdColor);

        int counter = 0;
        while (counter < categoryNames.Length)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < 3; i++)
            {
                if (counter >= categoryNames.Length)
                    break;

                GUI.color = (OLD_DataSaver.Type)counter == chosenCategory ? selectedColor : stdColor;

                if (GUILayout.Button(categoryNames[counter]))
                {
                    chosenCategory = (OLD_DataSaver.Type)counter;
                    RefreshKeys();
                }

                counter++;
            }
            EditorGUILayout.EndHorizontal();
        }

        GUI.color = stdColor;
    }

    private void DrawDataTypeButtons()
    {
        var stdColor = GUI.color;
        var selectedColor = StandardToSelectedColor(stdColor);

        bool refreshKeys = false;

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < dataTypeCount; i++)
        {
            var dataType = (DataType)i;
            GUI.color = dataType == chosenDataType ? selectedColor : stdColor;

            if (GUILayout.Button(dataType.ToString()))
            {
                chosenDataType = dataType;
                refreshKeys = true;
            }
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = stdColor;

        if (refreshKeys)
            RefreshKeys();

    }

    private void DrawRefreshButton()
    {
        var guiColor = GUI.color;
        GUI.color = StandardToRefreshColor(guiColor);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Refresh"))
        {
            RefreshKeys();
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = guiColor;
    }

    private void DrawData()
    {
        OneWayBool refresh = new OneWayBool(false);

        if (chosenDataType == DataType.All)
        {
            if (keys == null)
            {
                for (int i = 1; i < dataTypeCount; i++)
                {
                    var dataType = (DataType)i;
                    refresh.TryToSet(!DrawData(dataType, -1, -1));
                }
            }
            else
            {
                for (int i = 1; i < dataTypeCount; i++)
                {
                    var dataType = (DataType)i;
                    var begin = keyTypeCounts[i];
                    var end = (i + 1) >= dataTypeCount ? keys.Length : keyTypeCounts[i + 1];

                    refresh.TryToSet(!DrawData(dataType, begin, end));
                }
            }
        }
        else
        {
            refresh.TryToSet(!DrawData(chosenDataType, 0, keys != null ? keys.Length : 0));
        }

        if (refresh)
            RefreshKeys();
    }

    /// <summary>
    /// Retourne Faux si une key n'existait pas
    /// </summary>
    private bool DrawData(DataType type, int keysStart, int keysEnd)
    {
        OneWayBool allKeyExist = new OneWayBool(true);

        EditorGUILayout.BeginHorizontal();


        // + Button
        if (IsDataTypeModifiable(type))
        {
            var p = GUI.skin.button.padding;
            var o = GUI.skin.button.contentOffset;
            RectOffset wasPadding = new RectOffset(p.left, p.right, p.top, p.bottom);
            Vector2 wasContentOffset = new Vector2(o.x, o.y);

            GUI.skin.button.padding = new RectOffset(0, 0, 0, 0);
            GUI.skin.button.contentOffset = new Vector2(0, -1);

            if (GUILayout.Button("+", GUILayout.Width(16), GUILayout.Height(16)))
            {
                var screenPoint = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                OLD_DataSaverEditorPopup.Popup(gameSaves, chosenCategory, type, screenPoint, RefreshKeys);
            }
            GUI.skin.button.padding = wasPadding;
            GUI.skin.button.contentOffset = wasContentOffset;
        }

        // Label
        EditorGUILayout.LabelField(type.ToString(), EditorStyles.boldLabel);

        EditorGUILayout.EndHorizontal();


        // Data
        if (keysStart == keysEnd) // If length == 0
        {
            EditorGUILayout.LabelField("Empty", EditorStyles.centeredGreyMiniLabel);
        }
        else
        {
            for (int i = keysStart; i < keysEnd; i++)
            {
                allKeyExist.TryToSet(DrawData(type, keys[i]));
            }
        }
        EditorGUILayout.Space();
        return allKeyExist;
    }

    private static bool IsDataTypeModifiable(DataType type)
    {
        switch (type)
        {
            default:
            case DataType.Object:
            case DataType.All:
                return false;
            case DataType.Int:
            case DataType.Bool:
            case DataType.Float:
            case DataType.String:
                return true;
        }
    }

    /// <summary>
    /// Retourne Faux si la key n'existait pas
    /// </summary>
    private bool DrawData(DataType type, string key)
    {
        EditorGUILayout.BeginHorizontal();

        var deleteKey = GUILayout.Button("X", GUILayout.Height(16), GUILayout.Width(16));

        bool keyExists = true;
        switch (type)
        {
            case DataType.Int:
                {
                    if (deleteKey)
                        gameSaves.DeleteInt(chosenCategory, key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.DelayedIntField(key, gameSaves.GetInt(chosenCategory, key));
                    keyExists = gameSaves.ContainsInt(chosenCategory, key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetInt(chosenCategory, key, newValue);

                    break;
                }

            case DataType.Float:
                {
                    if (deleteKey)
                        gameSaves.DeleteFloat(chosenCategory, key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.DelayedFloatField(key, gameSaves.GetFloat(chosenCategory, key));
                    keyExists = gameSaves.ContainsFloat(chosenCategory, key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetFloat(chosenCategory, key, newValue);
                    break;
                }

            case DataType.String:
                {
                    if (deleteKey)
                        gameSaves.DeleteString(chosenCategory, key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.DelayedTextField(key, gameSaves.GetString(chosenCategory, key));
                    keyExists = gameSaves.ContainsString(chosenCategory, key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetString(chosenCategory, key, newValue);
                    break;
                }

            case DataType.Bool:
                {
                    if (deleteKey)
                        gameSaves.DeleteBool(chosenCategory, key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.Toggle(key, gameSaves.GetBool(chosenCategory, key));
                    keyExists = gameSaves.ContainsBool(chosenCategory, key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetBool(chosenCategory, key, newValue);
                    break;
                }

            case DataType.Object:
                {
                    if (deleteKey)
                        gameSaves.DeleteObjectClone(chosenCategory, key);

                    var obj = gameSaves.GetObjectClone(chosenCategory, key);
                    var text = key + ": " + (obj == null ? "null" : obj.GetType().ToString());
                    EditorGUILayout.LabelField(text);
                    keyExists = gameSaves.ContainsObject(chosenCategory, key);

                    break;
                }
            default:
                EditorGUILayout.LabelField("Error type", EditorStyles.whiteBoldLabel);
                break;
        }

        EditorGUILayout.EndHorizontal();

        return keyExists;
    }

    private void DrawUtilityButtons()
    {
        var originalGUIColor = GUI.color;
        GUI.color = Color.Lerp(StandardToSelectedColor(originalGUIColor), originalGUIColor, 0);


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            gameSaves.SaveData(chosenCategory);
        }

        if (GUILayout.Button("Load"))
        {
            loadCategory = true;
        }

        if (GUILayout.Button("Clear Save"))
        {
            clearCategory = true;
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = originalGUIColor;
    }
    private void DrawGlobalUtilityButtons()
    {
        if (GUILayout.Button("Save All"))
        {
            gameSaves.SaveAll();
        }

        if (GUILayout.Button("Load All"))
        {
            gameSaves.LoadAll();
        }

        if (GUILayout.Button("Clear All Saves"))
        {
            gameSaves.ClearAllSaves();
        }
    }

    private void ExecuteAwaitingActions()
    {
        if (clearCategory)
        {
            gameSaves.ClearSave(chosenCategory);
            clearCategory = false;
            RefreshKeys();
        }
        if (loadCategory)
        {
            gameSaves.LoadData(chosenCategory);
            loadCategory = false;
            RefreshKeys();
        }
    }
}

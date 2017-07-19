using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectCreator : MonoBehaviour
{
    [MenuItem("GameObject/Time Drifter/Smart Surface", priority = -1)]
    private static void CreateSmartSurface(MenuCommand command)
    {
        GameObject obj = new GameObject("Smart Surface");
        obj.AddComponent<EdgeCollider2D>();
        obj.AddComponent<NAV_SmartSurface>();
        obj.layer = Layers.NAVIGATION;
        obj.isStatic = true;

        AdjustGameobject(obj, command);
    }

    [MenuItem("GameObject/Time Drifter/Smart Wall", priority = -1)]
    private static void CreateSmartWall(MenuCommand command)
    {
        GameObject obj = new GameObject("Smart Wall");
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<NAV_SmartWall>();
        obj.layer = Layers.NAVIGATION;
        obj.isStatic = true;

        AdjustGameobject(obj, command);
    }

    private static void AdjustGameobject(GameObject obj, MenuCommand command, Vector3 localScale)
    {
        Selection.activeGameObject = obj;

        //Set Parent
        if (command != null && command.context != null && command.context.GetType() == typeof(GameObject))
            obj.transform.SetParent((command.context as GameObject).transform);

        obj.transform.localPosition = Vector2.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = localScale;

    }

    private static void AdjustGameobject(GameObject obj, MenuCommand command)
    {
        AdjustGameobject(obj, command, Vector3.one);
    }
}

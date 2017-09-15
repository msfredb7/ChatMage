using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectCreator : MonoBehaviour
{
    [MenuItem("GameObject/Time Drifter/Smart Surface(AI)", priority = -1)]
    private static void CreateSmartSurface(MenuCommand command)
    {
        GameObject obj = new GameObject("Smart Surface");
        obj.AddComponent<EdgeCollider2D>();
        obj.AddComponent<NAV_SmartSurface>();
        obj.layer = Layers.NAVIGATION;
        obj.isStatic = true;

        AdjustGameobject(obj, command);
    }

    [MenuItem("GameObject/Time Drifter/Smart Wall(AI)", priority = -1)]
    private static void CreateSmartWall(MenuCommand command)
    {
        GameObject obj = new GameObject("Smart Wall");
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<NAV_SmartWall>();
        obj.layer = Layers.NAVIGATION;
        obj.isStatic = true;

        AdjustGameobject(obj, command);
    }

    [MenuItem("GameObject/Time Drifter/Position Displacer", priority = -1)]
    private static void CreatePositionDisplacer(MenuCommand command)
    {
        GameObject obj = new GameObject("Position Displacer");
        obj.AddComponent<Box>();
        obj.AddComponent<PositionDisplacer>();
        obj.isStatic = true;

        AdjustGameobject(obj, command);
    }

    [MenuItem("GameObject/Time Drifter/Milestone", priority = -1)]
    private static void CreateMilestone(MenuCommand command)
    {
        GameObject obj = new GameObject("Milestone");
        obj.AddComponent<Milestone>();
        obj.isStatic = true;
        obj.tag = "Milestone";

        AdjustGameobject(obj, command);

        obj.transform.localPosition = new Vector3(0, obj.transform.localPosition.y, 0);
    }

    [MenuItem("GameObject/Time Drifter/Two Way Milestone", priority = -1)]
    private static void CreateTwoWayMilestone(MenuCommand command)
    {
        GameObject obj = new GameObject("Two Way Milestone");
        obj.AddComponent<GameEvents.TwoWayMilestone>();
        obj.isStatic = true;
        obj.tag = "Milestone";

        AdjustGameobject(obj, command);

        obj.transform.localPosition = new Vector3(0, obj.transform.localPosition.y, 0);
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

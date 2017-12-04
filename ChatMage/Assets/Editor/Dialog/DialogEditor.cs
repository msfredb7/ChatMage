using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using Dialoguing;
using UnityEditor;

[CustomBehaviorEditor(typeof(Dialog))]
public class DialogEditor : DefaultBehaviorEditor<Dialog>
{
    static GUIStyle labels;


    protected override void OnEdit(Rect rect, Dialog dialog, fiGraphMetadata metadata)
    {
        if (labels == null)
            labels = new GUIStyle(EditorStyles.label);
        labels.alignment = TextAnchor.MiddleRight;

        string ID = dialog.ID.ToString();
        int world;
        int level;
        int dialogIndex;

        try
        {
            if(ID.Length == 5)
            {
                world = int.Parse(ID.Substring(0, 1));
                level = int.Parse(ID.Substring(1, 2));
                dialogIndex = int.Parse(ID.Substring(3, 2));
            }
            else
            {
                world = int.Parse(ID.Substring(0, 2));
                level = int.Parse(ID.Substring(2, 2));
                dialogIndex = int.Parse(ID.Substring(4, 2));
            }
        }
        catch
        {
            world = 99;
            level = 99;
            dialogIndex = 99;
        }

        float height = 16;
        float numberWidth = 28;
        Rect fieldRect = new Rect(rect.x, rect.yMin, rect.width, height);

        Rect thirdRect = new Rect(fieldRect);
        thirdRect.width = thirdRect.width / 3 - numberWidth;

        Rect numberRect = new Rect(thirdRect)
        {
            width = numberWidth,
            x = thirdRect.xMax
        };


        EditorGUI.LabelField(thirdRect, "World", labels);
        world = Mathf.Clamp(EditorGUI.IntField(numberRect, world), 1, 99);

        thirdRect.x += thirdRect.width;
        numberRect.x += thirdRect.width;
        EditorGUI.LabelField(thirdRect, "Level", labels);
        level = Mathf.Clamp(EditorGUI.IntField(numberRect, level), 1, 99);

        thirdRect.x += thirdRect.width;
        numberRect.x += thirdRect.width;
        EditorGUI.LabelField(thirdRect, "Dialog", labels);
        dialogIndex = Mathf.Clamp(EditorGUI.IntField(numberRect, dialogIndex), 1, 99);


        dialog.ID = dialogIndex + level * 100 + world * 10000;


        rect.y += height + 3;
        base.OnEdit(rect, dialog, metadata);
    }

    protected override void OnBeforeEdit(Rect rect, Dialog behavior, fiGraphMetadata metadata)
    {

        base.OnBeforeEdit(rect, behavior, metadata);
    }
}
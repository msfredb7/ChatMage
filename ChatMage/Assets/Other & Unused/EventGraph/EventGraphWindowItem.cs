using UnityEditor;
using System;
using UnityEngine;

public class EventGraphWindowItem
{
    private const string DRAG_KEY = "egwidr"; //Pour Event Graph Window Item DRag
    public INodeDisplay myEvent;
    public Editor editor;

    Action<EventGraphWindowItem> removeRequest;
    bool isVirtual;

    public EventGraphWindowItem(INodeDisplay myEvent, Action<EventGraphWindowItem> removeRequest)
    {
        this.myEvent = myEvent;
        if (myEvent == null)
            throw new Exception("my event == null");
        editor = Editor.CreateEditor(myEvent.AsObject());
        this.removeRequest = removeRequest;

        isVirtual = (myEvent.AsObject() is BaseVirtualEvent) ? true : false;
    }

    public Rect WindowRect
    {
        get
        {
            return myEvent.WindowRect;
        }
        set
        {
            myEvent.WindowRect = value;
        }
    }

    public void MoveToPos(Vector2 position)
    {
        Rect rect = myEvent.WindowRect;
        rect.position = position;
        myEvent.WindowRect = rect;
    }

    public string NodeLabel
    {
        get { return myEvent.name; }
    }

    public void DrawDetails()
    {
        editor.DrawDefaultInspector();
    }

    public void DrawNode(int unusedWindowId)
    {
        Event e = Event.current;
        
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(isVirtual ? "Virtual" : "Physical", EditorStyles.miniLabel);

        //Label  +  x button
        GUILayout.FlexibleSpace();
        if (myEvent.CanBeManuallyDestroyed() && GUILayout.Button("x"))
        {
            removeRequest(this);
            return;
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label(myEvent.DefaultLabel(), EditorStyles.boldLabel);


        //---------------Reference Box---------------//

        EditorGUILayout.ObjectField(myEvent.AsObject(), myEvent.AsObject().GetType(), true);
        if (GUILayoutUtility.GetLastRect().Contains(e.mousePosition) && e.type == EventType.MouseDrag)
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new UnityEngine.Object[] { myEvent.AsObject() };
            DragAndDrop.StartDrag(DRAG_KEY);
            e.Use();
        }


        //---------------Drag---------------//

        if (e.button != 1)
            GUI.DragWindow();
    }

    public void OpenContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Reset Rect Size"), false, myEvent.ResetWindowRectSize);
        menu.AddItem(new GUIContent("Reset Rect Position"), false, myEvent.ResetWindowRectPos);
        menu.ShowAsContext();
    }
}

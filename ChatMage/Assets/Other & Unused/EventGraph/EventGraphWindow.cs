using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

public class EventGraphWindow : EditorWindow
{
    public GameObject gameobject;

    private EventGraph graph;

    private List<EventGraphWindowItem> items;
    private EventGraphWindowItem selectedItem;
    private Vector2 contextMenuMousePos;

    [MenuItem("The Time Drifter/Event Graph")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EventGraphWindow window = (EventGraphWindow)EditorWindow.GetWindow(typeof(EventGraphWindow));
        window.Show();
    }

    public void Awake()
    {
        SetScene(EditorSceneManager.GetActiveScene());
        EditorSceneManager.activeSceneChanged += EditorSceneManager_activeSceneChanged;
    }

    void OnDestroy()
    {
        EditorSceneManager.activeSceneChanged -= EditorSceneManager_activeSceneChanged;
        if (graph != null)
            graph.onEventsAddedOrRemoved -= RebuildItems;
    }

    private void EditorSceneManager_activeSceneChanged(Scene previousScene, Scene newScene)
    {
        SetScene(newScene);
    }

    void SetScene(Scene scene)
    {
        EventGraph newGraph = scene.FindRootObject<EventGraph>();
        if (newGraph == null)
            Debug.Log("Il n'y a pas de EventGraph dans la scène " + scene.name + ". (Il doit être sur un gameobject racine)");
        SetGraph(newGraph);
    }

    void SetGraph(EventGraph newGraph)
    {
        Debug.Log("Rebuilding graph");

        //On le met vrm a null pour ne pas faire d'erreur avec les Object detruit
        if (graph == null)
            graph = null;

        //Remove previous listeners
        if (graph != null)
            graph.onEventsAddedOrRemoved -= RebuildItems;

        //Assign new graph
        graph = newGraph;

        if (graph == null)
        {
            ClearItems();
        }
        else
        {
            graph.onEventsAddedOrRemoved += RebuildItems;
            RebuildItems();
        }
    }

    void ClearItems()
    {
        selectedItem = null;
        items = null;
        Repaint();
    }

    void RebuildItems()
    {
        Debug.Log("Rebuilding items");

        selectedItem = null;

        items = new List<EventGraphWindowItem>(graph.virtualEvents.Count + graph.physicalEvents.Count);
        for (int i = 0; i < graph.virtualEvents.Count; i++)
        {
            NewItem(graph.virtualEvents[i]);
        }
        for (int i = 0; i < graph.physicalEvents.Count; i++)
        {
            NewItem(graph.physicalEvents[i]);
        }

        Repaint();
    }

    EventGraphWindowItem NewItem(INodeDisplay theEvent)
    {
        EventGraphWindowItem newItem = new EventGraphWindowItem(theEvent, RemoveEvent);
        items.Add(newItem);
        return newItem;
    }

    void MarkSceneAsDirty()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    public Rect graphInfoRect = new Rect(0, 0, 200, 10);
    public Rect nodeDetailsRect = new Rect(0, 0, 200, 10);

    void OnGUI()
    {
        Event e = Event.current;
        EventType eventPastType = e.type;

        //Debug.Log(e.type);

        //On ouvre le minimenu
        if (e.type == EventType.ContextClick)
        {
            EventGraphWindowItem item = GetItemOnMousePosition(e.mousePosition);
            if (item != null)
            {
                item.OpenContextMenu();
                e.Use();
                return;
            }
            else
            {
                if (!graphInfoRect.Contains(e.mousePosition) && !nodeDetailsRect.Contains(e.mousePosition))
                {
                    contextMenuMousePos = e.mousePosition;
                    e.Use();
                    OpenContextMenu();
                }
            }
        }
        //On selectionne l'item
        else if (e.type == EventType.MouseDown)
        {
            EventGraphWindowItem newSelection = GetItemOnMousePosition(e.mousePosition);
            if (newSelection == null)
            {
                if (selectedItem != null && !graphInfoRect.Contains(e.mousePosition) && !nodeDetailsRect.Contains(e.mousePosition))
                {
                    selectedItem = null;
                    ClearFocus();
                    Repaint();
                }
            }
            else
            {
                if (selectedItem != newSelection)
                {
                    selectedItem = newSelection;
                    Repaint();
                }
                ClearFocus();
            }
        }

        // The position of the window
        BeginWindows();

        //Window Count
        int winC = 0;

        //Graph info
        graphInfoRect = GUILayout.Window(winC++, graphInfoRect, Window_GraphInfo, "Graph Info");

        //Node info (inspecteur a gauche)
        nodeDetailsRect.min = new Vector2(nodeDetailsRect.xMin, graphInfoRect.yMax + 5);
        nodeDetailsRect = GUILayout.Window(winC++, nodeDetailsRect, Window_NodeDetails, "Event Info");

        //Items !
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].myEvent == null || items[i].myEvent.AsObject() == null)
                {
                    graph.RemoveNulls();
                    items = null;
                    break;
                }
                GUI.color = items[i].myEvent.DefaultColor();
                items[i].WindowRect = GUILayout.Window(winC++, items[i].WindowRect, items[i].DrawNode, items[i].NodeLabel);
            }
            GUI.color = Color.white;
        }
        else if (eventPastType == EventType.repaint)
        {
            //Rebuild items ?
            if (graph != null)
            {
                SetGraph(graph);
            }
        }

        EndWindows();

    }

    void ClearFocus()
    {
        GUI.SetNextControlName("");
        GUI.FocusControl("");
    }

    EventGraphWindowItem GetItemOnMousePosition(Vector2 mousePosition)
    {
        if (items != null)
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].WindowRect.Contains(mousePosition))
                    return items[i];
            }

        return null;
    }

    void OpenContextMenu()
    {
        if (graph == null)
            return;

        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("New Empty Event"), false, NewVirtualEvent<BaseVirtualEvent>);
        menu.AddItem(new GUIContent("New Test Event"), false, NewVirtualEvent<TestVirtualEvent>);

        menu.ShowAsContext();
    }

    void NewVirtualEvent<T>() where T : BaseVirtualEvent
    {
        Rect popupRect = new Rect(contextMenuMousePos + Vector2.right * 100, new Vector2(210, 90));
        try
        {
            PopupWindow.Show(popupRect, new EventNamePopup(graph.IsVirtualNameAvailable,
                delegate (string name)
                {
                    PopupWindow.focusedWindow.Close();
                    NewVirtualEvent<T>(name);
                }));
        }
        catch { }
    }

    void NewVirtualEvent<T>(string name) where T : BaseVirtualEvent
    {
        Debug.Log("New event of type " + typeof(T) + ": " + name);
        
        graph.CreateAndAddVirtualEvent<T>(name, contextMenuMousePos);
        MarkSceneAsDirty();
    }

    void RemoveEvent(EventGraphWindowItem item)
    {
        Rect popupRect = new Rect(Vector2.zero, new Vector2(210, 65));
        try
        {
            PopupWindow.Show(popupRect, new EventRemovePopup(
                delegate ()
                {
                    Debug.Log("Remove: " + item.myEvent.name);

                    PopupWindow.focusedWindow.Close();

                    UnityEngine.Object obj = item.myEvent.AsObject();
                    if (obj is BaseVirtualEvent)
                        graph.RemoveVirtualEvent(obj as BaseVirtualEvent);
                    //else if (obj is BasePhysicalEvent)
                    //    graph.RemoveEvent(obj as BasePhysicalEvent);
                    else
                        Debug.LogError("Removing a graph item that holds neither BaseVirtualEvent nor BasePhysicalEvent");

                    MarkSceneAsDirty();
                }));
        }
        catch
        {
        }
    }

    void Window_GraphInfo(int unusedWindowID)
    {
        if (graph == null)
        {
            GUILayout.Label("No graph to edit.");
            if (GUILayout.Button("Refresh"))
                SetScene(EditorSceneManager.GetActiveScene());
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Graph nodes: " + graph.virtualEvents.Count);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Rebuild"))
            {
                SetGraph(graph);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void Window_NodeDetails(int unusedWindowID)
    {
        if (selectedItem != null && selectedItem.myEvent == null)
            selectedItem = null;

        if (selectedItem != null)
            selectedItem.DrawDetails();
        else
            GUILayout.Label("No event to edit.");
    }
}

public class EventNamePopup : PopupWindowContent
{
    string name = "";
    Func<string, bool> nameVerifier;
    string error = "";
    Action<string> onSuccess;

    public override Vector2 GetWindowSize()
    {
        return new Vector2(210, 90);
    }

    public EventNamePopup(Func<string, bool> nameVerifier, Action<string> onSuccess)
    {
        this.nameVerifier = nameVerifier;
        this.onSuccess = onSuccess;


    }
    public override void OnGUI(Rect rect)
    {
        GUILayout.Label("Event Name", EditorStyles.boldLabel);

        GUI.SetNextControlName("NameField");
        name = EditorGUILayout.TextField(name);
        EditorGUI.FocusTextInControl("NameField");

        if (GUILayout.Button("Submit") || (Event.current.isKey && Event.current.keyCode == KeyCode.Return))
        {
            if (name == "")
            {
                error = "Invalid name";
            }
            else if (nameVerifier(name))
            {
                onSuccess(name);
            }
            else
            {
                error = "Name taken by another event";
            }
        }

        if (error != "")
            GUILayout.Label(error, EditorStyles.whiteBoldLabel);
    }
}
public class EventRemovePopup : PopupWindowContent
{
    Action onConfirm;

    public override Vector2 GetWindowSize()
    {
        return new Vector2(210, 65);
    }

    public EventRemovePopup(Action onConfirm)
    {
        this.onConfirm = onConfirm;
    }
    public override void OnGUI(Rect rect)
    {
        GUILayout.Label("The event will be deleted", EditorStyles.label);
        GUILayout.Label("This action cannot be undone", EditorStyles.boldLabel);

        if (GUILayout.Button("Confirm"))
        {
            onConfirm();
        }
    }
}

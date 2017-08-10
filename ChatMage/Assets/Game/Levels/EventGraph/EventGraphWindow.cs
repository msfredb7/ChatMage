using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

public class EventGraphWindow : EditorWindow
{
    public EventGraph graph;
    public OngoingLink ongoingLink;

    private List<EventGraphWindowItem> items;
    private EventGraphWindowItem selectedItem;
    private Vector2 contextMenuMousePos;
    private Vector2 lastMousePos;

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

    void Update()
    {
        if (ongoingLink.CanDraw)
            Repaint();
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

    void ClearItems(bool repaint = true)
    {
        selectedItem = null;
        ongoingLink.source = null;
        items = null;
        ResetNodeDetailsPanel();
        Repaint();
    }

    void RebuildItems()
    {
        selectedItem = null;
        ongoingLink.source = null;
        ResetNodeDetailsPanel();

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

    EventGraphWindowItem NewItem(IEventDisplay theEvent)
    {
        EventGraphWindowItem newItem = new EventGraphWindowItem(theEvent, RemoveItem, this);
        items.Add(newItem);
        return newItem;
    }

    void MarkSceneAsDirty()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    public Rect graphInfoRect = new Rect(0, 0, 240, 10);
    public Rect nodeDetailsRect = new Rect(0, 0, DEFAULT_PANEL_WIDTH, DEFAULT_NDP_HEIGHT);
    private const float DEFAULT_PANEL_WIDTH = 240;
    private const float DEFAULT_NDP_HEIGHT = 50;

    private void ResetNodeDetailsPanel()
    {
        nodeDetailsRect = new Rect(0, 0, DEFAULT_PANEL_WIDTH, DEFAULT_NDP_HEIGHT);
    }

    void OnGUI()
    {
        Event e = Event.current;
        EventType eventPastType = e.type;

        lastMousePos = e.mousePosition;
        //Debug.Log(e.type);

        //On ouvre le minimenu
        if (e.type == EventType.ContextClick)
        {
            EventGraphWindowItem item = GetItemOnMousePosition(lastMousePos);
            if (item != null)
            {
                item.OpenContextMenu();
                e.Use();
                return;
            }
            else
            {
                if (!graphInfoRect.Contains(lastMousePos) && !nodeDetailsRect.Contains(lastMousePos))
                {
                    contextMenuMousePos = lastMousePos;
                    e.Use();
                    OpenContextMenu();
                }
            }
        }
        //On selectionne l'item
        else if (e.type == EventType.MouseDown)
        {
            EventGraphWindowItem newSelection = GetItemOnMousePosition(lastMousePos);
            if (newSelection == null)
            {
                //Cancel ongoing moment link
                if (ongoingLink.source != null && e.button == 0)
                {
                    ongoingLink.source = null;
                }

                //Unselect the previous item
                if (selectedItem != null && !graphInfoRect.Contains(lastMousePos) && !nodeDetailsRect.Contains(lastMousePos))
                {
                    selectedItem = null;
                    ClearFocus();
                    ResetNodeDetailsPanel();
                    Repaint();
                }
            }
            else
            {
                //Create moment link !
                if (ongoingLink.source != null && e.button == 0)
                {
                    ongoingLink.BuildLink(newSelection);
                    MarkSceneAsDirty();
                }

                //Change selected item
                if (selectedItem != newSelection)
                {
                    selectedItem = newSelection;
                    ResetNodeDetailsPanel();
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
                    ClearItems(false);
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


        //----------------Links----------------//

        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].DrawLinks();
            }

            ongoingLink.Draw(lastMousePos);
        }
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
        menu.AddItem(new GUIContent("Delayed Moment"), false, NewDelayedMoment);

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

    void NewDelayedMoment()
    {
        NewVirtualEvent<DelayedMoment>("Delay");
    }

    void NewVirtualEvent<T>(string name) where T : BaseVirtualEvent
    {
        Debug.Log("New event of type " + typeof(T) + ": " + name);


        T newEvent = ScriptableObject.CreateInstance<T>();
        newEvent.name = name;
        newEvent.MoveToPos(contextMenuMousePos);

        if(ongoingLink.source != null)
        {
            ongoingLink.BuildLink(newEvent);
        }

        graph.CreateAndAddVirtualEvent<T>(newEvent);

        MarkSceneAsDirty();
    }

    void RemoveItem(EventGraphWindowItem item)
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
                    {
                        graph.RemoveVirtualEvent(obj as BaseVirtualEvent);
                    }
                    else if (obj is BasePhysicalEvent)
                    {
                        graph.RemovePhysicalEvent(obj as BasePhysicalEvent);
                    }
                    else
                        Debug.LogError("Removing a graph item that holds an undefined object.");

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
            GUILayout.Label("Graph nodes: " + (graph.virtualEvents.Count + graph.physicalEvents.Count));
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
            if (name == "" || name == DelayedMoment.DEFAULT_NAME)
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

public struct OngoingLink
{
    public EventGraphWindowItem source;
    public int momentIndex;
    public void Draw(Vector2 mousePosition)
    {
        if (!CanDraw)
            return;
        source.DrawOngoingLink(momentIndex, mousePosition);
    }
    public bool CanDraw { get { return source != null; } }

    public void BuildLink(EventGraphWindowItem other)
    {
        if(source != null && (other.myEvent is IEvent))
        {
            IEvent iEvent = other.myEvent as IEvent;
            source.moments[momentIndex].moment.AddIEvent(iEvent);
        }
        source = null;
    }
    public void BuildLink(UnityEngine.Object other)
    {
        if (source != null && (other is IEvent))
        {
            IEvent iEvent = other as IEvent;
            source.moments[momentIndex].moment.AddIEvent(iEvent);
        }
        source = null;
    }
}

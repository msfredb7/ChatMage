using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using System.Linq;

namespace GameEvents
{
    public class EventGraphWindow : EditorWindow
    {
        public EventGraph graph;
        public OngoingLink ongoingLink;

        private List<EventGraphWindowItem> items;
        private Vector2 contextMenuMousePos;
        private Vector2 lastMousePos;
        private EventGraphWindowItem lastHilighted;
        private INodedEvent copyNode;
        private GenericMenu contextMenu;

        [UnityEditor.MenuItem("The Time Drifter/Event Graph")]
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
            BuildContextMenu();
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
            ongoingLink.source = null;
            items = null;
            lastHilighted = null;
            copyNode = null;
            Repaint();
        }

        void RebuildItems()
        {
            ongoingLink.source = null;
            lastHilighted = null;

            graph.events.RemoveNulls();

            items = new List<EventGraphWindowItem>(graph.events.Count);
            for (int i = 0; i < graph.events.Count; i++)
            {
                UnityEngine.Object obj = graph.events[i];
                if (obj is INodedEvent)
                    NewItem(obj as INodedEvent);
            }

            Repaint();
        }

        EventGraphWindowItem NewItem(INodedEvent theEvent)
        {
            EventGraphWindowItem newItem = new EventGraphWindowItem(theEvent, RemoveItem, this);
            items.Add(newItem);
            return newItem;
        }

        public void MarkSceneAsDirty()
        {
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        public Rect graphInfoRect = new Rect(0, 0, 240, 10);

        void OnGUI()
        {
            Event e = Event.current;
            EventType eventPastType = e.type;

            lastMousePos = e.mousePosition;

            if (e.type == EventType.ValidateCommand)
            {
                if (e.commandName == "Copy")
                {
                    Copy();
                    e.Use();
                }
                else if (e.commandName == "Paste")
                {
                    Paste();
                    e.Use();
                }
            }
            //On ouvre le minimenu
            else if (e.type == EventType.ContextClick)
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
                    if (!graphInfoRect.Contains(lastMousePos))
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
                    if (!graphInfoRect.Contains(lastMousePos))
                    {
                        ClearFocus();
                        Repaint();
                    }

                    HighLightItem(null);
                }
                else
                {
                    //Change selected item
                    if (Selection.activeObject != newSelection.myEvent.AsObject())
                    {
                        SetSelectedItem(newSelection);
                        Repaint();
                    }

                    HighLightItem(newSelection);
                    ClearFocus();
                }
            }
            else if (e.type == EventType.MouseDrag)
            {
                if (e.button == 2)
                {
                    Vector2 delta = e.delta;
                    for (int i = 0; i < items.Count; i++)
                    {
                        Rect rect = items[i].WindowRect;
                        rect.position = rect.position + delta;
                        items[i].WindowRect = rect;
                    }
                    Repaint();
                }
            }

            // The position of the window
            BeginWindows();

            //Window Count
            int winC = 0;

            //Graph info
            graphInfoRect = GUILayout.Window(winC++, graphInfoRect, Window_GraphInfo, "Graph Info");

            //Items !
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].myEvent == null || items[i].myEvent.AsObject() == null)
                    {
                        graph.events.RemoveNulls();
                        ClearItems(false);
                        break;
                    }
                    GUI.backgroundColor = items[i].myEvent.GUIColor();
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

        void NewVirtualEvent(Type type)
        {
            Rect popupRect = new Rect(position.position + contextMenuMousePos + new Vector2(-70, -90), new Vector2(210, 90));
            try
            {
                PopupWindow.Show(popupRect, new EventNamePopup(
                    delegate (string name)
                    {
                        PopupWindow.focusedWindow.Close();
                        if (graph.CheckForNameDuplicate(name))
                            Debug.LogWarning("Attention, la nouvelle node utilise le meme nom qu'une autre node."
                                + " Ceci peut vous empecher d'appeler l'event manuellement par nom.");
                        NewVirtualEvent(type, name);
                    }));
            }
            catch { }
        }
        void NewVirtualEvent(Type type, string name)
        {
            INodedEvent newEvent = ScriptableObject.CreateInstance(type) as INodedEvent;
            NewVirtualEvent(newEvent, name);
        }

        void NewVirtualEvent(INodedEvent newEvent, string name)
        {
            newEvent.name = name;
            newEvent.MoveToPos(contextMenuMousePos);

            if (ongoingLink.source != null)
            {
                ongoingLink.BuildLink(newEvent.AsObject());
            }

            newEvent.LinkToGraph();

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
                        PopupWindow.focusedWindow.Close();

                        UnityEngine.Object obj = item.myEvent.AsObject();
                        DestroyImmediate(obj);
                        SetSelectedItem(null);

                        MarkSceneAsDirty();
                    }));
            }
            catch { }
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
                GUILayout.Label("Graph nodes: " + graph.events.Count);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Rebuild"))
                {
                    SetGraph(graph);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        void BuildContextMenu()
        {
            contextMenu = new GenericMenu();

            Assembly gameAssembly = typeof(Game).Assembly;
            var virtualTypes = from type in gameAssembly.GetTypes()
                               where typeof(VirtualEvent).IsAssignableFrom(type) || typeof(FIVirtualEvent).IsAssignableFrom(type)
                               select type;

            foreach (var type in virtualTypes)
            {
                if (type == typeof(VirtualEvent) || type == typeof(FIVirtualEvent))
                    continue;

                object[] menuItemAt = type.GetCustomAttributes(typeof(MenuItem), false);

                if (menuItemAt.Length <= 0)
                    continue;

                MenuItem nodeItem = menuItemAt[0] as MenuItem;

                contextMenu.AddItem(new GUIContent(nodeItem.menuItem), false, delegate ()
                {
                    object[] nodeNameAt = type.GetCustomAttributes(typeof(DefaultNodeName), false);
                    if (nodeNameAt.Length > 0)
                    {
                        string name = (nodeNameAt[0] as DefaultNodeName).nodeName;
                        NewVirtualEvent(type, name);
                    }
                    else
                    {
                        NewVirtualEvent(type);
                    }
                });
            }

            contextMenu.AddItem(new GUIContent("Recenter nodes"), false, RecenterNodes);
        }

        void OpenContextMenu()
        {
            if (graph == null)
                return;

            if (contextMenu == null)
                BuildContextMenu();

            contextMenu.ShowAsContext();
        }

        void Copy()
        {
            if (lastHilighted == null)
                Debug.Log("Cannot copy. No selected node.");
            else
            {
                copyNode = lastHilighted.myEvent;
            }
        }
        void Paste()
        {
            if (copyNode == null || copyNode.AsObject() == null)
                Debug.Log("Cannot paste. No copied node.");
            else
            {
                INodedEvent newEvent = Instantiate(copyNode.AsObject()) as INodedEvent;

                newEvent.LinkToGraph();
                newEvent.MoveToPos(copyNode.WindowRect.position + Vector2.one * 75);

                MarkSceneAsDirty();
            }
        }

        public void SetSelectedItem(EventGraphWindowItem item)
        {
            if (item == null)
            {
                Selection.SetActiveObjectWithContext(null, null);
            }
            else
            {
                Selection.SetActiveObjectWithContext(item.myEvent.AsObject(), null);
            }
        }

        public void HighLightItem(EventGraphWindowItem item)
        {
            if (lastHilighted != null)
                lastHilighted.isHilighted = false;

            lastHilighted = item;

            if (lastHilighted != null)
                lastHilighted.isHilighted = true;
        }

        public void RecenterNodes()
        {
            float x = 0;
            float y = 0;

            for (int i = 0; i < items.Count; i++)
            {
                Vector2 pos = items[i].WindowRect.position;
                x += pos.x;
                y += pos.y;
            }

            x /= items.Count;
            y /= items.Count;
            Vector2 moyenne = new Vector2(x, y);
            Vector2 center = position.size / 2;
            Vector2 delta = (center - moyenne).Rounded();

            for (int i = 0; i < items.Count; i++)
            {
                Rect rect = items[i].WindowRect;
                rect.position = rect.position + delta;
                float posX = rect.position.x;
                float posY = rect.position.y;
                Vector2 newPos = new Vector2(posX, posY).Clamped(Vector2.zero, position.size * 0.95f);
                rect.position = newPos;
                items[i].WindowRect = rect;
            }
        }
    }

    public class EventNamePopup : PopupWindowContent
    {
        string name = "";
        string error = "";
        Action<string> onSuccess;

        public override Vector2 GetWindowSize()
        {
            return new Vector2(210, 90);
        }

        public EventNamePopup(Action<string> onSuccess)
        {
            this.onSuccess = onSuccess;
        }
        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Object Name", EditorStyles.boldLabel);

            GUI.SetNextControlName("NameField");
            name = EditorGUILayout.TextField(name);
            EditorGUI.FocusTextInControl("NameField");

            if (GUILayout.Button("Submit") || (Event.current.isKey && Event.current.keyCode == KeyCode.Return))
            {
                if (name == "")
                {
                    error = "Invalid name";
                }
                else
                {
                    onSuccess(name);
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
            if (source == null)
                return;
            BuildLink(other.myEvent.AsObject());
        }
        public void BuildLink(UnityEngine.Object other)
        {
            Type otherType = other.GetType();
            if (otherType == typeof(IBaseEvent) || !(other is IBaseEvent) || source == null)
            {
                Debug.LogError("Cette node n'accepte pas les liens 'Moment'. Elle n'implémente pas l'interface IEvent.");
            }
            else
            {
                BaseMoment moment = source.moments[momentIndex].moment;
                Type[] interfaces = otherType.FindInterfaces(
                    InterfaceFilter,
                    otherType);

                for (int i = 0; i < interfaces.Length; i++)
                {
                    Type[] types = interfaces[i].GetGenericArguments();
                    if (moment.MatchWithGenericTypes(types))
                    {
                        source.moments[momentIndex].moment.AddIEvent(other);
                        break;
                    }
                }
            }
            source = null;
        }

        private bool InterfaceFilter(Type t, System.Object criteriaObj)
        {
            return t.GetInterfaces().Contains(typeof(IBaseEvent));
        }
    }
}
using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace GameEvents
{
    public class EventGraphWindowItem
    {
        public class NamedMoments
        {
            public string displayName;
            public BaseMoment moment;
            public Vector2 lastDrawnPos;
            public Type[] genericTypes;
            public NamedMoments(BaseMoment moment, string varName)
            {
                this.moment = moment;
                this.genericTypes = moment.GetGenericTypes();
                displayName = varName + "(";
                for (int i = 0; i < genericTypes.Length; i++)
                {
                    if (i > 0)
                        displayName += ", ";
                    displayName += genericTypes[i].Name;
                }
                displayName += ")";
            }
        }
        public const float MOMENT_BUTTON_WIDTH = 20;
        public const float MOMENT_BUTTON_HEIGHT = 18;

        private const string DRAG_KEY = "egwid"; //Pour Event Graph Window Item Drag
        public INodedEvent myEvent;
        public List<NamedMoments> moments;
        public bool isHilighted = false;

        private bool collapsed = true;
        public Type[] entryTypes;

        Action<EventGraphWindowItem> removeRequest;
        EventGraphWindow parentWindow;

        public EventGraphWindowItem(INodedEvent myEvent, Action<EventGraphWindowItem> removeRequest, EventGraphWindow window)
        {
            this.myEvent = myEvent;
            if (myEvent == null)
                throw new Exception("my event == null");

            this.removeRequest = removeRequest;
            this.parentWindow = window;

            BuildEntryTypes();
            BuildNamedMoments();
        }

        private void BuildEntryTypes()
        {
            entryTypes = myEvent.GetType().FindInterfaces(
                (Type t, System.Object obj) => t.GetInterfaces().Contains(typeof(IBaseEvent)),
                null);
        }

        private void BuildNamedMoments()
        {
            //On cherche a travers tous les membres, lesquels sont de type 'MomentLauncher'
            Type myEventType = myEvent.GetType();
            FieldInfo[] allFields = myEventType.GetFields();
            moments = new List<NamedMoments>();
            for (int i = 0; i < allFields.Length; i++)
            {
                if (allFields[i].FieldType.IsSubclassOf(typeof(BaseMoment)))
                {
                    BaseMoment moment = allFields[i].GetValue(myEvent) as BaseMoment;
                    moments.Add(new NamedMoments(moment, allFields[i].Name));
                }
            }

            BaseMoment[] additionalMoments;
            string[] additionalNames;
            myEvent.GetAdditionalMoments(out additionalMoments, out additionalNames);

            if (additionalMoments != null && additionalNames != null)
            {
                int count = Mathf.Min(additionalNames.Length, additionalMoments.Length);
                moments.Capacity = moments.Count + count;
                for (int i = 0; i < count; i++)
                {
                    moments.Add(new NamedMoments(additionalMoments[i], additionalNames[i]));
                }
            }

            myEvent.ResetWindowRectSize();
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
            get { return myEvent.NodeLabel(); }
        }

        public void DrawNode(int unusedWindowId)
        {
            Event e = Event.current;

            EditorGUILayout.BeginHorizontal();

            if (myEvent is IBaseEvent && GUILayout.Button(">"))
            {
                if (e.button == 0)
                {
                    parentWindow.ongoingLink.BuildLink(this);
                    parentWindow.MarkSceneAsDirty();
                }
                else
                {
                    parentWindow.graph.RemoveAllLinksTo(myEvent as IBaseEvent);
                }
            }

            //Buttons: - et x
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-"))
            {
                collapsed = !collapsed;
                myEvent.ResetWindowRectSize();
            }
            if (myEvent.CanBeManuallyDestroyed() && GUILayout.Button("x"))
            {
                removeRequest(this);
                return;
            }
            EditorGUILayout.EndHorizontal();


            if (!collapsed)
            {
                //---------------Entries---------------//
                for (int i = 0; i < entryTypes.Length; i++)
                {
                    Type[] genericArgs = entryTypes[i].GetGenericArguments();
                    string text = "";
                    for (int u = 0; u < genericArgs.Length; u++)
                    {
                        if (u > 0)
                            text += ", ";
                        text += genericArgs[u].Name;
                    }
                    GUILayout.Label("(" + text + ")");
                }


                //---------------Reference Box---------------//

                EditorGUILayout.ObjectField(myEvent.AsObject(), myEvent.AsObject().GetType(), true);
                if (isHilighted &&
                    GUILayoutUtility.GetLastRect().Contains(e.mousePosition) &&
                    e.type == EventType.MouseDrag &&
                    e.button == 0)
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new UnityEngine.Object[] { myEvent.AsObject() };
                    DragAndDrop.StartDrag(DRAG_KEY);
                    e.Use();
                }
            }


            //---------------Moment---------------//
            for (int i = 0; i < moments.Count; i++)
            {
                if (moments[i].moment == null)
                {
                    moments.RemoveAt(i);
                    i--;
                }
                else
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();
                    GUILayout.Label(moments[i].displayName);
                    if (GUILayout.Button(">"))
                    {
                        if (e.button == 0)
                        {
                            parentWindow.ongoingLink.source = this;
                            parentWindow.ongoingLink.momentIndex = i;
                        }
                        else
                        {
                            moments[i].moment.ClearMoments();
                        }

                    }
                    moments[i].lastDrawnPos = GUILayoutUtility.GetLastRect().position;
                    GUILayout.EndHorizontal();
                }
            }

            //---------------Drag---------------//

            if (e.button == 0)
                GUI.DragWindow();

        }

        public void DrawLinks()
        {
            Handles.BeginGUI();

            for (int i = 0; i < moments.Count; i++)
            {
                BaseMoment moment = moments[i].moment;
                for (int u = 0; u < moment.iEvents.Count; u++)
                {
                    UnityEngine.Object otherNode = moment.iEvents[u];
                    if (otherNode is INodedEvent)
                    {
                        INodedEvent otherDisplay = otherNode as INodedEvent;
                        Rect targetRect = otherDisplay.WindowRect;
                        DrawBezierRight(WindowRect.position + moments[i].lastDrawnPos,
                            new Vector2(targetRect.xMin, targetRect.y + 28));
                    }
                    else
                    {
                        Debug.LogWarning("Le moment apparenant a: " + NodeLabel + " a un lien vers un IEvent qui n'est pas IEventDisplay."
                            + " Destruction du lien.");
                        moment.RemoveNulls();
                        break;
                    }
                }
            }

            Handles.EndGUI();
        }

        public void DrawOngoingLink(int momentIndex, Vector2 mousePosition)
        {
            Handles.BeginGUI();

            DrawBezierRight(moments[momentIndex].lastDrawnPos + WindowRect.position, mousePosition);

            Handles.EndGUI();
        }

        private void DrawBezierRight(Vector2 from, Vector2 to)
        {
            from += Vector2.right * MOMENT_BUTTON_WIDTH + Vector2.up * MOMENT_BUTTON_HEIGHT / 2;

            float xDif = (from.x - to.x).Abs();
            Vector2 startTangent = from + (Vector2.right * xDif / 2);
            Vector2 endTangent = to + (Vector2.left * xDif / 2);

            Handles.DrawBezier(from, to, startTangent, endTangent, new Color(0.6f, 0, 0.6f), null, 2);
        }

        public void OpenContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            //menu.AddItem(new GUIContent("Reset Rect Size"), false, myEvent.ResetWindowRectSize);
            //menu.AddItem(new GUIContent("Reset Rect Position"), false, myEvent.ResetWindowRectPos);

            menu.AddItem(new GUIContent("Rename"), false, Rename);

            menu.AddItem(new GUIContent("Remove outgoing links"), false, delegate ()
                {
                    for (int i = 0; i < moments.Count; i++)
                    {
                        moments[i].moment.ClearMoments();
                    }
                });

            if (myEvent is IBaseEvent)
                menu.AddItem(new GUIContent("Remove incoming links"), false, delegate ()
                {
                    parentWindow.graph.RemoveAllLinksTo(myEvent as IBaseEvent);
                });

            menu.AddItem(new GUIContent("Rebuild"), false, () => { BuildEntryTypes(); BuildNamedMoments(); });

            menu.ShowAsContext();
        }

        void Rename()
        {
            Rect popupRect = new Rect(WindowRect.position, new Vector2(210, 90));
            try
            {
                PopupWindow.Show(popupRect, new EventNamePopup(
                    delegate (string name)
                    {
                        PopupWindow.focusedWindow.Close();
                        myEvent.AsObject().name = name;
                        parentWindow.MarkSceneAsDirty();
                    }));
            }
            catch { }
        }
    }
}
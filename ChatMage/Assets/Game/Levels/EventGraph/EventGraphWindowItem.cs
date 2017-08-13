using UnityEditor;
using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace GameEvents
{
    public class EventGraphWindowItem
    {
        public class NamedMoments
        {
            public string displayName;
            public Moment moment;
            public Vector2 lastDrawnPos;
        }
        public const float MOMENT_BUTTON_WIDTH = 20;
        public const float MOMENT_BUTTON_HEIGHT = 18;

        private const string DRAG_KEY = "egwidr"; //Pour Event Graph Window Item DRag
        public INodedEvent myEvent;
        public List<NamedMoments> moments;
        public bool isHilighted = false;

        Action<EventGraphWindowItem> removeRequest;
        EventGraphWindow parentWindow;

        public EventGraphWindowItem(INodedEvent myEvent, Action<EventGraphWindowItem> removeRequest, EventGraphWindow window)
        {
            this.myEvent = myEvent;
            if (myEvent == null)
                throw new Exception("my event == null");

            this.removeRequest = removeRequest;
            this.parentWindow = window;

            BuildNamedMoments();
        }

        private void BuildNamedMoments()
        {
            //On cherche a travers tous les membres, lesquels sont de type 'MomentLauncher'
            Type myEventType = myEvent.GetType();
            FieldInfo[] allFields = myEventType.GetFields();
            moments = new List<NamedMoments>();
            for (int i = 0; i < allFields.Length; i++)
            {
                if (allFields[i].FieldType == typeof(Moment))
                    moments.Add(new NamedMoments()
                    {
                        displayName = allFields[i].Name,
                        moment = allFields[i].GetValue(myEvent) as Moment
                    });
            }

            Moment[] additionalMoments;
            string[] additionalNames;
            myEvent.GetAdditionalMoments(out additionalMoments, out additionalNames);

            if (additionalMoments != null && additionalNames != null)
            {
                int count = Mathf.Min(additionalNames.Length, additionalMoments.Length);
                moments.Capacity = moments.Count + count;
                for (int i = 0; i < count; i++)
                {
                    moments.Add(new NamedMoments()
                    {
                        displayName = additionalNames[i],
                        moment = additionalMoments[i]
                    });
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
            get { return myEvent.name; }
        }

        public void DrawNode(int unusedWindowId)
        {
            Event e = Event.current;

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(myEvent.TypeLabel(), EditorStyles.miniLabel);

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
            if (isHilighted && GUILayoutUtility.GetLastRect().Contains(e.mousePosition) && e.type == EventType.MouseDrag)
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new UnityEngine.Object[] { myEvent.AsObject() };
                DragAndDrop.StartDrag(DRAG_KEY);
                e.Use();
            }


            //---------------Moment launchers---------------//
            for (int i = 0; i < moments.Count; i++)
            {
                if (moments[i].moment == null)
                {
                    moments.RemoveAt(i);
                    i--;
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label(moments[i].displayName);
                    GUILayout.FlexibleSpace();
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
                    EditorGUILayout.EndHorizontal();
                }
            }



            //---------------Drag---------------//

            if (e.button != 1)
                GUI.DragWindow();

        }

        public void DrawLinks()
        {
            Handles.BeginGUI();

            for (int i = 0; i < moments.Count; i++)
            {
                Moment moment = moments[i].moment;
                for (int u = 0; u < moment.iEvents.Count; u++)
                {
                    UnityEngine.Object iEvent = moment.iEvents[u];
                    if (iEvent is INodedEvent)
                    {
                        INodedEvent otherDisplay = iEvent as INodedEvent;
                        Rect targetRect = otherDisplay.WindowRect;
                        DrawBezierRight(WindowRect.position + moments[i].lastDrawnPos,
                            new Vector2(targetRect.xMin, targetRect.y + 28));
                    }
                    else
                    {
                        Debug.LogWarning("Le moment apparenant a: " + NodeLabel + " a un lien vers un IEvent qui n'est pas IEventDisplay."
                            + "dis is weird.");
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

            if (myEvent is IEvent)
                menu.AddItem(new GUIContent("Remove incoming links"), false, delegate ()
                {
                    parentWindow.graph.RemoveAllLinksTo(myEvent as IEvent);
                });

            menu.AddItem(new GUIContent("Refresh moments"), false, BuildNamedMoments);

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
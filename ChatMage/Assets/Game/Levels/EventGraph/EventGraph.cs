using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace GameEvents
{
    public class EventGraph : MonoBehaviour
    {
        [Header("NE PAS TOUCHER A LA LISTE")]
        [ReadOnly(forwardToChildren = false)]
        public List<Object> events = new List<Object>();

        public event SimpleEvent onEventsAddedOrRemoved;

        public bool CheckForNameDuplicate(string name)
        {
            if (events == null)
                return false;

            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].name == name)
                    return true;
            }
            return false;
        }

        public INodedEvent AddEvent(INodedEvent existingEvent)
        {
            events.Add(existingEvent.AsObject());
            existingEvent.Graph = this;

            if (onEventsAddedOrRemoved != null)
                onEventsAddedOrRemoved();
            return existingEvent;
        }

        public bool ContainsEvent(INodedEvent existingEvent)
        {
            return events.Contains(existingEvent.AsObject());
        }

        public void RemoveEvent(INodedEvent theEvent)
        {
            if (theEvent is IBaseEvent)
                RemoveAllLinksTo(theEvent as IBaseEvent);

            if (events.Remove(theEvent.AsObject()))
            {
                if (onEventsAddedOrRemoved != null)
                    onEventsAddedOrRemoved();
            }
        }

        public void RemoveAllLinksTo(IBaseEvent theEvent)
        {
            for (int i = 0; i < events.Count; i++)
            {
                RemoveAllLinksTo(theEvent, events[i]);
            }
        }

        private void RemoveAllLinksTo(IBaseEvent theEvent, Object on)
        {
            FieldInfo[] fields = on.GetType().GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType.IsSubclassOf(typeof(BaseMoment)))
                {
                    BaseMoment moment = fields[i].GetValue(on) as BaseMoment;
                    moment.RemoveIEvent(theEvent.AsObject());
                }
            }


            if (on is INodedEvent)
            {
                INodedEvent onDisplay = on as INodedEvent;

                BaseMoment[] additionalMoments;
                string[] additionalNames;
                onDisplay.GetAdditionalMoments(out additionalMoments, out additionalNames);

                if (additionalMoments != null)
                {
                    for (int i = 0; i < additionalMoments.Length; i++)
                    {
                        additionalMoments[i].RemoveIEvent(theEvent.AsObject());
                    }
                }
            }
        }

    }

    public static class Colors
    {
        public static Color FLOW_CONTROL
        {
            get { return new Color(0.9f, 1, 1, 1); }
        }

        public static Color MAP
        {
            get { return new Color(0.65f, 0.65f, 1, 1); }
        }

        public static Color LEVEL_SCRIPT
        {
            get { return new Color(0.95f, 0.65f, 0, 1); }
        }

        public static Color VARIABLES
        {
            get { return new Color(0.67f, 0.67f, 0.67f, 1); }
        }

        public static Color WAVES
        {
            get { return new Color(1, 0.5f, 0.5f, 1); }
        }

        public static Color DIALOG
        {
            get { return new Color(1, 1, 0.5f, 1); }
        }

        public static Color DELAY
        {
            get { return new Color(1, 0.7f, 1, 1); }
        }

        public static Color CAMERA
        {
            get { return new Color(1f, 0.8f, 0.8f, 1); }
        }

        public static Color AI
        {
            get { return new Color(0.75f, 0.8f, 1f, 1); }
        }

        public static Color MILESTONE
        {
            get { return new Color(.8f, 10, .8f, 1); }
        }
    }
}
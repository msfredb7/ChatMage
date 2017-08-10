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
                return true;

            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].name == name)
                    return false;
            }
            return true;
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
            RemoveAllLinksTo(theEvent);

            if (events.Remove(theEvent.AsObject()))
            {
                if (onEventsAddedOrRemoved != null)
                    onEventsAddedOrRemoved();
            }
        }

        public void RemoveNulls()
        {
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i] == null)
                {
                    events.RemoveAt(0);
                    i--;
                }
            }
        }

        public void RemoveAllLinksTo(IEvent theEvent)
        {
            for (int i = 0; i < events.Count; i++)
            {
                RemoveAllLinksTo(theEvent, events[i]);
            }
        }

        private void RemoveAllLinksTo(IEvent theEvent, Object on)
        {
            FieldInfo[] fields = on.GetType().GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == typeof(Moment))
                {
                    Moment moment = fields[i].GetValue(on) as Moment;
                    moment.RemoveIEvent(theEvent);
                }
            }


            if (on is INodedEvent)
            {
                INodedEvent onDisplay = on as INodedEvent;

                Moment[] additionalMoments;
                string[] additionalNames;
                onDisplay.GetAdditionalMoments(out additionalMoments, out additionalNames);

                if (additionalMoments != null)
                {
                    for (int i = 0; i < additionalMoments.Length; i++)
                    {
                        additionalMoments[i].RemoveIEvent(theEvent);
                    }
                }
            }
        }

    }
}
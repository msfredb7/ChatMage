using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class EventGraph : MonoBehaviour
{
    [Header("NE PAS TOUCHER AU LISTES")]
    [ReadOnly(forwardToChildren = false)]
    public List<BaseVirtualEvent> virtualEvents;
    [ReadOnly(forwardToChildren = false)]
    public List<BasePhysicalEvent> physicalEvents;

    public event SimpleEvent onEventsAddedOrRemoved;

    public bool IsVirtualNameAvailable(string name)
    {
        if (virtualEvents == null)
            return true;

        for (int i = 0; i < virtualEvents.Count; i++)
        {
            if (virtualEvents[i].name == name)
                return false;
        }
        return true;
    }

    public T CreateAndAddVirtualEvent<T>(T newEvent) where T : BaseVirtualEvent
    {
        virtualEvents.Add(newEvent);

        if (onEventsAddedOrRemoved != null)
            onEventsAddedOrRemoved();
        return newEvent;
    }

    public void RemoveVirtualEvent(BaseVirtualEvent theEvent)
    {
        RemoveAllLinksTo(theEvent);

        if (virtualEvents.Remove(theEvent))
        {
            if (onEventsAddedOrRemoved != null)
                onEventsAddedOrRemoved();
        }
    }

    public BasePhysicalEvent AddPhysicalEvent(BasePhysicalEvent existingEvent)
    {
        physicalEvents.Add(existingEvent);
        existingEvent.graph = this;

        if (onEventsAddedOrRemoved != null)
            onEventsAddedOrRemoved();
        return existingEvent;
    }

    public bool ContainsPhysicalEvent(BasePhysicalEvent existingEvent)
    {
        return physicalEvents.Contains(existingEvent);
    }

    public void RemovePhysicalEvent(BasePhysicalEvent theEvent)
    {
        RemoveAllLinksTo(theEvent);

        if (physicalEvents.Remove(theEvent))
        {
            if (onEventsAddedOrRemoved != null)
                onEventsAddedOrRemoved();
        }
    }

    public void RemoveNulls()
    {
        for (int i = 0; i < virtualEvents.Count; i++)
        {
            if(virtualEvents[i] == null)
            {
                virtualEvents.RemoveAt(0);
                i--;
            }
        }
        for (int i = 0; i < physicalEvents.Count; i++)
        {
            if (physicalEvents[i] == null)
            {
                physicalEvents.RemoveAt(0);
                i--;
            }
        }
    }

    
    public void RemoveAllLinksTo(IEvent theEvent)
    {
        for (int i = 0; i < virtualEvents.Count; i++)
        {
            RemoveAllLinksTo(theEvent, virtualEvents[i]);
        }
        for (int i = 0; i < physicalEvents.Count; i++)
        {
            RemoveAllLinksTo(theEvent, physicalEvents[i]);
        }
    }

    private void RemoveAllLinksTo(IEvent theEvent, Object on)
    {
        FieldInfo[] fields = on.GetType().GetFields();

        for (int i = 0; i < fields.Length; i++)
        {
            if(fields[i].FieldType == typeof(Moment))
            {
                Moment moment = fields[i].GetValue(on) as Moment;
                moment.RemoveIEvent(theEvent);
            }
        }


        if(on is IEventDisplay)
        {
            IEventDisplay onDisplay = on as IEventDisplay;

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
